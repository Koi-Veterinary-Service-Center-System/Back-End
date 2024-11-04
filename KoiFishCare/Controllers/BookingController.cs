using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.Mappers;
using Microsoft.VisualBasic;
using KoiFishCare.Models.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using KoiFishCare.Dtos.Booking;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KoiFishCare.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISlotRepository _slotRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IVetSlotRepository _vetSlotRepo;
        private readonly IPrescriptionRecordRepository _presRecRepo;
        private readonly IBookingRecordRepository _recordRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IEmailService _emailService;

        public BookingController(UserManager<User> userManager, ISlotRepository slotRepo,
        IServiceRepository serviceRepo, IBookingRepository bookingRepo,
        IVetSlotRepository vetSlotRepo, IPrescriptionRecordRepository presRecRepo,
        IBookingRecordRepository recordRepo, IPaymentRepository paymentRepo,
        IEmailService emailService)
        {
            _userManager = userManager;
            _slotRepo = slotRepo;
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
            _vetSlotRepo = vetSlotRepo;
            _presRecRepo = presRecRepo;
            _recordRepo = recordRepo;
            _paymentRepo = paymentRepo;
            _emailService = emailService;
        }

        [HttpPost("create-booking")]
        [Authorize(Roles = "Customer, Staff")]
        public async Task<IActionResult> CreateBooking([FromBody] FromCreateBookingDTO createBookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string customerId = "";
            if (createBookingDto.CustomerId != null)
            {
                customerId = createBookingDto.CustomerId;
            }

            // Get logged in user
            var userModel = await _userManager.GetUserAsync(this.User);
            if (userModel == null)
                return BadRequest("Login before booking!");

            if (User.IsInRole("Staff"))
            {
                if (string.IsNullOrEmpty(customerId))
                    return BadRequest("CustomerId is required for staff booking!");

                var customerByStaff = await _userManager.FindByIdAsync(customerId);
                if (customerByStaff == null) return BadRequest("Invalid CustomerId");

                var isCusRole = await _userManager.IsInRoleAsync(customerByStaff, "Customer");
                if (!isCusRole) return BadRequest("The provided user is not a customer");

                userModel = customerByStaff;
            }

            if (userModel.PhoneNumber == null)
                return BadRequest("Update your phone number before booking!");

            // if is customer then can not create if any booking in process
            if (User.IsInRole("Customer"))
            {
                //check if there is any booking in process
                var booking = await _bookingRepo.GetBookingsByCusIdAsync(userModel.Id);
                if (!booking.IsNullOrEmpty())
                    return BadRequest("You already have booking that in process!");
            }

            //check date time
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var currentDateTime = DateTime.UtcNow;

            // Check if booking time is before 12 hours
            var bookingDateTime = createBookingDto.BookingDate.ToDateTime(TimeOnly.MinValue);
            if (bookingDateTime < currentDateTime.AddHours(12))
                return BadRequest("Booking time must be at least 12 hours after today.");

            // Check if booking date is within 365 days from today
            if (createBookingDto.BookingDate > currentDate.AddDays(365))
            {
                return BadRequest("Booking date cannot be more than 365 days from today.");
            }

            // Get Slot
            var slot = await _slotRepo.GetSlotById(createBookingDto.SlotId);
            if (slot == null) return BadRequest("Slot does not exist");

            // Get Service
            var service = await _serviceRepo.GetServiceById(createBookingDto.ServiceId);
            if (service == null) return BadRequest("Service does not exist");

            // Get Payment
            var payment = await _paymentRepo.GetPaymentByID(createBookingDto.PaymentId);
            if (payment == null) return BadRequest("Payment does not exist");

            // Get vet
            var vetUsername = createBookingDto.VetName;
            if (vetUsername != null && !vetUsername.IsNullOrEmpty())
            {
                var vet = await _userManager.FindByNameAsync(vetUsername) as User;
                if (vet == null)
                {
                    return BadRequest("Vet does not exist");
                }

                var vetSlot = await _vetSlotRepo.GetVetSlot(vet.Id, slot.SlotID);
                if (vetSlot == null)
                {
                    return BadRequest("The selected vet slot does not exist.");
                }

                if (vetSlot.isBooked == true)
                {
                    return BadRequest("The selected vet is already booked");
                }

                var bookingModel = createBookingDto.ToBookingFromCreate(slot, service, payment, userModel.Id, vet.Id);

                // FIX: Convert nullable TimeOnly to DateTime
                var startDateTime = bookingModel.Slot.StartTime.HasValue
                    ? DateTime.UtcNow.Date.Add(bookingModel.Slot.StartTime.Value.ToTimeSpan())
                    : throw new Exception("Start time is missing");

                var endDateTime = bookingModel.Slot.EndTime.HasValue
                    ? DateTime.UtcNow.Date.Add(bookingModel.Slot.EndTime.Value.ToTimeSpan())
                    : throw new Exception("End time is missing");

                if (service.IsOnline == true)
                {
                    // Create a Google Calendar event and get the Meet link
                    var googleCalendarRequest = new GoogleCalendar
                    {
                        Summary = "Booking: " + service.ServiceName,
                        Description = "Booking with vet " + vet.UserName,
                        Location = "KoiNe",
                        Start = startDateTime, // Using the DateTime after conversion
                        End = endDateTime      // Using the DateTime after conversion
                    };

                    // Vet's credential file path - modify to retrieve actual path if stored in DB or configuration
                    string vetCredentialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "VetCredentials", $"{vet?.Id}_Cre.json");

                    List<string> attendeeEmails = new List<string> { userModel.Email! }; // Add the customer email

                    var calendarEvent = await GoogleCalendarService.CreateGoogleCalendar(googleCalendarRequest, vetCredentialFilePath, vet!.Email!, attendeeEmails);
                    var googleMeetLink = calendarEvent.ConferenceData?.EntryPoints?.FirstOrDefault(e => e.EntryPointType == "video")?.Uri;

                    bookingModel.MeetURL = googleMeetLink;
                }

                bookingModel.Customer = userModel;

                var result = await _bookingRepo.CreateBooking(bookingModel);

                if (vetSlot.VetID == null) return NotFound("Can not find VetId");
                await _vetSlotRepo.Update(vetSlot.VetID, vetSlot.SlotID, true);

                return Ok(result.ToDtoFromModel());
            }
            else
            {
                var availableVet = await _slotRepo.GetAvailableVet(slot);
                if (availableVet == null || string.IsNullOrEmpty(availableVet.Veterinarian!.Email))
                {
                    return BadRequest("No available vet for the chosen slot or vet does not have a valid email.");
                }

                if (availableVet.VetID == null) return NotFound("Can not find VetId");
                var bookingModel = createBookingDto.ToBookingFromCreate(slot, service, payment, userModel.Id, availableVet.VetID);

                // FIX: Convert nullable TimeOnly to DateTime
                var startDateTime = bookingModel.Slot.StartTime.HasValue
                    ? DateTime.UtcNow.Date.Add(bookingModel.Slot.StartTime.Value.ToTimeSpan())
                    : throw new Exception("Start time is missing");

                var endDateTime = bookingModel.Slot.EndTime.HasValue
                    ? DateTime.UtcNow.Date.Add(bookingModel.Slot.EndTime.Value.ToTimeSpan())
                    : throw new Exception("End time is missing");
                if (service.IsOnline == true)
                {
                    // Create a Google Calendar event and get the Meet link
                    var googleCalendarRequest = new GoogleCalendar
                    {
                        Summary = "Booking: " + service.ServiceName,
                        Description = "Booking with vet " + availableVet.Veterinarian,
                        Location = "KoiNe",
                        Start = startDateTime, // Using the DateTime after conversion
                        End = endDateTime      // Using the DateTime after conversion
                    };

                    // Vet's credential file path - modify to retrieve actual path if stored in DB or configuration
                    string vetCredentialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "VetCredentials", $"{availableVet?.VetID}_Cre.json");

                    List<string> attendeeEmails = new List<string> { userModel.Email! }; // Add the customer email

                    var calendarEvent = await GoogleCalendarService.CreateGoogleCalendar(googleCalendarRequest, vetCredentialFilePath, availableVet!.Veterinarian.Email, attendeeEmails);
                    var googleMeetLink = calendarEvent.ConferenceData?.EntryPoints?.FirstOrDefault(e => e.EntryPointType == "video")?.Uri;

                    bookingModel.MeetURL = googleMeetLink;
                }

                bookingModel.Customer = userModel;

                var result = await _bookingRepo.CreateBooking(bookingModel);

                await _vetSlotRepo.Update(availableVet.VetID, availableVet.SlotID, true);

                return Ok(result.ToDtoFromModel());
            }
        }

        [Authorize]
        [HttpGet("view-booking-process")]
        public async Task<IActionResult> GetBookingByID()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var isVet = await _userManager.IsInRoleAsync(user, "Vet");
            var isCus = await _userManager.IsInRoleAsync(user, "Customer");

            if (!isVet && !isCus)
            {
                return Unauthorized("You have no permission to access this feature!");
            }

            if (isVet)
            {
                var bookings = await _bookingRepo.GetBookingByVetIdAsync(user.Id);
                if (bookings == null || !bookings.Any())
                {
                    return NotFound("There is no booking!");
                }

                var result = bookings.Select(x => x.ToDtoFromModel());
                return Ok(result);
            }

            if (isCus)
            {
                var bookings = await _bookingRepo.GetBookingsByCusIdAsync(user.Id);
                if (bookings == null || !bookings.Any())
                {
                    return NotFound("There is no booking!");
                }

                var result = bookings.Select(x => x.ToDtoFromModel());
                return Ok(result);
            }

            return BadRequest("Invalid request");
        }

        [HttpGet("view-booking/{id}")]
        public async Task<IActionResult> GetBookingById([FromRoute] int id)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound("Can not find booking!");
            }
            return Ok(booking.ToDtoFromModel());
        }

        [HttpGet("view-booking-history")]
        [Authorize(Roles = "Customer, Vet")]

        public async Task<IActionResult> GetBookingHistory()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var bookingHistorys = await _bookingRepo.GetBookingByStatusAsync(user.Id);
            if (bookingHistorys == null || !bookingHistorys.Any())
            {
                return NotFound("There is no booking!");
            }

            var result = bookingHistorys.Select(x => x.ToDtoFromModel());
            return Ok(result);
        }

        // [HttpPatch("schedule/{bookingID:int}")]
        // [Authorize(Roles = "Staff")]
        // public async Task<IActionResult> ScheduleBooking(int bookingID)
        // {
        //     var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
        //     if (booking == null)
        //     {
        //         return NotFound("Booking not found!");
        //     }

        //     if (booking.BookingStatus == BookingStatus.Scheduled)
        //     {
        //         return BadRequest("The booking is already scheduled!");
        //     }

        //     booking.BookingStatus = BookingStatus.Scheduled;
        //     await _bookingRepo.UpdateBooking(booking);

        //     return Ok("Booking status updated successfully!");
        // }


        [HttpPatch("ongoing/{bookingID:int}")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> OnGoingBooking(int bookingID)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Booking not found!");
            }

            if (booking.BookingStatus == BookingStatus.Ongoing)
            {
                return BadRequest("The booking is already on-going!");
            }
            else if (booking.BookingStatus != BookingStatus.Scheduled)
            {
                return BadRequest("The booking is not ready to set!");
            }

            if (User.IsInRole("Vet"))
            {
                if (booking.VetID != user.Id)
                {
                    return Unauthorized("You can only set on-going to your own bookings!");
                }
            }

            booking.BookingStatus = BookingStatus.Ongoing;
            await _bookingRepo.UpdateBooking(booking);

            return Ok("Booking status updated successfully!");
        }

        [HttpPatch("complete/{bookingID:int}")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> CompleteServiceBooking(int bookingID)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Booking not found!");
            }

            if (booking.BookingStatus == BookingStatus.Completed)
            {
                return BadRequest("The booking is already completed!");
            }
            else if (booking.BookingStatus != BookingStatus.Ongoing)
            {
                return BadRequest("The booking is not ready to set!");
            }

            if (User.IsInRole("Vet"))
            {
                if (booking.VetID != user.Id)
                {
                    return Unauthorized("You can only complete your own bookings!");
                }
            }

            booking.BookingStatus = BookingStatus.Completed;
            await _bookingRepo.UpdateBooking(booking);

            return Ok("Booking status updated successfully!");
        }

        [HttpPatch("receive-money/{bookingID:int}")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> ReceiveMoneyBooking(int bookingID)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Booking not found!");
            }

            if (booking.BookingStatus == BookingStatus.Received_Money)
            {
                return BadRequest("The booking is already received money!");
            }
            else if (booking.BookingStatus != BookingStatus.Completed)
            {
                return BadRequest("The booking is not ready to set!");
            }

            if (User.IsInRole("Vet"))
            {
                if (booking.VetID != user.Id)
                {
                    return Unauthorized("You can only receive money from your own bookings!");
                }
            }

            booking.BookingStatus = BookingStatus.Received_Money;
            booking.isPaid = true;
            await _bookingRepo.UpdateBooking(booking);

            return Ok("Booking status updated successfully!");
        }

        [HttpPatch("success/{bookingID:int}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SuccessBooking(int bookingID)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Booking not found!");
            }

            if (booking.BookingStatus == BookingStatus.Succeeded)
            {
                return BadRequest("The booking is already succeeded!");
            }
            // else if (booking.BookingStatus != BookingStatus.Received_Money)
            // {
            //     return BadRequest("The booking is not ready to set!");
            // }

            if (User.IsInRole("Customer"))
            {
                if (booking.CustomerID != user.Id)
                {
                    return Unauthorized("You can only success your own bookings!");
                }
            }

            booking.BookingStatus = BookingStatus.Succeeded;
            await _bookingRepo.UpdateBooking(booking);
            await _vetSlotRepo.Update(booking.VetID, booking.SlotID, false);

            return Ok("Booking status updated successfully!");
        }

        [HttpPatch("cancel-booking/{bookingId:int}")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<IActionResult> CancelBooking(int bookingId, [FromBody] CancelBookingDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(this.User);
            if (user == null) return Unauthorized("User is not available!");

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null) return NotFound("Booking not found!");
            if (booking.BookingStatus == BookingStatus.Cancelled || booking.BookingStatus == BookingStatus.Refunded)
                return BadRequest("The booking is already canceled");

            if (User.IsInRole("Customer") && booking.CustomerID != user.Id)
                return Unauthorized("You can only cancel your own bookings.");

            string noteByCentre = User.IsInRole("Staff") ? "The center will contact you within 2 days to refund you if you have already paid." : "";

            var currentDateTime = DateTime.UtcNow;
            var daysBeforeBooking = (booking.BookingDate.ToDateTime(TimeOnly.MinValue) - currentDateTime).Days;
            var timeDifference = (booking.BookingDate.ToDateTime(TimeOnly.MinValue) - currentDateTime).TotalHours;

            bool isCashPayment = booking.Payment.Type.Contains("cash", StringComparison.OrdinalIgnoreCase);

            if (isCashPayment && timeDifference <= 12)
                return BadRequest("Cannot cancel booking less than 12 hours before the appointment.");

            if (!isCashPayment && (booking.BookingStatus != BookingStatus.Scheduled && booking.BookingStatus != BookingStatus.Pending))
                return BadRequest("Cannot cancel this booking.");

            decimal refundPercent;

            // Allow staff to set custom refund percent
            if (User.IsInRole("Staff"))
            {
                refundPercent = dto.RefundPercent ?? 100; // Use the provided RefundPercent or default to 100%
            }
            else
            {
                // Refund logic for customers
                refundPercent = daysBeforeBooking switch
                {
                    > 7 => 100,
                    > 3 => 50,
                    _ => 0
                };
            }

            string refundInfor = string.IsNullOrWhiteSpace(dto.BankName) || string.IsNullOrWhiteSpace(dto.CustomerBankNumber) || string.IsNullOrWhiteSpace(dto.CustomerBankAccountName)
                ? ""
                : dto.BankName + " " + dto.CustomerBankNumber + " " + dto.CustomerBankAccountName;

            var refundMoney = isCashPayment ? 0 : booking.InitAmount * (refundPercent / 100);
            string note = refundPercent == 0 ? "Your booking is not refundable because the cancellation time is too close to the appointment date." : "";

            if (isCashPayment)
            {
                refundPercent = 0;
                refundMoney = 0;
                note = "Cash Payment doesn't have refund!";
            }

            var record = new BookingRecord
            {
                BookingID = booking.BookingID,
                RefundPercent = refundPercent,
                RefundMoney = refundMoney,
                CreateAt = DateTime.Now,
                Note = note + " | " + noteByCentre + " | " + refundInfor
            };

            await _recordRepo.CreateAsync(record);

            booking.BookingStatus = BookingStatus.Cancelled;
            await _bookingRepo.UpdateBooking(booking);
            await _vetSlotRepo.Update(booking.VetID, booking.SlotID, false);

            if (User.IsInRole("Staff"))// Send email for customer if centre cancel booking
            {
                var emailContent = $@"
    <html>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
        <div style='max-width: 600px; margin: auto; background-color: #fff; border-radius: 10px; padding: 20px;'>
            <h2 style='text-align: center;'>Booking Cancellation Confirmation</h2>
            <p>Hello {booking.Customer.FirstName},</p>
            <p>Your booking with ID #{booking.BookingID} scheduled for {booking.BookingDate.ToString("dd MMMM yyyy")} has been successfully canceled.</p>
            <p>{note} {noteByCentre}</p>
            <p>Refund amount: <strong>{refundMoney:C}</strong> (Refund Percentage: {refundPercent}%)</p>
            <p>If you have any questions, please feel free to contact us.</p>
            <p>Thank you,<br>KoiFishCare Team</p>
            <hr style='margin-top: 20px;' />
            <p style='font-size: 12px; text-align: center; color: #777;'>&copy; 2024 KoiFishCare. All rights reserved.</p>
        </div>
    </body>
    </html>";

                await _emailService.SendEmailAsync(booking.Customer.Email!, "Booking Cancellation Confirmation", emailContent);
            }

            return Ok(record.ToDTOFromModel());
        }


        [HttpPatch("refund-booking/{bookingID:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RefundBooking([FromRoute] int bookingID)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Booking not found!");
            }

            if (booking.BookingStatus == BookingStatus.Refunded)
            {
                return BadRequest("The booking is already refunded!");
            }

            booking.BookingStatus = BookingStatus.Refunded;
            await _bookingRepo.UpdateBooking(booking);

            return Ok("Booking status updated successfully!");

        }

        [HttpGet("all-booking")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> GetAllBooking()
        {
            var bookings = await _bookingRepo.GetAllBooking();
            if (!bookings.Any()) return NotFound("No booking!");

            return Ok(bookings);
        }

    }
}