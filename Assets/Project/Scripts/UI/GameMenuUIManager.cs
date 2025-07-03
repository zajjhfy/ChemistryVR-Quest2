using UnityEngine;

public class GameMenuUIManager : MonoBehaviour
{
    [Header("Layouts")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _lessons;
    [SerializeField] private GameObject _account;

    [Header("Authentification Layouts")]
    [SerializeField] private GameObject _unauthorizedLayout;
    [SerializeField] private GameObject _authorizedLayout;

    [Header("Warning window")]
    [SerializeField] private GameObject _warningWindow;

    public static GameMenuUIManager Instance;

    private ServerManager _accountManager;

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Start()
    {
        _accountManager = ServerManager.Instance;

        if (_accountManager.Session == Session.Authorized)
        {
            ChangeAccountAuthorized();
        }
        else ChangeAccountUnauthorized();
    }

    public void ChangeToOptions()
    {
        _options.SetActive(true);
        _menu.SetActive(false);
        _lessons.SetActive(false);
        _account.SetActive(false);
    }

    public void ChangeToMenu()
    {
        _menu.SetActive(true);
        _options.SetActive(false);
        _lessons.SetActive(false);
        _account.SetActive(false);
    }

    public void ChangeToLessons()
    {
        if(_accountManager.Session == Session.Unauthorized)
        {
            _warningWindow.SetActive(true);
            return;
        } 
        _lessons.SetActive(true);
        _menu.SetActive(false);
        _options.SetActive(false);
        _account.SetActive(false);
    }

    public void ChangeToAccount()
    {
        _account.SetActive(true);
        _lessons.SetActive(false);
        _menu.SetActive(false);
        _options.SetActive(false);
    }

    public void ChangeAccountAuthorized()
    {
        _authorizedLayout.SetActive(true);
        _unauthorizedLayout.SetActive(false);
    }

    public void ChangeAccountUnauthorized()
    {
        _unauthorizedLayout.SetActive(true);
        _authorizedLayout.SetActive(false);
    }

    public void QuitApplication() => Application.Quit();

    public void CloseWarningWindow() => _warningWindow.SetActive(false);    

}
