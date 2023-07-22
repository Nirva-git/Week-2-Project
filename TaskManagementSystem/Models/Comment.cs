using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public required string Text { get; set; }
        public int TaskId { get; set; }
        public TaskItem TaskItem { get; set; }
    }
}
