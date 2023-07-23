using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RecipientUserId { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
