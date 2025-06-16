using Application.Abstractions.Models;
using Application.Services;
using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Boards.ViewBoardDetails;
using Application.UseCases.Boards.ViewBoardsList;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly UseCaseFactory _factory;

    public BoardsController(UseCaseFactory factory) => _factory = factory;

    [HttpGet]
    public async Task<IActionResult> GetBoards()
    {
        var result = await _factory.ExecuteAsync<object?, Result<ViewBoardsListResponse>, ViewBoardsListHandler>(null);
        return result.Success ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBoard(Guid id)
    {
        var result = await _factory.ExecuteAsync<Guid, Result<ViewBoardDetailsResponse>, ViewBoardDetailsHandler>(id);
        if (result.Success) return Ok(result.Value);
        return NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardDto dto)
    {
        var request = new CreateBoardRequest(dto.Title);
        var result = await _factory.ExecuteAsync<CreateBoardRequest, Result<CreateBoardResponse>, CreateBoardHandler>(request);
        return result.Success ? Ok(result.Value) : BadRequest(result.Error);
    }
}
