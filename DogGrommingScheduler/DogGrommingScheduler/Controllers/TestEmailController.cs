using AplicationLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEmailController : Controller
    {
        private readonly IEmailService _emailService;
        public TestEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("prueba-confirmacion")]
        public async Task<IActionResult> SendTestConfirmation([FromQuery] string emailDestino)
        {
            // Datos duros de prueba para simular una reserva real
            string clientName = "Renato";
            DateTime dateAppointment = DateTime.Now.AddDays(2);

            try
            {
                // Llamamos a tu método exacto
                await _emailService.SendAppointmentConfirmationAsync(emailDestino, clientName, dateAppointment);

                return Ok(new
                {
                    Mensaje = "¡Éxito! El correo fue enviado a la API de Resend.",
                    Destino = emailDestino
                });
            }
            catch (Exception ex)
            {
                // Si algo falla con la API Key o la red, lo veremos aquí
                return StatusCode(500, new
                {
                    Mensaje = "Ocurrió un error al intentar enviar el correo.",
                    Detalle = ex.Message
                });
            }
        }

    }
}
