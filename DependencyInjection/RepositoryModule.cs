using Autofac;
using DLARS.Repositories;
using DLARS.Services;

namespace DLARS.DependencyInjection
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestRepository>().As<IRequestRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserAccountRepository>().As<IUserAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherRepository>().As<ITeacherRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubjectRepository>().As<ISubjectRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherSubjectsRepository>().As<ITeacherSubjectsRepository>().InstancePerLifetimeScope();

        }
    }
}
