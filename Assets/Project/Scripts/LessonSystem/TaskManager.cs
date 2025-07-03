using System;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public LessonInfo lessonInfo;

    public event EventHandler<LessonInfoEventArgs> OnTaskInfoUpdated;
    public event EventHandler OnLastTaskUnlocked;

    private TaskInfo[] _tasks;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _tasks = lessonInfo.tasks;
    }

    private void Start()
    {
        OnTaskInfoUpdated?.Invoke(this, new LessonInfoEventArgs { lessonInfo = this.lessonInfo});
    }

    public static void MarkTaskCompleted(string caller, TaskActionType type, string args = "")
    {
        var task = TaskManager.Instance._tasks.FirstOrDefault(task => 
        {
            return task.caller.Equals(caller) && task.actionType == type && task.args.Equals(args)
            && !task.isCompleted;
        });

        if(task != null)
        {
            Debug.Log(task.description);
            task.isCompleted = true;

            TaskManager.Instance.OnTaskInfoUpdated?.Invoke(TaskManager.Instance,
                new LessonInfoEventArgs { lessonInfo = TaskManager.Instance.lessonInfo });

            if (TaskManager.Instance._tasks.All(task => task.isCompleted))
            {
                TaskManager.Instance.OnLastTaskUnlocked?.Invoke(TaskManager.Instance, EventArgs.Empty);
            };
        }
    }

    private void OnDisable()
    {
        if (_tasks == null) return;

        foreach (var item in _tasks.Where(task => task.isCompleted))
        {
            item.isCompleted = false;
        }
    }
}
