using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tablet : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private ReactionManager _reactionManager;

    [Header("Tasks Canvas")]
    [SerializeField] private TextMeshProUGUI _tasksText;
    [SerializeField] private Button _continueButton;

    [Header("Reactions Canvas")]
    [SerializeField] private TextMeshProUGUI _reactionText1;
    [SerializeField] private Button _confirmButton;

    [Header("Canvases")]
    [SerializeField] private GameObject _tasksCanvas;
    [SerializeField] private GameObject _reactionsCanvas;

    [Header("Backgrounds")]
    [SerializeField] private GameObject _reactionsBackground;
    [SerializeField] private GameObject _lessonCompleteBackground;

    public event EventHandler OnReactionsComplete;

    private bool _isLastTaskUnlocked = false;

    private int _reactionsSuccess = 0;
    private string _greenHex = "#05f505";
    private string _redHex = "red";

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(ContinueButtonClickHandler);
        _confirmButton.onClick.AddListener(() =>
        {
            if (_reactionsSuccess == 1)
            {
                _reactionsBackground.SetActive(false);
                _lessonCompleteBackground.SetActive(true);

                OnReactionsComplete?.Invoke(this, EventArgs.Empty);
            }
        });

        _taskManager.OnTaskInfoUpdated += UpdateTasksUIHandler;
        _reactionManager.OnReactionUpdate += UpdateReactionUIHandler;

        _taskManager.OnLastTaskUnlocked += (x, y) => _isLastTaskUnlocked = true;
    }

    private void UpdateReactionUIHandler(object sender, ReactionEventArgs e)
    {
        var inputFieldToUpdate = e.inputField;
        _reactionsSuccess++;

        inputFieldToUpdate.text = $"<color={_greenHex}>{inputFieldToUpdate.text}";
    }

    private void UpdateTasksUIHandler(object sender, LessonInfoEventArgs e)
    {
        string text = "";
        int i = 1;
        string hex = _redHex;

        var tasks = e.lessonInfo.tasks;

        foreach (var task in tasks)
        {
            hex = task.isCompleted ? _greenHex : _redHex;

            text += "<b>" + i + $". </b><color={hex}>{task.description}</color>\n";
            i++;
        }

        _tasksText.text = text;
    }

    private void SetReactionsText()
    {
        _reactionText1.text = $"<b>1. </b>{_reactionManager.LessonInfo.reactions[0].description}";
    }

    private void ContinueButtonClickHandler()
    {
        if (_isLastTaskUnlocked)
        {
            _tasksCanvas.SetActive(false);
            _reactionsCanvas.SetActive(true);
            SetReactionsText();
        }
    }
}
