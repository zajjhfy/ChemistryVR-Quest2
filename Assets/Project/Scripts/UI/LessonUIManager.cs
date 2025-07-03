using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LessonUIManager : MonoBehaviour
{
    [Header("Lessons Buttons")]
    [SerializeField] private Button[] _lessonsButton;

    [Header("Lessons Text")]
    [SerializeField] private TextMeshProUGUI[] _lessonsText;

    [Header("Lesson Grades")]
    [SerializeField] private TextMeshProUGUI[] _lessonsGrades;

    [Header("Start Lesson Button")]
    [SerializeField] private Button _startLessonButton;

    private ServerManager _serverManager;

    private int _id = 0;

    private async void OnEnable()
    {
        _serverManager = ServerManager.Instance;

        await LoadLessonInfo();
    }

    private void Start()
    {
        foreach(var val in _lessonsButton)
        {
            val.onClick.AddListener(() => _id = val.GetComponent<LessonReferenceId>().Id);
        }

        _startLessonButton.onClick.AddListener(LoadLesson);
    }

    private async Task LoadLessonInfo()
    {
        bool request = await _serverManager.RequestUserLessonInfoDataAsync();

        if (request)
        {
            _serverManager.TryGetLessonInfoResponse(out var info);
            int i = 0;

            foreach (var item in _lessonsText)
            {
                foreach (var lesson in info.lessonData)
                {
                    if (lesson.lesson_name == item.text)
                    {
                        _lessonsGrades[i].text = lesson.student_grade;
                        i++;
                        break;
                    }
                }
            }
        }
    }

    private void LoadLesson()
    {
        if (_id == 0) return;

        SceneTransitionManager.singleton.GoToScene(_id);
    }

    private void OnDisable()
    {
        _id = 0;
    }

}
