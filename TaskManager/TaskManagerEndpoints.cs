using static TaskManager.TaskManagerLogic;

namespace TaskManager;

public static class TaskManagerEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/tasks").WithTags("Tasks");

        g.MapGet("/task/{id:Guid}", GetTaskById);

        g.MapGet("/tasks", GetAllTasks);
    }
}
