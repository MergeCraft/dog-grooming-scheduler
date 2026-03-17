using System;
using System.Linq.Expressions;

namespace AplicationLogic.Interfaces
{
    public interface IBackgroundJobService
    {
        void Enqueue(Expression<Action> job);
        string Schedule(Expression<Action> job, DateTime scheduleAt);
        void Delete(string jobId);
    }
}
