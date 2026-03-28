using FlowDesk.Application.DTO;
using FlowDesk.Application.Interfaces;
using FlowDesk.Domain.Entities;
using FlowDesk.Domain.Enums;
using FlowDesk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db) => _db = db;

    public async Task<TaskResponseDto> CreateTaskAsync(Guid projectId, CreateTaskDto dto)
    {
        if (dto.DueDate < DateTime.UtcNow)
            throw new InvalidOperationException("Due date cannot be in the past.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new InvalidOperationException("Title cannot be empty.");

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            AssigneeId = dto.AssigneeId,
            ProjectId = projectId
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return await ToDtoAsync(task.Id);
    }

    // ✅ Updated signature with sorting and pagination
    public async Task<IEnumerable<TaskResponseDto>> GetProjectTasksAsync(
        Guid projectId, string? status, string? priority,
        Guid? assigneeId, string? sortBy, int page, int pageSize)
    {
        var query = _db.Tasks
            .Include(t => t.Assignee)
            .Where(t => t.ProjectId == projectId && !t.IsArchived);

        // Filtering
        if (!string.IsNullOrEmpty(status) &&
            Enum.TryParse<BoardTaskStatus>(status, out var s))
            query = query.Where(t => t.Status == s);

        if (!string.IsNullOrEmpty(priority) &&
            Enum.TryParse<TaskPriority>(priority, out var p))
            query = query.Where(t => t.Priority == p);

        if (assigneeId.HasValue)
            query = query.Where(t => t.AssigneeId == assigneeId);

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "priority" => query.OrderByDescending(t => t.Priority),
            "duedate" => query.OrderBy(t => t.DueDate),
            "status" => query.OrderBy(t => t.Status),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        // Pagination
        var tasks = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return tasks.Select(MapToDto);
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FindAsync(taskId)
            ?? throw new KeyNotFoundException("Task not found.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new InvalidOperationException("Title cannot be empty.");

        if (dto.DueDate < DateTime.UtcNow)
            throw new InvalidOperationException("Due date cannot be in the past.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.AssigneeId = dto.AssigneeId;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return await ToDtoAsync(task.Id);
    }

    public async Task<TaskResponseDto> TransitionStatusAsync(
        Guid taskId, BoardTaskStatus newStatus)
    {
        var task = await _db.Tasks.FindAsync(taskId)
            ?? throw new KeyNotFoundException("Task not found.");

        task.TransitionTo(newStatus);
        await _db.SaveChangesAsync();
        return await ToDtoAsync(task.Id);
    }

    public async Task ArchiveTaskAsync(Guid taskId)
    {
        var task = await _db.Tasks.FindAsync(taskId)
            ?? throw new KeyNotFoundException("Task not found.");

        task.IsArchived = true;
        task.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskResponseDto>> GetArchivedTasksAsync(Guid projectId)
    {
        var tasks = await _db.Tasks
            .Include(t => t.Assignee)
            .Where(t => t.ProjectId == projectId && t.IsArchived)
            .ToListAsync();

        return tasks.Select(MapToDto);
    }

    private async Task<TaskResponseDto> ToDtoAsync(Guid taskId)
    {
        var task = await _db.Tasks
            .Include(t => t.Assignee)
            .FirstAsync(t => t.Id == taskId);
        return MapToDto(task);
    }

    private static TaskResponseDto MapToDto(TaskItem t) => new(
        t.Id, t.Title, t.Description,
        t.Status.ToString(), t.Priority.ToString(),
        t.DueDate, t.IsArchived, t.ProjectId,
        t.AssigneeId, t.Assignee?.Name,
        t.CreatedAt, t.UpdatedAt
    );
}