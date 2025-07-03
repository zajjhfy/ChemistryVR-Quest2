using UnityEngine;

[CreateAssetMenu]
public class LessonInfo : ScriptableObject
{
    public int lessonId;
    public TaskInfo[] tasks;
    public ReactionInfo[] reactions;
}
