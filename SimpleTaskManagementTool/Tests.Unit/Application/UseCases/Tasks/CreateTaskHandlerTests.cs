using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Services;
using Application.UseCases.Tasks.Common;
using Application.UseCases.Tasks.CreateTask;
using Application.UseCases.Tasks.CreateTask.Activities;
using Application.Validators;
using Domain.Interfaces;
using Application.Abstractions.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.UseCases.Tasks;

public class CreateTaskHandlerTests
{
    [Fact]
    public async Task Creates_Task_For_Board()
    {
        var services = new ServiceCollection();
        var repo = new FakeBoardRepository();
        var board = new global::Domain.Entities.Board(Guid.NewGuid(), "b");
        repo.Add(board);
        var clock = new FakeDateTimeProvider { UtcNow = DateTime.UtcNow };

        services.AddSingleton<IBoardRepository>(repo);
        services.AddSingleton<IDateTimeProvider>(clock);
        services.AddTransient<TaskTitleValidator>();
        services.AddTransient<DueDateValidator>();
        services.AddTransient<CreateTaskValidator>();
        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<global::Application.Abstractions.DTOs.TaskDto>>>, ValidateTaskDetailsActivity>();
        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, AttachTaskToBoardActivity>();
        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, SaveTaskActivity>();
        services.AddScoped<IActivityFactory<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, ActivityFactory<ActivityContext<CreateTaskRequest, Result<TaskDto>>>>();
        services.AddTransient<CreateTaskHandler>();
        services.AddScoped<UseCaseFactory>();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<UseCaseFactory>();

        var request = new CreateTaskRequest(board.Id, "Task1", clock.UtcNow.AddDays(1));
        var result = await factory.ExecuteAsync<CreateTaskRequest, Result<TaskDto>, CreateTaskHandler>(request);

        result.Success.Should().BeTrue();
        repo.Boards.Single().Tasks.Should().ContainSingle();
    }
}
