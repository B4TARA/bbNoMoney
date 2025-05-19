namespace KOP.Common.DTOs
{
    public class CommentDTO
    {
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorPosition { get; set; }
        public string Text { get; set; }
        public bool IsFeedback { get; set; }
        public DateOnly DateOfCreation { get; set; }
    }
}