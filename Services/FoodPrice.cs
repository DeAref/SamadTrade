

using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using samadApp.Models;
namespace samadApp.services{
    public class FoodPriceClass : BackgroundService
    {
        private readonly ILogger<FoodPriceClass> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly RequestDelegate _next;
        public FoodPriceClass(ILogger<FoodPriceClass> logger,IServiceScopeFactory serviceScopeFactory)
        {
           // _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("saving data");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MvcFprojectContext>();

                    // الگوی ذخیره داده‌ها در دیتابیس
                    var newData = new Price
                    {
                       FoodPrice = 10000 ,
                       TimeStamp = DateTime.Now
                    };

                    dbContext.Price.Add(newData);
                    

                    if (dbContext.Price.Count() > 3){
                            var FirstPriceRow =dbContext.Price.OrderBy(p => p.Id).First();
                            dbContext.Price.Remove(FirstPriceRow);
                    }
                    await dbContext.SaveChangesAsync();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}

      