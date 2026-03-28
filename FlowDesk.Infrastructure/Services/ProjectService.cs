using FlowDesk.Application.DTO;
using FlowDesk.Application.Interfaces;
using FlowDesk.Domain.Entities;
using FlowDesk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDesk.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _db;
        public ProjectService(AppDbContext db) => _db = db;

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return MapToDto(project);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _db.Projects.ToListAsync();
            return projects.Select(MapToDto);
        }

        public async Task<ProjectResponseDto> GetProjectByIdAsync(Guid id)
        {
            var project = await _db.Projects.FindAsync(id)
                ?? throw new KeyNotFoundException("Project not found.");
            return MapToDto(project);
        }

        private static ProjectResponseDto MapToDto(Project p) =>
            new(p.Id, p.Name, p.Description, p.CreatedAt);
    }

}
