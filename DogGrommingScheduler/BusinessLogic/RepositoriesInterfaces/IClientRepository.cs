using BusinessLogic.Entities;
using BusinessLogic.RepositoryInterfaces;
using BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.RepositoriesInterfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Result<Client>> GetByUserIdAsync(Guid userId);
    }
}
