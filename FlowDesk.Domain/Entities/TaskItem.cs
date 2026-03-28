using FlowDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesk.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BoardTaskStatus Status { get; set; } = BoardTaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime DueDate { get; set; }
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        // Business rule: only certain transitions are allowed
        public void TransitionTo(BoardTaskStatus newStatus)
        {
            var allowed = new Dictionary<BoardTaskStatus, List<BoardTaskStatus>>
            {
                [BoardTaskStatus.ToDo] = new() { BoardTaskStatus.InProgress },
                [BoardTaskStatus.InProgress] = new() { BoardTaskStatus.Done, BoardTaskStatus.ToDo },
                [BoardTaskStatus.Done] = new() { BoardTaskStatus.Archived },
                [BoardTaskStatus.Archived] = new()
            };

            if (!allowed[Status].Contains(newStatus))
                throw new InvalidOperationException(
                    $"Cannot transition from '{Status}' to '{newStatus}'.");

            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
