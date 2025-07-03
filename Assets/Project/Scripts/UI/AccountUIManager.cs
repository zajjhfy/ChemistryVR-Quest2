using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class AccountUIManager : MonoBehaviour
{
    [Header("Authorized layout")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _userInfoText;

    [Header("Unauthorized layout")]
    [SerializeField] private Button _loginButton;
    [SerializeField] private TextMeshProUGUI _errorText;
    [SerializeField] private TMP_InputField _codeField;

    private ServerManager _serverManager;
    private GameMenuUIManager _uiManager;

    private void Start()
    {
        _serverManager = ServerManager.Instance;
        _uiManager = GameMenuUIManager.Instance;

        if(_serverManager.TryGetUserInfo(out var info))
        {
            SetUserInfoText(info);
        }

        _loginButton.onClick.AddListener(GetCode);
        _exitButton.onClick.AddListener(SetUnauthorizedLayout);
    }

    private async void GetCode()
    {
        if (_serverManager.Session == Session.Authorized) return;

        bool request = await _serverManager.RequestVerifyCodeAsync(_codeField);

        if (request)
        {
            await SetAuthorizathionLayout();
        }
        else
        {
            _errorText.text = "<color=red>Произошла ошибка.\nПроверьте правильность ввода кода";
        }
    }

    private async Task SetAuthorizathionLayout()
    {
        if (_serverManager.TryGetUserInfo(out var info))
        {
            _uiManager.ChangeToMenu();
            _uiManager.ChangeAccountAuthorized();
            _errorText.text = null;

            SetUserInfoText(info);

            return;
        }

        bool request = await _serverManager.RequestUserInfoDataAsync();

        if (request)
        {
            _uiManager.ChangeToMenu();
            _uiManager.ChangeAccountAuthorized();
            _errorText.text = null;

            _serverManager.TryGetUserInfo(out info);

            SetUserInfoText(info);
        } 
        else
        {
            _serverManager.ClearConnection();
            _errorText.text = "<color=red>Произошла ошибка.\nНе получилось извлечь данные";
        }
    }

    private void SetUnauthorizedLayout()
    {
        _serverManager.ClearConnection();

        _uiManager.ChangeToMenu();
        _uiManager.ChangeAccountUnauthorized();
    }

    private void SetUserInfoText(UserInfo info)
    {
        _userInfoText.text = "Вы вошли в аккаунт как:\n" + info.userName;
    }

}
