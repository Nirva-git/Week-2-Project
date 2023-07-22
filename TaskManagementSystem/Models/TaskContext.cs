using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace TaskManagementSystem.Models
{
    public class TaskContext: DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
       : base(options)
        {
        }
        public DbSet<TaskItem> TaskItems { get; set; } = null!;
        public DbSet<UserTable> UsersTable { get; set; }
        public DbSet<Coment> Comments { get; set; } 
    }
}
