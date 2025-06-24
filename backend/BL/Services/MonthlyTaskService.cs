using BL.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class MonthlyTaskService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public MonthlyTaskService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleNextRun();
            return Task.CompletedTask;
        }

        private void ScheduleNextRun()
        {
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            var timeToGo = nextRunTime - now;

            _timer = new Timer(async _ =>
            {
                try
                {
                    await DoWorkAsync();
                    ScheduleNextRun(); // מחשב את ההפעלה הבאה בדיוק לפי 1 לחודש
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in scheduled task: {ex.Message}");
                }
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private async Task DoWorkAsync()
        {
            Console.WriteLine("Monthly task started: " + DateTime.Now);

            var today = DateTime.Today;
            var nextMonth = today.AddMonths(1);
            var firstDay = new DateOnly(nextMonth.Year, nextMonth.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            using (var scope = _serviceProvider.CreateScope())
            {
                var generator = scope.ServiceProvider.GetRequiredService<ILessonServiceBL>();
                await generator.GenerateLessonsAsync(firstDay, lastDay);
            }

            Console.WriteLine("Lesson generation complete for month: " + nextMonth.ToString("MMMM yyyy"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
