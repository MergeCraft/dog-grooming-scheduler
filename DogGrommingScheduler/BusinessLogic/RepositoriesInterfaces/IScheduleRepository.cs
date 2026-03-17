using BusinessLogic.Entities;
using BusinessLogic.RepositoryInterfaces;
using BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.RepositoriesInterfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<Result<Schedule?>> GetByGroomerAndDateAsync(Guid groomerId, DateTime date);
    }
}
