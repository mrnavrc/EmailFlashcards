using Quartz.Impl;
using Quartz;

namespace EmailFlashcards.Jobs
{
    public class SendEmailJobTrigger
    {
        public static void Start()
        {
            IScheduler scheduler = (IScheduler)StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SendEmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);

        }
    }
}
