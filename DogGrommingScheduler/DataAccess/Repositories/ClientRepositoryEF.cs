using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ClientRepositoryEF : IClientRepository
    {
        private readonly ContextDB _context;

        public ClientRepositoryEF(ContextDB context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Client>>> GetAllAsync()
        {
            try
            {
                var clients = await _context.Clients.ToListAsync();
                return Result<IEnumerable<Client>>.Success(clients);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Client>>.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result<Client>> GetByIdAsync(Guid id)
        {
            try
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                    return Result<Client>.Failure(new[] { new Error("NotFound", "Cliente no encontrado") });

                return Result<Client>.Success(client);
            }
            catch (Exception ex)
            {
                return Result<Client>.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result<Client>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId.ToString()); // <-- Usa la FK, no la PK

                if (client == null)
                    return Result<Client>.Failure(new[] { new Error("NotFound", "No hay un cliente vinculado a este usuario de Identity") });

                return Result<Client>.Success(client);
            }
            catch (Exception ex)
            {
                return Result<Client>.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> AddAsync(Client entity)
        {
            try
            {
                await _context.Clients.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> UpdateAsync(Client entity)
        {
            try
            {
                _context.Clients.Update(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> RemoveAsync(Guid id)
        {
            try
            {
                var entity = await _context.Clients.FindAsync(id);
                if (entity == null) return Result.Failure(new[] { new Error("NotFound", "No existe el registro") });

                _context.Clients.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> RemoveAsync(Client entity)
        {
            try
            {
                _context.Clients.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }
    }
}
