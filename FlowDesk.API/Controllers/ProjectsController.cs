using FlowDesk.Application.DTO;
using FlowDesk.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var project = await _projectService.CreateProjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return Ok(project);
    }
}