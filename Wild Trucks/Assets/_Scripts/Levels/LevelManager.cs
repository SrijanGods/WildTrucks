using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public int maxSpeed;
    public GameObject player;
    public TextMeshProUGUI levelText;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        levelText.text = gameManager.currLevelName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
