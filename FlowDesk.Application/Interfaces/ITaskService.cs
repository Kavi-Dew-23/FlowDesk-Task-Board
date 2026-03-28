using FlowDesk.Application.DTO;
using FlowDesk.Domain.Enums;


namespace FlowDesk.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(Guid projectId, CreateTaskDto dto);
        Task<IEnumerable<TaskResponseDto>> GetProjectTasksAsync(
            Guid projectId, string? status, string? priority,
            Guid? assigneeId, string? sortBy, int page, int pageSize);
        Task<TaskResponseDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto dto);
        Task<TaskResponseDto> TransitionStatusAsync(Guid taskId, BoardTaskStatus newStatus);
        Task ArchiveTaskAsync(Guid taskId);
        Task<IEnumerable<TaskResponseDto>> GetArchivedTasksAsync(Guid projectId);
    }
}
