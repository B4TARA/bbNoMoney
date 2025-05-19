namespace KOP.Common.DTOs
{
    public class MarkDTO
    {
        public int Id { get; set; }
        public int PlanValue { get; set; }
        public int FactValue { get; set; }
        public int PercentageValue { get; set; }
        public bool Result { get; set; }
        public bool IsPercentage { get; set; }

        public string TypeName { get; set; }       
        public string Period { get; set; }
    }
}