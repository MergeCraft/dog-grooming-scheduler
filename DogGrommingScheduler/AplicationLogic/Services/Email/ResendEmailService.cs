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
        // Nota: Resend requiere que envíes desde un dominio verificado. 
        // Mientras se prueba podemos usar su correo de testing (onboarding@resend.dev)
        private readonly string _emailOrigen = "onboarding@resend.dev";

        public ResendEmailService(IResend resend)
        {
            _resend = resend;
        }
        public async Task SendAppointmentConfirmationAsync(string toEmail, string clientName, DateTime dateAppointment)
        {
            var subject = "Confirmación de cita";
            var htmlContent = $"<p>Hola {clientName},</p><p>Tu cita ha sido confirmada para el {dateAppointment:MMMM dd, yyyy} a las {dateAppointment:hh:mm tt}.</p><p>¡Gracias por elegirnos!</p>";
            var message = new EmailMessage
            {
                From = _emailOrigen,
                To = { toEmail },
                Subject = subject,
                HtmlBody = htmlContent
            };
            await _resend.EmailSendAsync(message);
        }

        public async Task SendReminderAppointmentAsync(string toEmail, string clientName, DateTime dateAppointment, string hairstylistName)
        {
            var subject = "Recordatorio de cita";
            var htmlContent = $"<p>Hola {clientName},</p><p>Este es un recordatorio de tu cita con {hairstylistName} el {dateAppointment:MMMM dd, yyyy} a las {dateAppointment:hh:mm tt}.</p><p>¡Nos vemos pronto!</p>";
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
