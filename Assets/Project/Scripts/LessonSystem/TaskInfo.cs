using UnityEngine;

[System.Serializable]
public class TaskInfo
{
    public string caller;
    public string description;
    public TaskActionType actionType;
    public string args;
    public bool isCompleted;
}
