using FlowDesk.Application.DTO;
using FlowDesk.Application.Interfaces;
using FlowDesk.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid projectId, CreateTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(projectId, dto);
        return CreatedAtAction(nameof(GetAll), new { projectId }, task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid projectId,
        [FromQuery] string? status,
        [FromQuery] string? priority,
        [FromQuery] Guid? assigneeId,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var tasks = await _taskService.GetProjectTasksAsync(
            projectId, status, priority, assigneeId, sortBy, page, pageSize);
        return Ok(tasks);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> Update(Guid projectId, Guid taskId, UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(taskId, dto);
        return Ok(task);
    }

    [HttpPatch("{taskId}/status")]
    public async Task<IActionResult> TransitionStatus(
        Guid projectId, Guid taskId, [FromBody] StatusTransitionDto dto)
    {
        if (!Enum.TryParse<BoardTaskStatus>(dto.NewStatus, out var status))
            return BadRequest(new { message = "Invalid status. Use: ToDo, InProgress, Done, Archived" });

        var task = await _taskService.TransitionStatusAsync(taskId, status);
        return Ok(task);
    }

    [HttpPatch("{taskId}/archive")]
    public async Task<IActionResult> Archive(Guid projectId, Guid taskId)
    {
        await _taskService.ArchiveTaskAsync(taskId);
        return Ok(new { message = "Task archived successfully." });
    }

    [HttpGet("archived")]
    public async Task<IActionResult> GetArchived(Guid projectId)
    {
        var tasks = await _taskService.GetArchivedTasksAsync(projectId);
        return Ok(tasks);
    }
}