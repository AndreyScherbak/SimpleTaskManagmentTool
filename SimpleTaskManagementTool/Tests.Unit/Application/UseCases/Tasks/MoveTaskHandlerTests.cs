using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Services;
using Application.UseCases.Tasks.Common;
using Application.UseCases.Tasks.MoveTask;
using Application.UseCases.Tasks.MoveTask.Activities;
using Application.Validators;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.UseCases.Tasks;

public class MoveTaskHandlerTests
{
    [Fact]
    public async Task Moves_Task_Status()
    {
        var services = new ServiceCollection();
        var repo = new FakeBoardRepository();
        var board = new global::Domain.Entities.Board(Guid.NewGuid(), "b");
        var task = new global::Domain.Entities.TaskEntity("t", DateTime.UtcNow, null);
        board.AddTask(task);
        repo.Add(board);

        services.AddSingleton<IBoardRepository>(repo);
        services.AddTransient<MoveTaskValidator>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, LoadTaskActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, ValidateStatusTransitionActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, UpdateTaskStatusActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, PersistStatusChangeActivity>();
        services.AddScoped<IActivityFactory<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, ActivityFactory<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>>();
        services.AddTransient<MoveTaskHandler>();
        services.AddScoped<UseCaseFactory>();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<UseCaseFactory>();

        var request = new MoveTaskRequest(board.Id, task.Id, "InProgress");
        var result = await factory.ExecuteAsync<MoveTaskRequest, Result<MoveTaskResponse>, MoveTaskHandler>(request);

        result.Success.Should().BeTrue();
        task.Status.ToString().Should().Be("InProgress");
    }
}
