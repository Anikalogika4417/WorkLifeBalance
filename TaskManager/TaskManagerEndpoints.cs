using static TaskManager.TaskManagerLogic;

namespace TaskManager;

public static class TaskManagerEndpoints
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        var g = app.MapGroup("/api/tasks").WithTags("Tasks");

        g.MapGet("/tasks", GetAllTasksAsync);

        g.MapGet("/task/{id:Guid}", GetTaskByIdAsync);

        g.MapPost("/task/{id:Guid}", CreateTaskAsync);

        g.MapPut("/task/{id:Guid}", UpdateTaskAsync);

        g.MapDelete("/task/{id:Guid}", DeleteTaskAsync);
    }
}
