using System;
using System.Collections.Generic;
using System.Text;
using Resend;
using AplicationLogic.Interfaces;

namespace AplicationLogic.Services.Email
{
    public class ResendEmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly string _emailOrigen = "schedule@updates.riosrenato.com";

        public ResendEmailService(IResend resend)
        {
            _resend = resend;
        }
        public async Task SendAppointmentConfirmationAsync(string toEmail, string clientName, DateTime dateAppointment)
        {
            var subject = "Date Confirmin";
            var htmlContent = $"<p>Hi {clientName},</p><p>Your date was confirm to {dateAppointment:MMMM dd, yyyy} at {dateAppointment:hh:mm tt}.</p><p>¡Thank you to choosing us!</p>";
            var message = new EmailMessage
            {
                From = _emailOrigen,
                To = { toEmail },
                Subject = subject,
                HtmlBody = htmlContent
            };
           /* await _resend.EmailSendAsync(message);*/
        }

        public async Task SendReminderAppointmentAsync(string toEmail, string clientName, DateTime dateAppointment, string hairstylistName)
        {
            var subject = "Reserve reminder";
            var htmlContent = $"<p>Hi {clientName},</p><p>This is a reminder. You have a date with {hairstylistName} on {dateAppointment:MMMM dd, yyyy} at {dateAppointment:hh:mm tt}.</p><p>¡See you!</p>";
            var message = new EmailMessage
            {
                From = _emailOrigen,
                To = { toEmail },
                Subject = subject,
                HtmlBody = htmlContent
            };
            await _resend.EmailSendAsync(message);
        }
    }
}
