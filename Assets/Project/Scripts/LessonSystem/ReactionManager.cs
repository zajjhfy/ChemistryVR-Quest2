using System;
using TMPro;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    [SerializeField] private LessonInfo _lessonInfo;

    [SerializeField] private TMP_InputField _reactionInput1;

    [SerializeField] private Tablet _tablet;

    public event EventHandler<ReactionEventArgs> OnReactionUpdate;

    public static ReactionManager Instance;

    public LessonInfo LessonInfo => _lessonInfo;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _tablet.OnReactionsComplete += OnReactionsCompleteHandler;

        _reactionInput1.onValueChanged.AddListener(x =>
        {
            if (_lessonInfo.reactions[0].reaction == x)
            {
                OnReactionUpdate?.Invoke(this, new ReactionEventArgs { inputField = _reactionInput1});
                _reactionInput1.interactable = false;
            }
        });

    }

    private async void OnReactionsCompleteHandler(object sender, EventArgs e)
    {
        var request = await ServerManager.Instance.RequestMarkLessonComplete(_lessonInfo.lessonId, "Зачёт");

        if (request)
        {
            Debug.Log("Данные урока были успешно обновлены!");
        }
        else Debug.Log("Ошибка в обновлении данных урока");

        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }
}
