using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameObject loadingPanel;
    GameObject loginPanel;
    LoginManager loginManager;
    public int maxSpeed;

    public string currLevelName;
    public bool isLoggedIn;

    private bool tag = true;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
    }

    void Update()
    {
        
    }

    public void OnLevelWasLoaded(int level)
    {
        if(currLevelName == "MainMenu")
        {
            MainMenuLoaded();
        }
    }

    public void MainMenuLoaded()
    {
        loginPanel = GameObject.FindGameObjectWithTag("LevelManager").gameObject;
        loginManager = loginPanel.GetComponent<LoginManager>();
        loadingPanel = loginManager.LoadingPanel;
        loginPanel.SetActive(false);
        loginManager.LoadingPanel.SetActive(false);
        loginManager.MapScene.SetActive(true);
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(currLevelName);
        Time.timeScale = 1;
    }

    //ERROR PART: CHANGE currLevelName!!!
    public void nextLevel()
    {
        SceneManager.LoadScene("MainMenu");
        currLevelName = "MainMenu"
;        Time.timeScale = 1;
    }

    public void LoadLevel(string levelName)
    {
        loadingPanel.SetActive(true);
        currLevelName = levelName;
        StartCoroutine(LoadSceneAsync(levelName));
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            loginManager.loadingBar.ChangeValue(progress*10);

            yield return null;
        }
    }
}
