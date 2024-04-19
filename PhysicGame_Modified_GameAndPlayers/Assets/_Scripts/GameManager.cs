using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    LPPV_CarController playerController;
    public bool isSunk;

    GameObject player;
    GameObject playerPrefab;
    public Vector3 startingPoint;

    GameObject newPlayer;

    // For the Start of the game
    // Message is the main picture in the center of the screen
    // Go is the button
    public bool isStart = false;
    public Canvas startUI;
    public Image startMessage;
    public List<Sprite> startMsgSpritePrefabs = new List<Sprite>();
    private int currentStartSprite = 0;
    public Image go;
    public Sprite goNext;
    public Sprite goFinal;

    // Over
    public bool isOver;
    public Canvas overPageUI;
    public Image overMessage;
    private Sprite overMsgSprite;

    // Pausing
    public bool isPaused;
    public Canvas pausePageUI;
    public Image pauseMessage;
    private Sprite pauseMsgSprite;

    // For HUD (showing how many gift left)
    public Canvas HUD;
    public GameObject giftPrefab;
    public RectTransform canvasRect;
    public float giftGapX;
    public int startPosX;

    // For InstructionMessage
    public Image instructionMessage;

    // For package managing
    public int packageTotal; // Total amount of packages
    public int packageNum = 0; // How many package(s) already delivered

    //public GameObject lastClient;

    // For calling other functions
    public UnityEvent playerRespawned;
    public UnityEvent oceanColliderRespawned;

    // For adding HitBumper script for every Hard Object
    GameObject[] hardObjects;

    // For buffer before loading End Scene
    public float endBuffer = 2.0f;

    // For ending
    public EndingManager endingManager;

    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = Resources.Load<GameObject>("Utility");
        startingPoint = new Vector3(0f, 3.42f, 0f);

        // Set up the start UI
        startUI = GameObject.FindGameObjectWithTag("StartUI").GetComponent<Canvas>();
        startMsgSpritePrefabs = Resources.LoadAll<Sprite>("StartUIMsg").ToList();

        startMessage = GameObject.FindGameObjectWithTag("StartMessage").GetComponent<Image>();
        go = GameObject.FindGameObjectWithTag("StartGo").GetComponent<Image>();

        goNext = Resources.Load<Sprite>("StartUIGo/GoNext");
        goFinal = Resources.Load<Sprite>("StartUIGo/GoFinal");

        // Set Over and Paused canvas not visibal at first
        // Over
        overPageUI = GameObject.Find("OverPage").GetComponent<Canvas>();

        overMessage = GameObject.Find("OverMessage").GetComponent<Image>();
        overMsgSprite = Resources.Load<Sprite>("OverUIMsg/Over1");
        overMessage.sprite = overMsgSprite;

        overPageUI.enabled = false;

        // Pause
        pausePageUI = GameObject.Find("PausePage").GetComponent<Canvas>();

        pauseMessage = GameObject.Find("PauseMessage").GetComponent<Image>();
        pauseMsgSprite = Resources.Load<Sprite>("PauseUIMsg/Pause1");
        pauseMessage.sprite = pauseMsgSprite;

        pausePageUI.enabled = false;

        hardObjects = GameObject.FindGameObjectsWithTag("HardObject");
        foreach (GameObject obj in hardObjects)
        {
            if (!obj.GetComponent<HitBumper>())
            {
                obj.AddComponent<HitBumper>();
            } 
        }

        // HUD
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        giftPrefab = Resources.Load<GameObject>("HUD/HUDGift");

        // InstructionMessage
        instructionMessage = GameObject.Find("InstructionMessage").GetComponent<Image>();

        // For Ending
        endingManager = GameObject.Find("EndingManagerHolder").GetComponent<EndingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Keep updating the current "player" game object
        player = GameObject.FindGameObjectWithTag("PlayerWhole");
        playerController = player.GetComponent<LPPV_CarController>();

        //Debug.Log(carController.isSinking);

        if (player == null)
        {
            Debug.Log("CANNOT FIND PLAYER");
        }

        // The start scene
        if (!isStart)
        {
            // Start UI showed
            startUI.enabled = true;
            
            isPaused = false;
            isOver = false;
            instructionMessage.enabled = false;

            startMessage.sprite = startMsgSpritePrefabs[currentStartSprite];

            // The message part of the UI
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentStartSprite++;
            }

            // The button part of the UI
            if (currentStartSprite < startMsgSpritePrefabs.Count -1)
            {
                go.sprite = goNext;
            }
            else if (currentStartSprite == startMsgSpritePrefabs.Count -1)
            {
                go.sprite = goFinal;
            }

            // After showing all the messages, start the game
            if (currentStartSprite == startMsgSpritePrefabs.Count && Input.GetKeyDown(KeyCode.F))
            {
                isStart = true;
            }
        }
        else if (isStart)
        {
            startUI.enabled = false;
            instructionMessage.enabled = true;
        }

        if (isStart && !isOver && !isPaused)
        {
            UpdateGiftUI();
        }

        // If the player hit the ocean, game over
        if (playerController.isSinking == true)
        {
            GameOver();
        }

        // Pause the game and call the main menu
        // If the car already sunk, cannot use this
        if (isStart && !isOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // The button in main menu and execute them
        if (isPaused || isOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Get a new truck");
                RespawnPlayer();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Quit Game");
                Application.Quit();
            }
        }
        /*else if (isStart && isPaused && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }*/

        // Receiving packages
        playerController.packageReceiving.AddListener(PackageManager);
        Debug.Log(packageNum);

        if (packageNum >= packageTotal)
        {
            Debug.Log("Win!");


            StartCoroutine(ToEndLevel());
        }

        // For InstructionMessages
        if (isStart)
        {
            if(isPaused || isOver)
            {
                instructionMessage.enabled = false;
            }
            else if (!isPaused)
            {
                instructionMessage.enabled = true;
            }

        }
    }

    // Sinking truck
    void GameOver()
    {
        isOver = true;
        overPageUI.enabled = true;
    }

    void PauseGame()
    {
        Debug.Log("Need some help?");

        Time.timeScale = 0.0f;
        pausePageUI.enabled= true;
        isPaused = true;
    }

    void ResumeGame()
    {
        Debug.Log("Okay");

        Time.timeScale = 1.0f;
        pausePageUI.enabled = false;
        isPaused = false;
    }

    public void RespawnPlayer()
    {
        // Set Over/Paused canvas back to invisible
        overPageUI.enabled = false;
        ResumeGame();

        newPlayer =  Instantiate(playerPrefab, startingPoint, Quaternion.identity);

        Destroy(player);
        player = newPlayer;

        playerController.isSinking = false;
        isOver = false;

        RefreshSmoke("Smoke");

        // Work with Listeners
        playerRespawned?.Invoke();
        oceanColliderRespawned?.Invoke();
    }

    public void RefreshSmoke(string tag)
    {
        // When respawing the player, destroy all existed smoke object
        GameObject[] smokeObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject smoke in smokeObjects)
        {
            Destroy(smoke);
        }
    }

    public void PackageManager()
    {
        // Audio playing

    }

    public void UpdateGiftUI()
    {
        foreach (Transform child in HUD.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < (packageTotal - packageNum); i++)
        {
            GameObject newGiftImage = Instantiate(giftPrefab, HUD.transform);
            newGiftImage.transform.localPosition = new Vector3(startPosX + i * 120, 450, 0);
        }
    }

    IEnumerator ToEndLevel()
    {
        Debug.Log("Delay started");
        yield return new WaitForSeconds(endBuffer);
        Debug.Log("Delay finished");

        SceneManager.LoadScene("EndScene");
    }
}
