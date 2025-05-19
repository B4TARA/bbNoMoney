using KOP.DAL.Entities.RelationEntities;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeRouteGroup
    {
        [Key]
        public int Id { get; set; } // id группы оценки

        [Required]
        public int QueueNumber { get; set; } // Порядковый номер группы в цепочке оценки

        [Required]
        public string Name { get; set; } // название группы оценки
               
        public int RequiredAcceptancesNumber { get; set; } // Необходимое количество одобрений



        public GradeStatus GradeStatus { get; set; } // Статус, который определяет данную группу оценки
        public int GradeStatusId { get; set; }



        public List<EmployeeGradeRouteGroup> EmployeeGradeRouteGroups { get; set; } = new(); // Какие сотрудники участвуют в этой группе



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
