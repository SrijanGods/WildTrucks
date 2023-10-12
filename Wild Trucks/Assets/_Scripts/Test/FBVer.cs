using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBVer : MonoBehaviour
{
    private static FBVer instance;
    private bool initCalled;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (FB.IsInitialized == false && initCalled == false)
        {
            FB.Init();
            initCalled = true;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("OnApplicationPause = " + pause);
        if (pause == false)
        {
            if (FB.IsInitialized == true)
            {
                FB.ActivateApp();
                FB.LogAppEvent(AppEventName.ActivatedApp);
                RunItOnce();
            }
            else
            {
                if (initCalled == false)
                {
                    FB.Init();
                    initCalled = true;
                }
                StartCoroutine(ActivateEvent());
            }
        }
    }

    private IEnumerator ActivateEvent()
    {
        yield return new WaitUntil(() => FB.IsInitialized == true);
        Debug.Log("Facebook Activate Event Logged");
        FB.ActivateApp();
        FB.LogAppEvent(AppEventName.ActivatedApp);
        RunItOnce();
    }

    private void RunItOnce()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
           // Debug.Log("First Time Opening");

            //Set first time opening to false
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            FB.LogAppEvent("fb_mobile_first_app_launch");
            //FB.LogAppEvent("fb_auto_published");

        }
        else
        {
            //Debug.Log("NOT First Time Opening");

            //Do your stuff here
        }
    }
}
