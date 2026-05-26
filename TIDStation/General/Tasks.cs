using TIDStation.Radio;
using TIDStation.Serial;

namespace TIDStation.General
{
    public static class Tasks
    {
        private static readonly List<Task> tasks = [];
        private static Task? watcher = null;

        public static Task Watch
        {
            set
            {
                lock (tasks)
                {
                    tasks.Add(value);
                    if (watcher == null || watcher.IsCompleted)
                    {
                        watcher = Task.Run(Watchdog);
                    }
                }
            }
        }

        private static void Watchdog()
        {
            while(true)
            {
                Thread.Sleep(1000);
                if (Comms.Ready) TD.Update();
                lock (tasks)
                {
                    foreach (var task in tasks.ToArray())
                    {
                        if (task.IsCompleted)
                        {
                            using (task)
                            {
                                tasks.Remove(task);
                            }
                        }
                    }
                    if(tasks.Count == 0)
                    {
                        tasks.Add(watcher!);
                        return;
                    }
                }
            }
        }

    }
}
