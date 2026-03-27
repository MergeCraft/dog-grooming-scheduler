using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ScheduleRepositoryEF : IScheduleRepository
    {
        private readonly ContextDB _context;

        public ScheduleRepositoryEF(ContextDB context)
        {
            _context = context;
        }

        public async Task<Result<Schedule?>> GetByGroomerAndDateAsync(Guid groomerId, DateTime date)
        {
            try
            {
                var schedule = await _context.Schedules
                    .Include(s => s.Reservations)
                    .FirstOrDefaultAsync(s => s.PetGroomerId == groomerId && s.Date.Date == date.Date);

                return Result<Schedule?>.Success(schedule);
            }
            catch (Exception ex)
            {
                return Result<Schedule?>.Failure(new[] { new Error("Error.Database", ex.Message) });
            }
        }

        public async Task<Result> AddAsync(Schedule entity)
        {
            await _context.Schedules.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<Schedule>> GetByIdAsync(Guid id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Reservations)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
                return Result<Schedule>.Failure(new[] { new Error("Error.NotFound", "Schedule not found") });

            return Result<Schedule>.Success(schedule);
        }

        public async Task<Result> UpdateAsync(Schedule entity)
        {
            _context.Schedules.Update(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }


        public async Task<Result<IEnumerable<Schedule>>> GetAllAsync()
        {
            var list = await _context.Schedules.ToListAsync();
            return Result<IEnumerable<Schedule>>.Success(list);
        }

        public async Task<Result> RemoveAsync(Guid id)
        {
            var entity = await _context.Schedules.FindAsync(id);
            if (entity == null) return Result.Failure(new[] { new Error("Error.NotFound", "Not found") });

            _context.Schedules.Remove(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> RemoveAsync(Schedule entity)
        {
            _context.Schedules.Remove(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
