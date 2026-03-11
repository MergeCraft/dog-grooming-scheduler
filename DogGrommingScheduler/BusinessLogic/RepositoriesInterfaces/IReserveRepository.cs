using BusinessLogic.Entities;
using BusinessLogic.RepositoryInterfaces;
using BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.RepositoryInterfaces
{
    public interface IReserveRepository : IRepository<Reserve>
    {
        Task<Result> AddAsync(Reserve reserve);
    }
}
