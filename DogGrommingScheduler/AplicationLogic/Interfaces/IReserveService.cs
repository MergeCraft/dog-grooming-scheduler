using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Entities;

namespace AplicationLogic.Interfaces
{
    public interface IReserveService
    {
        Task ProcessNewReserveAsync(Reserve reserve, string clientEmail, string clientName, string groomerName);
         Task CancelReserveAsync(Reserve reserve, string clientEmail, string clientName, string groomerName);
    }
}
