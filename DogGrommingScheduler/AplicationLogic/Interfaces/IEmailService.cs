using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationLogic.Interfaces
{
    public interface IEmailService
    {
        Task SendAppointmentConfirmationAsync(string toEmail, string clientName, DateTime dateAppointment);
        Task SendReminderAppointmentAsync(string toEmail, string clientName, DateTime dateAppointment, string hairstylistName);
    }
}
