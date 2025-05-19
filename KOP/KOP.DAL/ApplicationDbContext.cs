using KOP.DAL.Entities;
using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Entities.RelationEntities;
using Microsoft.EntityFrameworkCore;
using Attribute = KOP.DAL.Entities.Attribute;

namespace KOP.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentInterval> AssessmentIntervals { get; set; }
        public DbSet<AssessmentMatrixElement> AssessmentMatrixElements { get; set; }
        public DbSet<AssessmentMatrix> AssessmentMatrices { get; set; }
        public DbSet<AssessmentIntervalMatrix> AssessmentIntervalMatrices { get; set; }
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<AssessmentResultValue> AssessmentResultValues { get; set; }
        public DbSet<AssessmentType> AssessmentTypes { get; set; }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeInterval> GradeIntervals { get; set; }
        public DbSet<GradeIntervalMatrix> GradeIntervalMatrices { get; set; }
        public DbSet<GradeMatrixColumn> GradeMatrixColumns { get; set; }
        public DbSet<GradeMatrix> GradeMatrices { get; set; }
        public DbSet<GradeRoute> GradeRoutes { get; set; }
        public DbSet<GradeResult> GradeResults { get; set; }
        public DbSet<GradeRouteGroup> GradeRouteGroups { get; set; }
        public DbSet<GradeStatus> GradeStatuses { get; set; }
        public DbSet<GradeType> GradeTypes { get; set; }

        public DbSet<EmployeeAttribute> EmployeeAttributes { get; set; }
        public DbSet<EmployeeStateAttribute> EmployeeStateAttributes { get; set; }
        public DbSet<EmployeeGradeRouteGroup> EmployeeGradeRouteGroups { get; set; }

        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeState> EmployeeStates { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<MarkType> MarkTypes { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleType> ModuleTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeAttribute>()
                .HasKey(t => new { t.EmployeeId, t.AttributeId });

            modelBuilder.Entity<EmployeeStateAttribute>()
                .HasKey(t => new { t.EmployeeStateId, t.AttributeId });

            modelBuilder.Entity<EmployeeGradeRouteGroup>()
                .HasKey(t => new { t.EmployeeId, t.GradeRouteGroupId });
        }
    }
}