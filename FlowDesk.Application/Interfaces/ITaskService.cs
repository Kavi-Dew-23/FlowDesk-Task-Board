using FlowDesk.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesk.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(Guid projectId, CreateTaskDto dto);
        Task<IEnumerable<TaskResponseDto>> GetProjectTasksAsync(Guid projectId, string? status, string? priority, Guid? assigneeId);
        Task<TaskResponseDto> UpdateTaskAsync(Guid taskId, UpdateTaskDto dto);
        Task<TaskResponseDto> TransitionStatusAsync(Guid taskId, TaskStatus newStatus);
        Task ArchiveTaskAsync(Guid taskId);
    }
}
