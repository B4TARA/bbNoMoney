using KOP.BLL.Interfaces;
using KOP.BLL.Services;
using KOP.DAL.Interfaces;
using KOP.DAL.Interfaces.AssessmentInterfaces;
using KOP.DAL.Interfaces.GradeInterfaces;
using KOP.DAL.Interfaces.RelationInterfaces;
using KOP.DAL.Repositories;
using KOP.DAL.Repositories.AssessmentRepositories;
using KOP.DAL.Repositories.GradeRepositories;
using KOP.DAL.Repositories.RelationRepositories;

namespace KOP.WEB
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<IAssessmentIntervalRepository, AssessmentIntervalRepository>();
            services.AddScoped<IAssessmentMatrixElementRepository, AssessmentMatrixElementRepository>();
            services.AddScoped<IAssessmentMatrixRepository, AssessmentMatrixRepository>();
            services.AddScoped<IAssessmentIntervalMatrixRepository, AssessmentIntervalMatrixRepository>();
            services.AddScoped<IAssessmentResultRepository, AssessmentResultRepository>();
            services.AddScoped<IAssessmentTypeRepository, AssessmentTypeRepository>();

            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IGradeResultRepository, GradeResultRepository>();
            services.AddScoped<IGradeIntervalRepository, GradeIntervalRepository>();
            services.AddScoped<IGradeIntervalMatrixRepository, GradeIntervalMatrixRepository>();
            services.AddScoped<IGradeMatrixColumnRepository, GradeMatrixColumnRepository>();
            services.AddScoped<IGradeMatrixRepository, GradeMatrixRepository>();
            services.AddScoped<IGradeRouteRepository, GradeRouteRepository>();
            services.AddScoped<IGradeRouteGroupRepository, GradeRouteGroupRepository>();
            services.AddScoped<IGradeStatusRepository, GradeStatusRepository>();
            services.AddScoped<IGradeTypeRepository, GradeTypeRepository>();

            services.AddScoped<IEmployeeAttributeRepository, EmployeeAttributeRepository>();
            services.AddScoped<IEmployeeStateAttributeRepository, EmployeeStateAttributeRepository>();
            services.AddScoped<IEmployeeGradeRouteGroupRepository, EmployeeGradeRouteGroupRepository>();

            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeStateRepository, EmployeeStateRepository>();
            services.AddScoped<IMarkRepository, MarkRepository>();
            services.AddScoped<IMarkTypeRepository, MarkTypeRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleTypeRepository, ModuleTypeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IMarkService, MarkService>();
            services.AddScoped<IMappingService, MappingService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISupervisorService, SupervisorService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
        }
    }
}