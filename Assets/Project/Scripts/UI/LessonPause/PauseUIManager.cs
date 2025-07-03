using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _options;

    public void ChangeToOptions()
    {
        _options.SetActive(true);
        _pause.SetActive(false);
    }

    public void ChangeToPause()
    {
        _pause.SetActive(true);
        _options.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }

    public void Resume()
    {
        Destroy(gameObject);
    }
}
