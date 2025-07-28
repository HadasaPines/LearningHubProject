using BL.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class StudentBirthdayUpdaterBL : BackgroundService
    {
       
            private readonly IServiceProvider _serviceProvider;
            private static readonly TimeSpan TargetTime = new TimeSpan(0, 0, 0); 

            public StudentBirthdayUpdaterBL(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var now = DateTime.Now;
                    var nextRun = now.Date.AddDays(now.TimeOfDay > TargetTime ? 1 : 0).Add(TargetTime);
                    var delay = nextRun - now;

                    try
                    {
                        Console.WriteLine($"[BirthdayUpdater] Waiting {delay.TotalMinutes:F0} minutes until next run at {nextRun}.");
                        await Task.Delay(delay, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var studentService = scope.ServiceProvider.GetRequiredService<IStudentServiceBL>();

                        try
                        {
                            Console.WriteLine($"[{DateTime.Now}] Running UpdateStudentAgesAsync...");
                            await studentService.UpdateStudentAges();
                            Console.WriteLine($"[{DateTime.Now}] Finished updating student ages.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] Failed to update student ages: {ex.Message}");
                        }
                    }
                }
            }
        }

    }

