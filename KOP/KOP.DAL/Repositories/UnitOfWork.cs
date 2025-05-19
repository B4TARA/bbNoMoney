using KOP.DAL.Interfaces;
using KOP.DAL.Interfaces.AssessmentInterfaces;
using KOP.DAL.Interfaces.GradeInterfaces;
using KOP.DAL.Interfaces.RelationInterfaces;
using KOP.DAL.Repositories.AssessmentRepositories;
using KOP.DAL.Repositories.GradeRepositories;
using KOP.DAL.Repositories.RelationRepositories;

namespace KOP.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        private IAssessmentIntervalRepository? assessmentIntervalRepository;
        private IAssessmentMatrixElementRepository? assessmentMatrixElementRepository;
        private IAssessmentMatrixRepository? assessmentMatrixRepository;
        private IAssessmentIntervalMatrixRepository? assessmentIntervalMatrixRepository;
        private IAssessmentRepository? assessmentRepository;
        private IAssessmentResultRepository? assessmentResultRepository;
        private IAssessmentResultValueRepository? assessmentResultValueRepository;
        private IAssessmentTypeRepository? assessmentTypeRepository;

        private IGradeIntervalMatrixRepository? gradeIntervalMatrixRepository;
        private IGradeIntervalRepository? gradeIntervalRepository;
        private IGradeMatrixColumnRepository? gradeMatrixColumnRepository;
        private IGradeMatrixRepository? gradeMatrixRepository;
        private IGradeRepository? gradeRepository;
        private IGradeResultRepository? gradeResultRepository;
        private IGradeRouteRepository? gradeRouteRepository;
        private IGradeRouteGroupRepository? gradeRouteGroupRepository;
        private IGradeStatusRepository? gradeStatusRepository;
        private IGradeTypeRepository? gradeTypeRepository;

        private IEmployeeAttributeRepository? employeeAttributeRepository;
        private IEmployeeStateAttributeRepository? employeeStateAttributeRepository;
        private IEmployeeGradeRouteGroupRepository? employeeGradeRouteGroupRepository;

        private IAttributeRepository? attributeRepository;
        private ICommentRepository? commentRepository;
        private IEmployeeRepository? employeeRepository;
        private IEmployeeStateRepository? employeeStateRepository;
        private IMarkRepository? markRepository;
        private IMarkTypeRepository? markTypeRepository;
        private IModuleRepository? moduleRepository;
        private IModuleTypeRepository? moduleTypeRepository;
        private INotificationRepository? notificationRepository;
        private IRoleRepository? roleRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public IAssessmentIntervalRepository AssessmentIntervals
        {
            get
            {
                if (assessmentIntervalRepository == null)
                    assessmentIntervalRepository = new AssessmentIntervalRepository(_dbContext);
                return assessmentIntervalRepository;
            }
        }

        public IAssessmentMatrixElementRepository AssessmentMatrixElements
        {
            get
            {
                if (assessmentMatrixElementRepository == null)
                    assessmentMatrixElementRepository = new AssessmentMatrixElementRepository(_dbContext);
                return assessmentMatrixElementRepository;
            }
        }

        public IAssessmentMatrixRepository AssessmentMatrices
        {
            get
            {
                if (assessmentMatrixRepository == null)
                    assessmentMatrixRepository = new AssessmentMatrixRepository(_dbContext);
                return assessmentMatrixRepository;
            }
        }

        public IAssessmentIntervalMatrixRepository AssessmentIntervalMatrices
        {
            get
            {
                if (assessmentIntervalMatrixRepository == null)
                    assessmentIntervalMatrixRepository = new AssessmentIntervalMatrixRepository(_dbContext);
                return assessmentIntervalMatrixRepository;
            }
        }

        public IAssessmentRepository Assessments
        {
            get
            {
                if (assessmentRepository == null)
                    assessmentRepository = new AssessmentRepository(_dbContext);
                return assessmentRepository;
            }
        }

        public IAssessmentResultRepository AssessmentResults
        {
            get
            {
                if (assessmentResultRepository == null)
                    assessmentResultRepository = new AssessmentResultRepository(_dbContext);
                return assessmentResultRepository;
            }
        }

        public IAssessmentResultValueRepository AssessmentResultValues
        {
            get
            {
                if (assessmentResultValueRepository == null)
                    assessmentResultValueRepository = new AssessmentResultValueRepository(_dbContext);
                return assessmentResultValueRepository;
            }
        }

        public IAssessmentTypeRepository AssessmentTypes
        {
            get
            {
                if (assessmentTypeRepository == null)
                    assessmentTypeRepository = new AssessmentTypeRepository(_dbContext);
                return assessmentTypeRepository;
            }
        }



        public IGradeIntervalMatrixRepository GradeIntervalMatrices
        {
            get
            {
                if (gradeIntervalMatrixRepository == null)
                    gradeIntervalMatrixRepository = new GradeIntervalMatrixRepository(_dbContext);
                return gradeIntervalMatrixRepository;
            }
        }

        public IGradeIntervalRepository GradeIntervals
        {
            get
            {
                if (gradeIntervalRepository == null)
                    gradeIntervalRepository = new GradeIntervalRepository(_dbContext);
                return gradeIntervalRepository;
            }
        }

        public IGradeMatrixColumnRepository GradeMatrixColumns
        {
            get
            {
                if (gradeMatrixColumnRepository == null)
                    gradeMatrixColumnRepository = new GradeMatrixColumnRepository(_dbContext);
                return gradeMatrixColumnRepository;
            }
        }

        public IGradeMatrixRepository GradeMatrices
        {
            get
            {
                if (gradeMatrixRepository == null)
                    gradeMatrixRepository = new GradeMatrixRepository(_dbContext);
                return gradeMatrixRepository;
            }
        }

        public IGradeRepository Grades
        {
            get
            {
                if (gradeRepository == null)
                    gradeRepository = new GradeRepository(_dbContext);
                return gradeRepository;
            }
        }

        public IGradeResultRepository GradeResults
        {
            get
            {
                if (gradeResultRepository == null)
                    gradeResultRepository = new GradeResultRepository(_dbContext);
                return gradeResultRepository;
            }
        }

        public IGradeRouteRepository GradeRoutes
        {
            get
            {
                if (gradeRouteRepository == null)
                    gradeRouteRepository = new GradeRouteRepository(_dbContext);
                return gradeRouteRepository;
            }
        }

        public IGradeRouteGroupRepository GradeRouteGroups
        {
            get
            {
                if (gradeRouteGroupRepository == null)
                    gradeRouteGroupRepository = new GradeRouteGroupRepository(_dbContext);
                return gradeRouteGroupRepository;
            }
        }

        public IGradeStatusRepository GradeStatuses
        {
            get
            {
                if (gradeStatusRepository == null)
                    gradeStatusRepository = new GradeStatusRepository(_dbContext);
                return gradeStatusRepository;
            }
        }

        public IGradeTypeRepository GradeTypes
        {
            get
            {
                if (gradeTypeRepository == null)
                    gradeTypeRepository = new GradeTypeRepository(_dbContext);
                return gradeTypeRepository;
            }
        }



        public IEmployeeAttributeRepository EmployeeAttributes
        {
            get
            {
                if (employeeAttributeRepository == null)
                    employeeAttributeRepository = new EmployeeAttributeRepository(_dbContext);
                return employeeAttributeRepository;
            }
        }

        public IEmployeeStateAttributeRepository EmployeeStateAttributes
        {
            get
            {
                if (employeeStateAttributeRepository == null)
                    employeeStateAttributeRepository = new EmployeeStateAttributeRepository(_dbContext);
                return employeeStateAttributeRepository;
            }
        }

        public IEmployeeGradeRouteGroupRepository EmployeeGradeRouteGroups
        {
            get
            {
                if (employeeGradeRouteGroupRepository == null)
                    employeeGradeRouteGroupRepository = new EmployeeGradeRouteGroupRepository(_dbContext);
                return employeeGradeRouteGroupRepository;
            }
        }



        public ICommentRepository Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(_dbContext);
                return commentRepository;
            }
        }

        public IAttributeRepository Attributes
        {
            get
            {
                if (attributeRepository == null)
                    attributeRepository = new AttributeRepository(_dbContext);
                return attributeRepository;
            }
        }

        public IEmployeeRepository Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new EmployeeRepository(_dbContext);
                return employeeRepository;
            }
        }

        public IEmployeeStateRepository EmployeeStates
        {
            get
            {
                if (employeeStateRepository == null)
                    employeeStateRepository = new EmployeeStateRepository(_dbContext);
                return employeeStateRepository;
            }
        }

        public IMarkRepository Marks
        {
            get
            {
                if (markRepository == null)
                    markRepository = new MarkRepository(_dbContext);
                return markRepository;
            }
        }

        public IMarkTypeRepository MarkTypes
        {
            get
            {
                if (markTypeRepository == null)
                    markTypeRepository = new MarkTypeRepository(_dbContext);
                return markTypeRepository;
            }
        }

        public IModuleRepository Modules
        {
            get
            {
                if (moduleRepository == null)
                    moduleRepository = new ModuleRepository(_dbContext);
                return moduleRepository;
            }
        }

        public IModuleTypeRepository ModuleTypes
        {
            get
            {
                if (moduleTypeRepository == null)
                    moduleTypeRepository = new ModuleTypeRepository(_dbContext);
                return moduleTypeRepository;
            }
        }

        public INotificationRepository Notifications
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository(_dbContext);
                return notificationRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(_dbContext);
                return roleRepository;
            }
        }



        public void Commit()
             => _dbContext.SaveChanges();
        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();
        public void Rollback()
            => _dbContext.Dispose();


        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();


        private bool disposed = false;


        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }


                disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}