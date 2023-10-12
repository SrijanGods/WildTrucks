using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
//using Mono.Cecil.Cil;
using PlayFabError = PlayFab.PlayFabError;
using TMPro;
using System.Collections;
using Michsky.UI.ModernUIPack;

public class LoginManager : MonoBehaviour
{
    public TextMeshProUGUI debugInfo;
    public bool isDev;

    public GameObject LoginPanel, LoadingPanel, MapScene;

    public ProgressBar loadingBar;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (!gameManager.isLoggedIn)
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.DebugLogEnabled = true;
        }
        
    }

    public void OnSignInButtonClicked()
    {
        if (isDev)
        {

            DebugLogin();

        }
        else
        {
            
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        }

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            debugInfo.text = "Logging In";
            LoginPanel.SetActive(false);
            LoadingPanel.SetActive(true);
            loadingBar.ChangeValue(50f);

            string g = Social.localUser.id;
            Social.localUser.Authenticate((bool success) => {

                if (success)
                {
                    Debug.Log("Signed In");

                    loadingBar.ChangeValue(35f);

                    //var serverAuthCode;
                    PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
                    {
                        var serverAuthCode = code;

                        

                        PlayFabClientAPI.LoginWithGooglePlayGamesServices(new LoginWithGooglePlayGamesServicesRequest()
                        {
                            TitleId = PlayFabSettings.TitleId,
                            ServerAuthCode = serverAuthCode,
                            CreateAccount = true
                        }, (result) =>
                        {
                            Debug.Log("Signed In as " + result.PlayFabId);
                            OnLoginWithPlayfab();

                        }, OnLoginWithGoogleAccountFailure);
                    });


                }
                else
                {
                    Debug.Log("Google Failed to Authorize your login");
                }

            });
        }
        else
        {
            debugInfo.text = status.ToString();
            Debug.LogError(status);
        }
    }
    
    private void DebugLogin() 
    {
        loadingBar.ChangeValue(50f);
        debugInfo.text = "Logging In";
        //LoginPanel.SetActive(false);
        LoadingPanel.SetActive(true);

        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, (result) =>
        {
            Debug.Log("Signed In as " + result.PlayFabId);
            OnLoginWithPlayfab();

        }, OnLoginWithGoogleAccountFailure);
    }

    private void OnLoginWithGoogleAccountFailure(PlayFabError error)
    {
        Debug.Log("PlayFab LoginWithGoogleAccount Failure: " + error.GenerateErrorReport());

        debugInfo.text = "Failed: " + error.GenerateErrorReport();

        StartCoroutine(failedLogin());
        

    }

    private IEnumerator failedLogin()
    {
        yield return new WaitForSeconds(4);
        LoginPanel.SetActive(true);
        LoadingPanel.SetActive(false);
    }

    private void OnLoginWithPlayfab()
    {
        gameManager.MainMenuLoaded();
        loadingBar.ChangeValue(95f);

        debugInfo.text = "Loading Game";
        
        gameManager.isLoggedIn = true;
        
    }

    public void Quit()
    {
        Application.Quit();
    }

}
