namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int IsCompleted { get; set; }
        public string DueDate { get; set; }
        public string Priority { get; set; }
        public string UserId { get; set; }
    }
}
