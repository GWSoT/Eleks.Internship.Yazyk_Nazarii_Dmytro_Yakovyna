using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WTracking.Data;
using FluentScheduler;
using System.Linq;
using System;
using WTracking.Models;
using WebPush;

namespace WTracking.Extensions
{
    public static class ScheduleResetExtensions
    {
        public static IWebHost ScheduleReset(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;


                JobManager.AddJob(async () =>
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    await dbContext.Profile.ForEachAsync(x =>
                    {
                        x.FirstDayProgress = x.SecondDayProgress;
                        x.SecondDayProgress = x.ThirdDayProgress;
                        x.ThirdDayProgress = x.FourthDayProgress;
                        x.FourthDayProgress = x.FifthDayProgress;
                        x.FifthDayProgress = x.SixthDayProgress;
                        x.SixthDayProgress = x.SeventhDayProgress;
                        x.SeventhDayProgress = x.TodayStepCount;
                        x.TodayStepCount = 0;
                        x.AverageStepCountForToday = (x.FirstDayProgress
                                                    + x.SecondDayProgress 
                                                    + x.ThirdDayProgress 
                                                    + x.FourthDayProgress
                                                    + x.FifthDayProgress 
                                                    + x.SixthDayProgress 
                                                    + x.SeventhDayProgress) / 7;
                    });
                    await dbContext.SaveChangesAsync();
                },
                (s) => s.ToRunEvery(24).Hours());  
            }
            return webHost;
        }
    }
}
