using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using BL.Api;

public class PastLessonUpdater : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly TimeSpan TargetTime = new TimeSpan(0, 0, 0); // 21:15

    public PastLessonUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // חישוב כמה זמן לחכות עד 21:15 הקרוב
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(now.TimeOfDay > TargetTime ? 1 : 0).Add(TargetTime);
            var delay = nextRun - now;

            Console.WriteLine($"[Scheduler] Waiting {delay.TotalMinutes:F0} minutes until next run at {nextRun}.");

            try
            {
                await Task.Delay(delay, stoppingToken); // ממתין עד הזמן הרצוי
            }
            catch (TaskCanceledException)
            {
                break; // עצירה תקינה אם השירות נסגר
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var lessonService = scope.ServiceProvider.GetRequiredService<ILessonServiceBL>();

                try
                {
                    Console.WriteLine($"[{DateTime.Now}] Running UpdatePastLessonsAsync...");

                    await lessonService.UpdatePastLessonsAsync();

                    Console.WriteLine($"[{DateTime.Now}] Finished updating past lessons.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to update past lessons: {ex.Message}");
                }
            }
        }
    }
}
