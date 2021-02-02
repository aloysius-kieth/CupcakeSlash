using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

#if NET_4_6
using System.Threading.Tasks;
#endif

/// <summary>
/// Async version of server manager
/// </summary>
public class TrinaxAsyncServerManager : MonoBehaviour
{
    #region SINGLETON
    public static TrinaxAsyncServerManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public string ip = "127.0.0.1";
    public bool useServer;
    public bool useMocky;
    public GameObject loadingCircle;

    public string userID;

    const string frontUrl = "Thermofisher/servlet/api.do/";
    const string port = ":8080/";

    public const string ERROR_CODE_200 = "200";
    public const string ERROR_CODE_201 = "201";

    bool isLoading = false;
    bool isVerifying = false;
    bool isDelayedScanCoroutineRunning = false;

    string LOG_DIR = "/log/";
    string LOG_FILE = "Async_server_logs.log";

    #if NET_4_6
#region API Funcs
    public async Task AddInteractionAsync(AddinteractionSendData json, Action<bool, AddinteractionReceiveData> callback)
    {
        AddinteractionReceiveData data;
        if (!useServer)
        {
            data = new AddinteractionReceiveData();
            callback(true, data);
            return;
        }

        string sendJson = JsonUtility.ToJson(json);
        string url;

        if (!useMocky)
        {
            //Debug.Log("Using actual server url...");
            url = "http://" + ip + port + frontUrl + "addInteraction";
        }
        else
        {
            //Debug.Log("Using mocky url...");
            url = "http://www.mocky.io/v2/5b6122fb3000007f006a3ffc";
        }

        var r = await LoadPostUrl(url, sendJson, (request) =>
        {
            if (string.IsNullOrEmpty(request.error))
            {
                string result = request.text.Trim();
                data = JsonUtility.FromJson<AddinteractionReceiveData>(result);
                //Debug.Log("myresult: " + result);

                if (data.errorCode == ERROR_CODE_200)
                {
                    callback(true, data);
                }
                else
                {
                    callback(false, data);
                }
            }
            else
            {
                WriteError(request, "AddInteraction");
                data = new AddinteractionReceiveData();
                callback(false, data);
            }
        });
    }

    public async Task Register(RegisterSendJsonData json, Action<bool, RegisterReceiveJsonData> callback)
    {
        RegisterReceiveJsonData data;
        if (!useServer)
        {
            data = new RegisterReceiveJsonData();
            callback(true, data);
            return;
        }

        string sendJson = JsonUtility.ToJson(json);

        string url;
        if (!useMocky)
        {
            url = "http://" + ip + port + frontUrl + "updateInfo";
        }
        else
        {
            url = "http://www.mocky.io/v2/5ba30bbc2f00004f008d2eec";
        }

        var r = await LoadPostUrl(url, sendJson, (request) =>
        {
            if (string.IsNullOrEmpty(request.error))
            {
                string result = request.text.Trim();
                data = JsonUtility.FromJson<RegisterReceiveJsonData>(result);
                //Debug.Log("myresult: " + result);

                if (data.error.errorCode == ERROR_CODE_200)
                {
                    callback(true, data);
                }
                else
                {
                    callback(false, data);
                }
            }
            else
            {
                WriteError(request, "UpdateInfo");
                data = new RegisterReceiveJsonData();
                callback(false, data);
            }
        });
    }

    public async Task AddResult(Action<bool, AddResultReceiveJsonData> callback)
    {
        AddResultReceiveJsonData data;
        if (!useServer)
        {
            data = new AddResultReceiveJsonData();
            callback(true, data);
            return;
        }

        string url;
        if (!useMocky)
        {
            url = "http://" + ip + port + frontUrl + "addGameResult/" + ScoreManager.Instance.Score;
        }
        else
        {
            url = "http://www.mocky.io/v2/5ba30c362f00005c008d2eed";
        }

        var r = await LoadUrlAsync(url, (request) =>
        {
            if (string.IsNullOrEmpty(request.error))
            {
                string result = request.text.Trim();
                data = JsonUtility.FromJson<AddResultReceiveJsonData>(result);
                //Debug.Log("myresult: " + result);

                if (data.error.errorCode == ERROR_CODE_200)
                {
                    callback(true, data);
                }
                else if (data.error.errorCode == ERROR_CODE_201)
                {
                    callback(true, data);
                }
                {
                    callback(false, data);
                }
            }
            else
            {
                WriteError(request, "AddResult");
                data = new AddResultReceiveJsonData();
                callback(false, data);
            }
        });
    }

