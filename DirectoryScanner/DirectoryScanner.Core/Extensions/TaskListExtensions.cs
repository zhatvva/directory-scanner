namespace DirectoryScanner.Core.Extensions
{
    internal static class TaskListExtensions
    {
        public static void ReplaceCompletedWithNewOrAdd(this List<Task> tasks, Task newTask)
        {
            for (var i = 0; i < tasks.Count; ++i)
            {
                if (tasks[i] != null && tasks[i].IsCompleted)
                {
                    tasks[i] = newTask;
                    return;
                }
            }

            tasks.Add(newTask);
        }
    }
}
