using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskItem
    { 

        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
