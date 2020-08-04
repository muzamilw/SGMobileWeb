using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using Quartz.Impl;
using SG2.CORE.BAL.Managers;

namespace SG2.CORE.WEB.App_Start
{
   
    public class Jobclass : IJob
    {
        protected readonly CustomerManager _customerManager;
        public Jobclass()
        {
            _customerManager = new CustomerManager();
        }
        public async Task Execute(IJobExecutionContext context)
        {

            var data = _customerManager.GetRunNotificationsData();
            foreach (var item in data)
            {
                var dynamicTemplateData = new Dictionary<string, string>
                            {
                                {"name",item.FirstName},
                                {"senddate", DateTime.Today.ToLongDateString() },
                                {"starttime",item.startTime }
                            };
                await BAL.Managers.EmailManager.SendEmail(item.EmailAddress, item.FirstName, EmailManager.EmailType.RunNotification, dynamicTemplateData);
            }
           
        }
    }

    public class JobScheduler
    {
        public async Task Start()
        {
            IScheduler scheduler = await (new StdSchedulerFactory()).GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<Jobclass>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(300)
            .RepeatForever())
            .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}