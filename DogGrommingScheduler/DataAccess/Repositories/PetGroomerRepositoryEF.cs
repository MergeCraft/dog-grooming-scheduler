using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PetGroomerRepositoryEF : IPetGroomerRepository
    {
        private readonly ContextDB _context;

        public PetGroomerRepositoryEF(ContextDB context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<PetGroomer>>> GetAllAsync()
        {
            try
            {
                var groomers = await _context.PetGroomers
                    .Include(g => g.User)
                    .ToListAsync();

                return Result<IEnumerable<PetGroomer>>.Success(groomers);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PetGroomer>>.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result<PetGroomer>> GetByIdAsync(Guid id)
        {
            try
            {
                var groomer = await _context.PetGroomers.FindAsync(id);
                if (groomer == null)
                    return Result<PetGroomer>.Failure(new[] { new Error("NotFound", "Peluquero no encontrado") });

                return Result<PetGroomer>.Success(groomer);
            }
            catch (Exception ex)
            {
                return Result<PetGroomer>.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> AddAsync(PetGroomer entity)
        {
            try
            {
                await _context.PetGroomers.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> UpdateAsync(PetGroomer entity)
        {
            try
            {
                _context.PetGroomers.Update(entity);
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
                var entity = await _context.PetGroomers.FindAsync(id);
                if (entity == null) return Result.Failure(new[] { new Error("NotFound", "No existe el registro") });

                _context.PetGroomers.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new[] { new Error("Database.Error", ex.Message) });
            }
        }

        public async Task<Result> RemoveAsync(PetGroomer entity)
        {
            try
            {
                _context.PetGroomers.Remove(entity);
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
