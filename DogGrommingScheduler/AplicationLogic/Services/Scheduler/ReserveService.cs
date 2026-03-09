using AplicationLogic.Interfaces;
using Hangfire;
using Hangfire.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Entities;

namespace AplicationLogic.Services.Scheduler
{
    public class ReserveService: IReserveService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEmailService _emailService;

        public ReserveService(IBackgroundJobClient backgroundJobs, IEmailService emailService)
        {
            _backgroundJobs = backgroundJobs;
            _emailService = emailService;
        }

        public async Task ProcessNewReserveAsync(Reserve reserve, string clientEmail, string clientName, string groomerName)
        {
            // 1. Aquí iría tu lógica para guardar la reserva en la Base de Datos usando tu Repositorio
            // await _reserveRepository.AddAsync(reserve);

            
            // Enqueue for second plane right now, it will be processed as soon as possible
            _backgroundJobs.Enqueue(() =>
                _emailService.SendAppointmentConfirmationAsync(clientEmail, clientName, reserve.ReservationDate));

            //Schedule send (2 hours before)
            DateTime reminderTime = reserve.ReservationDate.AddHours(-2);

            // Schedule para ejecutarlo en el futuro. Esto nos devuelve un ID único.
            string jobId = _backgroundJobs.Schedule(() =>
                _emailService.SendReminderAppointmentAsync(clientEmail, clientName, reserve.ReservationDate, groomerName),
                reminderTime);

            //if we wanto to cancel the job in the future, we need to store this jobId in the database associated with the reserve
            reserve.ReminderJobId = jobId;

            // Aquí actualizarías la reserva en la base de datos con el nuevo JobId
            // await _reserveRepository.UpdateAsync(reserve);
        }

        public async Task CancelReserveAsync(int reserveId)
        {
            
            // Reserve reserve = await _reserveRepository.GetByIdAsync(reserveId);
            /*      
     

            if (reserve == null)
            {
                throw new Exception("La reserva no existe.");
            }

            if (!string.IsNullOrEmpty(reserve.ReminderJobId))
            {
                bool isDeleted = _backgroundJobs.Delete(reserve.ReminderJobId);
            }
            */

            // Actualizar el estado de la reserva en la base de datos (Ej: Status = "Cancelada")
             //reserve.IsCanceled = true;
             //await _reserveRepository.UpdateAsync(reserve);
        }

    }
}
