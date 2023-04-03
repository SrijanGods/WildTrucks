using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.EditorModels;
using Mono.Cecil.Cil;
using PlayFabError = PlayFab.PlayFabError;

public class LoginManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("Success");
        }
        else
        {
            Debug.Log(status);
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }



    public void OnSignInButtonClicked()
    {
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        /*
        Social.localUser.Authenticate((bool success) => {

            if (success)
            {
                Debug.Log("Signed In");

                //var serverAuthCode;
                PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
                {
                    var serverAuthCode = code;

                    PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
                    {
                        TitleId = PlayFabSettings.TitleId,
                        ServerAuthCode = serverAuthCode,
                        CreateAccount = true
                    }, (result) =>
                    {
                        Debug.Log("Signed In as " + result.PlayFabId);

                    }, OnLoginWithGoogleAccountFailure);
                });

                
            }
            else
            {
                Debug.Log("Google Failed to Authorize your login");
            }

        });
        */

    }

    private void OnLoginWithGoogleAccountFailure(PlayFabError error)
    {
        Debug.Log("PlayFab LoginWithGoogleAccount Failure: " + error.GenerateErrorReport());
    }


}
