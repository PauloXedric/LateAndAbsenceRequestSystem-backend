using Autofac;
using DLARS.Repositories;
using DLARS.Services;


namespace DLARS.DependencyInjection
{
    public class ServiceModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<RequestService>().As<IRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAccountService>().As<IUserAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherService>().As<ITeacherService>().InstancePerLifetimeScope();
            builder.RegisterType<SubjectService>().As<ISubjectService>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherSubjectsService>().As<ITeacherSubjectsService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<FileStorageService>().As<IFileStorageService>().InstancePerLifetimeScope();
        }


    }
}
