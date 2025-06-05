using Application.Abstractions.Models;
using Application.Services;
using Application.UseCases.Tasks.Common;
using Application.UseCases.Tasks.CreateTask;
using Application.UseCases.Tasks.DeleteTask;
using Application.UseCases.Tasks.EditTask;
using Application.UseCases.Tasks.MoveTask;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/boards/{boardId:guid}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly UseCaseFactory _factory;

    public TasksController(UseCaseFactory factory) => _factory = factory;

    [HttpPost]
    public async Task<IActionResult> Create(Guid boardId, [FromBody] CreateTaskDto dto)
    {
        var request = new CreateTaskRequest(boardId, dto.Title, dto.DueDate);
        var result = await _factory.ExecuteAsync<CreateTaskRequest, Result<TaskDto>, CreateTaskHandler>(request);
        return result.Success ? Ok(ToDto(result.Value!)) : BadRequest(result.Error);
    }

    [HttpPut("{taskId:guid}")]
    public async Task<IActionResult> Edit(Guid boardId, Guid taskId, [FromBody] CreateTaskDto dto)
    {
        var request = new EditTaskRequest(boardId, taskId, dto.Title, dto.DueDate);
        var result = await _factory.ExecuteAsync<EditTaskRequest, Result<EditTaskResponse>, EditTaskHandler>(request);
        return result.Success ? Ok(ToDto(result.Value!.Task)) : BadRequest(result.Error);
    }

    [HttpPost("{taskId:guid}/move")]
    public async Task<IActionResult> Move(Guid boardId, Guid taskId, [FromQuery] string targetStatus)
    {
        var request = new MoveTaskRequest(boardId, taskId, targetStatus);
        var result = await _factory.ExecuteAsync<MoveTaskRequest, Result<MoveTaskResponse>, MoveTaskHandler>(request);
        return result.Success ? Ok(ToDto(result.Value!.Task)) : BadRequest(result.Error);
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> Delete(Guid boardId, Guid taskId)
    {
        var request = new DeleteTaskRequest(boardId, taskId);
        var result = await _factory.ExecuteAsync<DeleteTaskRequest, Result<DeleteTaskResponse>, DeleteTaskHandler>(request);
        return result.Success ? Ok(ToDto(result.Value!.DeletedTask)) : BadRequest(result.Error);
    }

    private static TaskResponseDto ToDto(TaskDto dto) => new(dto.Id, dto.BoardId, dto.Title, dto.CreatedAt, dto.DueDate, dto.Status);
}
