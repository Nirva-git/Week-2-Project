using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskContext _context;

        public TaskItemsController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
          if (_context.TaskItems == null)
          {
              return NotFound();
          }
            return await _context.TaskItems.ToListAsync();
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
          if (_context.TaskItems == null)
          {
              return NotFound();
          }
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
          if (_context.TaskItems == null)
          {
              return Problem("Entity set 'TaskContext.TaskItems'  is null.");
          }
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, taskItem);
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            if (_context.TaskItems == null)
            {
                return NotFound();
            }
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(int id)
        {
            return (_context.TaskItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: api/tasks/1/assign/2
        [HttpPost("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignTask(int taskId, int userId)
        {
            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            // In real-world scenarios, you would typically validate if the user is allowed to assign tasks.
            // For simplicity, we'll assume any user can assign any task.

            task.AssignedUserId = userId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/tasks/1/unassign
        [HttpPost("{taskId}/unassign")]
        public async Task<IActionResult> UnassignTask(int taskId)
        {
            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task.AssignedUserId = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
       
        // GET: api/tasks/search?status=InProgress&priority=High&sortBy=DueDate&sortOrder=Asc
        [HttpGet("search")]
        public IActionResult SearchTasks(
            [FromQuery] TaskStatusEnum? status,
            [FromQuery] TaskPriority? priority,
            [FromQuery] string sortBy,
            [FromQuery] string sortOrder)
        {
            var query = _context.TaskItems.AsQueryable();

            if (status.HasValue)
            {
                
                query = query.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("DueDate", StringComparison.OrdinalIgnoreCase))
                {
                    query = sortOrder.Equals("Asc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(t => t.DueDate)
                        : query.OrderByDescending(t => t.DueDate);
                }
                // Add more sorting options for other attributes as needed.
            }

            var tasks = query.ToList();
            return Ok(tasks);
        }

    }
}
