using Microsoft.AspNetCore.Mvc;

namespace TaskManager;

public static class TaskManagerLogic
{
    internal static async Task GetAllTasksAsync(
        [FromServices] HttpClientHandler httpHandler)
    {
        // Http Call 
        try
        {

        }
        catch (Exception ex)
        {
            // Log exception
            throw;
        }
    }

    internal static async Task GetTaskByIdAsync(
        [FromQuery] Guid id)
    {
        // Http Call 
        try
        {

        }
        catch (Exception ex)
        {
            // Log exception
            throw;
        }
    }

    internal static async Task CreateTaskAsync(
        [FromQuery] Guid id)
    {
        // Http Call 
        try
        {

        }
        catch (Exception ex)
        {
            // Log exception
            throw;
        }
    }

    internal static async Task UpdateTaskAsync(
        [FromQuery] Guid id)
    {
        // Http Call 
        try
        {

        }
        catch (Exception ex)
        {
            // Log exception
            throw;
        }
    }

    internal static async Task DeleteTaskAsync(
        [FromQuery] Guid id)
    {
        // Http Call 
        try
        {

        }
        catch (Exception ex)
        {
            // Log exception
            throw;
        }
    }
}
