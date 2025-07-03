using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;

    public Session Session { get
        {
            bool val = GetIsTokenNotNull();
            if (val)
                return Session.Authorized;
            return Session.Unauthorized;
        } }


    private const string domain = "https://learn-science.ru";

    private const string apiGetToken = "/api/verify-code/";

    private const string apiGetUserData = "/api/get-username-by-token/";
    private const string apiGetLessonData = "/api/lesson/get-info/";
    private const string apiMarkLessonComplete = "/api/lesson/complete";

    private UserInfo _userInfo;
    private UserLessonInfoResponse _lessonData;

    private string _token;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _token = GetToken();
        _userInfo = GetUserInfo();

        Debug.Log("token: " + _token);
        Debug.Log("userinfo: " + _userInfo);
    }

    public async Task<bool> RequestVerifyCodeAsync(TMP_InputField _inputField)
    {
        string code = _inputField.text;

        if (!GetIsTokenNotNull())
        {
            return await GetTokenByCode(code);
        }
        return false;

    }

    public async Task<bool> RequestUserInfoDataAsync()
    {
        if (GetIsTokenNotNull())
        {
            return await GetUserInfoByToken();
        }
        return false;
    }

    public async Task<bool> RequestUserLessonInfoDataAsync()
    {
        if (GetIsTokenNotNull())
        {
            return await GetUserLessonInfoByToken();
        }
        return false;
    }

    public async Task<bool> RequestMarkLessonComplete(int lessonId, string score)
    {
        if (GetIsTokenNotNull())
        {
            return await MarkLessonComplete(lessonId, score);
        }
        return false;
    }

    private async Task<bool> GetTokenByCode(string code)
    {
        string url = domain + apiGetToken;

        string json = "{\"code\":\"" + code + "\"}";
        byte[] rawData = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string token = request.downloadHandler.text;
            SaveToken(token);

            Debug.Log(token);

            return true;
        }
        else{ 
            Debug.Log(request.error);

            return false;
        }

    }

    private async Task<bool> GetUserInfoByToken()
    {
        string url = domain + apiGetUserData;

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", "Bearer " + _token);

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string userInfo = request.downloadHandler.text;
            SaveUserInfo(userInfo);
            
            Debug.Log(userInfo);

            return true;
        }
        else
        {
            Debug.Log(request.error);

            return false;
        }
    }

    private async Task<bool> GetUserLessonInfoByToken()
    {
        string url = domain + apiGetLessonData;

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", "Bearer " + _token);
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            UserLessonInfoResponse response = JsonUtility.FromJson<UserLessonInfoResponse>(request.downloadHandler.text);

            _lessonData = response;

            foreach(var lesson in response.lessonData)
            {
                Debug.Log($"id: {lesson.lesson_id}; name: {lesson.lesson_name}; grade: {lesson.student_grade}");
            }

            return true;
        }
        else
        {
            Debug.Log(request.error);

            return false;
        }
    }

    private async Task<bool> MarkLessonComplete(int lessonId, string score)
    {
        string url = domain + apiMarkLessonComplete;

        string json = $"{{\"lessonId\": {lessonId}, \"score\": \"{score}\"}}";
        byte[] raw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(raw);


        request.SetRequestHeader("Authorization", "Bearer " + _token);
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(result))
            {
                Debug.Log(result);
                return true;
            }

            return false;
        }
        else
        {
            Debug.Log(request.error);

            return false;
        }
    }

    private void SaveToken(string token)
    {
        token = token.Remove(0, 10).Split("\"}", System.StringSplitOptions.None)[0];
        _token = token;

        PlayerPrefs.SetString("token", token);
    }

    private void SaveUserInfo(string userInfo)
    {
        _userInfo = JsonUtility.FromJson<UserInfo>(userInfo);

        PlayerPrefs.SetString("userInfo", userInfo);
    }

    private string GetToken() => PlayerPrefs.GetString("token");

    private UserInfo GetUserInfo()
    {
        string info = PlayerPrefs.GetString("userInfo");

        return JsonUtility.FromJson<UserInfo>(info);
    }

    // true if not null
    public bool GetIsTokenNotNull()
    {
        if(!string.IsNullOrEmpty(_token) || !string.IsNullOrWhiteSpace(_token))
        {
            return true;
        }

        return false;
    }

    public bool TryGetUserInfo(out UserInfo info)
    {
        info = null;

        if(_userInfo != null)
        {
            info = _userInfo;
            return true;
        }
        return false;
    }

    public bool TryGetLessonInfoResponse(out UserLessonInfoResponse info)
    {
        info = null;

        if (_lessonData != null)
        {
            info = _lessonData;
            return true;
        }
        return false;
    }

    public void ClearConnection()
    {
        PlayerPrefs.SetString("token", null);
        _token = null;

        PlayerPrefs.SetString("userInfo", null);
        _userInfo = null;
    }

}
