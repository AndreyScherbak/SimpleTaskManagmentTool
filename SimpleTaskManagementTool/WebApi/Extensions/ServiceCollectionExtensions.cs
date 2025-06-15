using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Services;
using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Boards.ViewBoardDetails;
using Application.UseCases.Boards.ViewBoardsList;
using Application.UseCases.Tasks.CreateTask;
using Application.UseCases.Tasks.EditTask;
using Application.UseCases.Tasks.MoveTask;
using Application.UseCases.Tasks.DeleteTask;
using Application.UseCases.Tasks.CreateTask.Activities;
using Application.UseCases.Tasks.EditTask.Activities;
using Application.UseCases.Tasks.MoveTask.Activities;
using Application.UseCases.Boards.CreateBoard.Activities;
using Application.UseCases.Tasks.DeleteTask.Activities;
using Application.UseCases.Tasks.Common;
using Application.Abstractions.Services;
using Application.Validators;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Services;

namespace WebApi.Extensions;

/// <summary>
/// Registers application and infrastructure services for the Web API.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure
        services.AddInfrastructure("SimpleTaskDb");

        // Core application plumbing
        services.AddScoped<UseCaseFactory>();
        services.AddScoped(typeof(IActivityFactory<>), typeof(ActivityFactory<>));

        // Http context helpers
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextService, UserContextService>();

        // Validators
        services.AddTransient<BoardTitleValidator>();
        services.AddTransient<TaskTitleValidator>();
        services.AddTransient<DueDateValidator>();
        services.AddTransient<CreateBoardValidator>();
        services.AddTransient<CreateTaskValidator>();
        services.AddTransient<EditTaskValidator>();
        services.AddTransient<MoveTaskValidator>();

        // Use case handlers
        services.AddTransient<CreateBoardHandler>();
        services.AddTransient<ViewBoardsListHandler>();
        services.AddTransient<ViewBoardDetailsHandler>();
        services.AddTransient<CreateTaskHandler>();
        services.AddTransient<EditTaskHandler>();
        services.AddTransient<MoveTaskHandler>();
        services.AddTransient<DeleteTaskHandler>();

        // Activities - registration order defines pipeline order
        services.AddTransient<IActivity<ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>, ValidateTitleActivity>();
        services.AddTransient<IActivity<ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>, SaveBoardActivity>();

        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, ValidateTaskDetailsActivity>();
        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, AttachTaskToBoardActivity>();
        services.AddTransient<IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>, SaveTaskActivity>();

        services.AddTransient<IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>, ValidateEditTaskActivity>();
        services.AddTransient<IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>, Application.UseCases.Tasks.EditTask.Activities.LoadTaskActivity>();
        services.AddTransient<IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>, ApplyTaskChangesActivity>();

        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, Application.UseCases.Tasks.MoveTask.Activities.LoadTaskActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, ValidateStatusTransitionActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, UpdateTaskStatusActivity>();
        services.AddTransient<IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>, PersistStatusChangeActivity>();

        // Activities for DeleteTaskHandler
        services.AddTransient<LoadTaskForDeleteActivity>();
        services.AddTransient<PerformDeleteTaskActivity>();

        return services;
    }
}
