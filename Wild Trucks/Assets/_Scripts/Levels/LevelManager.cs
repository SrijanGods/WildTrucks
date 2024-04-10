using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public int maxSpeed;
    public GameObject player;
    public GameManager gameManager;
    [Header("UI")]
    public GameObject levelText;
    public GameObject Graphy;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        StartCoroutine(VanishUI());
        if (gameManager.isDebug)
        {
            Graphy.SetActive(true);
        }
    }

    IEnumerator VanishUI()
    {
        yield return new WaitForSeconds(3f);
        levelText.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
