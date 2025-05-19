using KOP.DAL.Interfaces.AssessmentInterfaces;
using KOP.DAL.Interfaces.GradeInterfaces;
using KOP.DAL.Interfaces.RelationInterfaces;

namespace KOP.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAssessmentIntervalRepository AssessmentIntervals { get; }
        IAssessmentMatrixElementRepository AssessmentMatrixElements { get; }
        IAssessmentMatrixRepository AssessmentMatrices { get; }
        IAssessmentIntervalMatrixRepository AssessmentIntervalMatrices { get; }
        IAssessmentRepository Assessments { get; }
        IAssessmentResultRepository AssessmentResults { get; }
        IAssessmentResultValueRepository AssessmentResultValues { get; }
        IAssessmentTypeRepository AssessmentTypes { get; }

        IGradeIntervalMatrixRepository GradeIntervalMatrices { get; }
        IGradeIntervalRepository GradeIntervals { get; }
        IGradeMatrixColumnRepository GradeMatrixColumns { get; }
        IGradeMatrixRepository GradeMatrices { get; }
        IGradeRepository Grades { get; }
        IGradeRouteRepository GradeRoutes { get; }
        IGradeResultRepository GradeResults { get; }
        IGradeRouteGroupRepository GradeRouteGroups { get; }
        IGradeStatusRepository GradeStatuses { get; }
        IGradeTypeRepository GradeTypes { get; }

        IEmployeeAttributeRepository EmployeeAttributes { get; }
        IEmployeeStateAttributeRepository EmployeeStateAttributes { get; }
        IEmployeeGradeRouteGroupRepository EmployeeGradeRouteGroups{ get; }

        IAttributeRepository Attributes { get; }
        ICommentRepository Comments { get; }
        IEmployeeRepository Employees { get; }
        IEmployeeStateRepository EmployeeStates { get; }
        IMarkRepository Marks { get; }
        IMarkTypeRepository MarkTypes { get; }
        IModuleTypeRepository ModuleTypes { get; }
        IModuleRepository Modules { get; }
        INotificationRepository Notifications { get; }
        IRoleRepository Roles { get; }

        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}