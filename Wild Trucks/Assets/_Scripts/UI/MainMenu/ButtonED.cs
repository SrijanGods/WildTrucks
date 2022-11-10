using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;

public class ButtonED : MonoBehaviour
{
    public GameObject lockImage;
    public string sceneName;

    private ButtonManager btnMan;
    private Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btnMan = gameObject.GetComponent<ButtonManager>();
        btnMan.enabled = true;

        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
