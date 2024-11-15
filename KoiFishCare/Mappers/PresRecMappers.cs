using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.PrescriptionRecord;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class PresRecMappers
    {
        public static PrescriptionRecordDto ToPresRecDtoFromModel(this PrescriptionRecord model)
        {
            return new PrescriptionRecordDto
            {
                PrescriptionRecordID = model.PrescriptionRecordID,
                DiseaseName = model.DiseaseName,
                Symptoms = model.Symptoms,
                MedicationDetails = model.MedicationDetails,
                Note = model.Note,
                BookingID = model.BookingID,
                CreateAt = model.CreateAt
            };
        }

        public static PrescriptionRecord ToModelFromCreate(this CreatePresRecordDto dto)
        {
            return new PrescriptionRecord
            {
                DiseaseName = dto.DiseaseName,
                Symptoms = dto.Symptoms,
                MedicationDetails = dto.MedicationDetails,
                Note = dto.Note,
                BookingID = dto.BookingID,
                CreateAt = DateTime.Now
            };
        }

        public static PrescriptionRecord ToModelFromUpdate(this UpdatePresRecordDto dto)
        {
            return new PrescriptionRecord
            {
                DiseaseName = dto.DiseaseName,
                Symptoms = dto.Symptoms,
                MedicationDetails = dto.MedicationDetails,
                Note = dto.Note,
                CreateAt = DateTime.UtcNow
            };
        }
    }
}