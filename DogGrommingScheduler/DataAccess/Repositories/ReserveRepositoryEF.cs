using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ReserveRepositoryEF : IReserveRepository
    {
        private readonly ContextDB _db;

        public ReserveRepositoryEF(ContextDB db)
        {
            _db = db;
        }

        public async Task<Result> AddAsync(Reserve entity)
        {
            try
            {
                await _db.Reserves.AddAsync(entity);
                await _db.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(Error.Unexpected);
            }
        }

        public async Task<Result<IEnumerable<Reserve>>> GetAllAsync()
        {
            try
            {
                var reserves = await _db.Reserves.ToListAsync();
                return Result<IEnumerable<Reserve>>.Success(reserves);
            }
            catch (Exception)
            {
                return Result<IEnumerable<Reserve>>.Failure(Error.Unexpected);
            }
        }

        public async Task<Result<Reserve>> GetByIdAsync(Guid id)
        {
            try
            {
                var reserve = await _db.Reserves.FindAsync(id);

                if (reserve == null)
                    return Result<Reserve>.Failure(Error.NotFound);

                return Result<Reserve>.Success(reserve);
            }
            catch (Exception)
            {
                return Result<Reserve>.Failure(Error.Unexpected);
            }
        }

        public async Task<Result> RemoveAsync(Guid id)
        {
            try
            {
                var reserve = await _db.Reserves.FindAsync(id);
                if (reserve == null) return Result.Failure(Error.NotFound);

                _db.Reserves.Remove(reserve);
                await _db.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(Error.Unexpected);
            }
        }

        public async Task<Result> RemoveAsync(Reserve entity)
        {
            try
            {
                _db.Reserves.Remove(entity);
                await _db.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(Error.Unexpected);
            }
        }

        public async Task<Result> UpdateAsync(Reserve entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.Failure(Error.Conflict);
            }
            catch (Exception)
            {
                return Result.Failure(Error.Unexpected);
            }
        }
    }
}
