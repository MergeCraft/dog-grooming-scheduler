using System;
using System.Linq.Expressions;
using AplicationLogic.Interfaces;
using Hangfire;

namespace AplicationLogic.Services.Scheduler
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _client;

        public BackgroundJobService(IBackgroundJobClient client)
        {
            _client = client;
        }

        public void Enqueue(Expression<Action> job) => _client.Enqueue(job);

        public string Schedule(Expression<Action> job, DateTime scheduleAt)
        {
            var delay = scheduleAt - DateTime.Now;
            if (delay <= TimeSpan.Zero)
            {
                return _client.Enqueue(job);
            }

            return _client.Schedule(job, delay);
        }

        public void Delete(string jobId) => BackgroundJob.Delete(jobId);
    }
}
