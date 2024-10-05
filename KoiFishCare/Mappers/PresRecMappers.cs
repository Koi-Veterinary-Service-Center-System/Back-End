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
                Medication = model.Medication,
                Note = model.Note,
                RefundMoney = model.RefundMoney,
                RefundPercent = model.RefundPercent,
                BookingID = model.BookingID
            };
        }

        public static PrescriptionRecord ToModelFromCreate(this CreatePresRecordDto dto)
        {
            return new PrescriptionRecord
            {
                DiseaseName = dto.DiseaseName,
                Symptoms = dto.Symptoms,
                Medication = dto.Medication,
                Note = dto.Note,
                RefundMoney = dto.RefundMoney,
                RefundPercent = dto.RefundPercent,
                BookingID = dto.BookingID
            };
        }

        public static PrescriptionRecord ToModelFromUpdate(this UpdatePresRecordDto dto)
        {
            return new PrescriptionRecord
            {
                DiseaseName = dto.DiseaseName,
                Symptoms = dto.Symptoms,
                Medication = dto.Medication,
                Note = dto.Note,
                RefundMoney = dto.RefundMoney,
                RefundPercent = dto.RefundPercent,
                BookingID = dto.BookingID
            };
        }
    }
}