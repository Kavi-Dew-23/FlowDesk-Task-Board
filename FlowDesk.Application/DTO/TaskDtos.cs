using FlowDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesk.Application.DTO
{
    // What comes IN when creating a task
    public record CreateTaskDto(
        string Title,
        string Description,
        TaskPriority Priority,
        DateTime DueDate,
        Guid? AssigneeId
    );

    // What comes IN when updating a task
    public record UpdateTaskDto(
        string Title,
        string Description,
        TaskPriority Priority,
        DateTime DueDate,
        Guid? AssigneeId
    );

    // What goes OUT in every response
    public record TaskResponseDto(
        Guid Id,
        string Title,
        string Description,
        string Status,
        string Priority,
        DateTime DueDate,
        bool IsArchived,
        Guid ProjectId,
        Guid? AssigneeId,
        string? AssigneeName,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
