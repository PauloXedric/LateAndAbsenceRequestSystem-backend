using Autofac;
using DLARS.HangfireJobs;

namespace DLARS.DependencyInjection
{
    public class HangfireJobModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationJob>().As<INotificationJob>().InstancePerLifetimeScope();
        }

    }
}