    public async Task PopulateLeaderboard(Action<bool, LeaderboardReceiveJsonData> callback)
    {
        LeaderboardReceiveJsonData data;
        if (!useServer)
        {
            data = new LeaderboardReceiveJsonData();
            callback(true, data);
            return;
        }

        string url;
        if (!useMocky)
        {
            Debug.Log("Using actual server url...");
            url = "http://" + ip + port + frontUrl + "getTop10";
        }
        else
        {
            Debug.Log("Using mocky url...");
            url = "http://www.mocky.io/v2/5ba30c4e2f000077008d2eee";
        }

        var r = await LoadUrlAsync(url, (request) =>
        {
            if (string.IsNullOrEmpty(request.error))
            {
                string result = request.text.Trim();
                data = JsonUtility.FromJson<LeaderboardReceiveJsonData>(result);
                //Debug.Log("myresult: " + result);

                if (data.error.errorCode == ERROR_CODE_200)
                {
                    callback(true, data);
                }
                else
                {
                    callback(false, data);
                }
            }
            else
            {
                WriteError(request, "PopulateLeaderboard");
                data = new LeaderboardReceiveJsonData();
                callback(false, data);
            }
        });
    }

#endregion

    async Task<string> LoadUrlAsync(string url, Action<WWW> callback)
    {
        isLoading = true;

        DelayLoadingCircle();

        Debug.Log("Loading url: " + url);
        var request = await new WWW(url);
        Debug.Log(url + "\n" + request.text);

        //await new WaitForSeconds(3.0f); // artifical wait
        callback(request);

        isLoading = false;
        if (loadingCircle != null)
        {
            loadingCircle.SetActive(false);
        }

        return request.text;
    }

    async Task<string> LoadPostUrl(string url, string jsonString, Action<WWW> callback)
    {
        isLoading = true;
        //then set the headers Dictionary headers=form.headers; headers["Content-Type"]="application/json";

        DelayLoadingCircle();

        WWWForm form = new WWWForm();
        byte[] jsonSenddata = null;
        if (!string.IsNullOrEmpty(jsonString))
        {
            Debug.Log(jsonString);
            jsonSenddata = System.Text.Encoding.UTF8.GetBytes(jsonString);
        }

        form.headers["Content-Type"] = "application/json";
        form.headers["Accept"] = "application/json";
        Dictionary<string, string> headers = form.headers;
        headers["Content-Type"] = "application/json";

        Debug.Log("Loading url: " + url);
        var request = await new WWW(url, jsonSenddata, headers);
        Debug.Log(url + "\n" + request.text);


        await new WaitForSeconds(0.1f); // artifical wait for 150ms

        callback(request);

        isLoading = false;
        if (loadingCircle != null)
        {
            loadingCircle.SetActive(false);
        }

        return request.text;
    }

    async void DelayLoadingCircle()
    {
        await new WaitForSeconds(1.5f);

        if (isLoading && loadingCircle != null)
            loadingCircle.SetActive(true);
    }

    async void DelayQrScanner()
    {
        isDelayedScanCoroutineRunning = true;
        await new WaitForSeconds(3f);

        isVerifying = false;
        isDelayedScanCoroutineRunning = false;
    }
#endif

    void WriteError(WWW request, string api)
    {
        string error = "<" + api + "> --- Begin Error Message: " + request.error + " >> Url: " + request.url + System.Environment.NewLine;
        File.AppendAllText(LOG_DIR + LOG_FILE, error);
    }

    void WriteError(string errorStr, string api)
    {
        string error = "<" + api + "> --- Begin Error Message: " + errorStr + System.Environment.NewLine;
        File.AppendAllText(LOG_DIR + LOG_FILE, error);
    }

    private void Start()
    {
        LOG_DIR = Application.dataPath + LOG_DIR;
        Debug.Log(LOG_DIR);
        if (!Directory.Exists(LOG_DIR))
        {
            Debug.Log("Created " + Directory.CreateDirectory(LOG_DIR));
            Directory.CreateDirectory(LOG_DIR);
        }

        if (loadingCircle != null)
            loadingCircle.SetActive(false);
    }
}
