﻿namespace LibiadaWeb.Tasks
{
    using System;

    using LibiadaCore.Extensions;

    using LibiadaWeb.Models;

    using Microsoft.AspNet.SignalR;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// SignalR messages hub class.
    /// </summary>
    public class TasksManagerHub : Hub
    {
        /// <summary>
        /// Send web socket message to all clients.
        /// </summary>
        /// <param name="taskEvent">
        /// Task event.
        /// </param>
        /// <param name="task">
        /// Task itself.
        /// </param>
        public static void Send(TaskEvent taskEvent, TaskData task)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TasksManagerHub>();

            lock (task)
            {
                var result = new
                {
                    task.Id,
                    task.DisplayName,
                    Created = task.Created.ToString(OutputFormats.DateTimeFormat),
                    Started = task.Started == null ? string.Empty : ((DateTimeOffset)task.Started).ToString(OutputFormats.DateTimeFormat),
                    Completed = task.Completed == null ? string.Empty : ((DateTimeOffset)task.Completed).ToString(OutputFormats.DateTimeFormat),
                    ExecutionTime = task.ExecutionTime == null ? string.Empty : ((TimeSpan)task.ExecutionTime).ToString(OutputFormats.TimeFormat),
                    TaskState = task.TaskState.ToString(),
                    TaskStateName = task.TaskState.GetDisplayValue(),
                    task.UserId,
                    task.UserName
                };

                hubContext.Clients.All.TaskEvent(taskEvent.ToString(), result);
            }
        }

        /// <summary>
        /// Called by clients on connect.
        /// </summary>
        public string GetAllTasks()
        {
            var tasks = TaskManager.GetTasksData().Select(t => new
            {
                t.Id,
                t.DisplayName,
                Created = t.Created.ToString(OutputFormats.DateTimeFormat),
                Started = t.Started == null ? string.Empty : ((DateTimeOffset)t.Started).ToString(OutputFormats.DateTimeFormat),
                Completed = t.Completed == null ? string.Empty : ((DateTimeOffset)t.Completed).ToString(OutputFormats.DateTimeFormat),
                ExecutionTime = t.ExecutionTime == null ? string.Empty : ((TimeSpan)t.ExecutionTime).ToString(OutputFormats.TimeFormat),
                TaskState = t.TaskState.ToString(),
                TaskStateName = t.TaskState.GetDisplayValue(),
                t.UserId,
                t.UserName
            });

            return JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    { "tasks", tasks.ToArray() }
                });

        }
    }
}