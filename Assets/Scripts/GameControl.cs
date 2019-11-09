using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public PlacementManager dart;
    public TargetsSpawn target;
    public CountDown countDown;

    private bool isCountDownFinished = false;
    private bool isGameOver = false;

    public GameObject gameTexts;

    public GameObject rematchButton;
    public Text buttonText;

    public AudioSource gameOVerAudio;

    public Text livesText;
    public Text scoreText;
    public float camZPos = 0;
    private float targetSpawnTime = 4f;
    private int lives = 5;
    private float timeSinceLastSpawn = 0;
    private float timeSinceColorChange = 0;
    private int currentScore = 0;
    private int numberOfTargetsInLevel = 4;
    private int targetsCounter = 1;
    private bool isRoundOn = false;
    private int roundCounter = 0;
    private float timeSinceRoundOn = 0;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits;
    private bool isFirstSpawn = true;
    private bool _canScream = true;
    public GameObject arCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        hits = new List<ARRaycastHit>();
        
    }

    public void countdownFinished()
    {

        initializeGame();
    }

    public void score()
    {
        currentScore += 10;
        scoreText.text = "Score: " + currentScore.ToString();
        spawnTarget();
    }

    public void miss()
    {
        lives--;
        if (lives == 0)
        {
           gameOver();
        }
        else
        {
            livesText.text = "Lives: " + lives.ToString();
        }
    }

    private void gameOver()
    {
        timeSinceLastSpawn = 0;
        isGameOver = true;
        gameOVerAudio.Play();
        countDown.textVisibility(true);
        countDown.setText("GAME OVER!\r\n" + "Your Score Is: " + currentScore.ToString());
        rematchButton.gameObject.SetActive(true);
        scoreText.enabled = false;
        livesText.enabled = false;
        dart.enabled = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (isFirstSpawn)
            {
                if (findSurface())
                {
                    isFirstSpawn = false;
                    countDown.run = true;
                }
            }
            else if (isCountDownFinished)
            {
                if(isRoundOn)
                {
                    timeSinceRoundOn += Time.deltaTime;
                    if(timeSinceRoundOn >= 2f)
                    {
                        isRoundOn = false;
                        countDown.textVisibility(false);
                    }
                }
                if (!dart.isDartSwiped())
                {
                    timeSinceLastSpawn += Time.deltaTime;
                }
                if ((timeSinceLastSpawn >= targetSpawnTime) && (!dart.isDartSwiped()))
                {
                    miss();
                    spawnTarget();
                }
                if (targetsCounter >= numberOfTargetsInLevel)
                {
                    changeSpawnRate();
                }
            }
        }
        else
        {
            timeSinceColorChange += Time.deltaTime;
            if(timeSinceColorChange >= 1f)
            {
                timeSinceColorChange = 0;
                changeButtonColor();
            }
        }
    }

    private void changeButtonColor()
    {
        buttonText.material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void spawnTarget()
    {
        target.locateTarget();
        camZPos = arCamera.transform.position.z;
        timeSinceLastSpawn = 0;
        targetsCounter++;
    }

    private void initializeGame()
    {
        gameTexts.SetActive(true);
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: 0";
        dart.createDart();
        target.initializeTarget();
        isFirstSpawn = false;
        isCountDownFinished = true;
        countDown.run = false;
        roundOn();
    }


    public bool findSurface()
    {
        return arRaycastManager.Raycast(arCamera.transform.position, hits, TrackableType.PlaneWithinPolygon);
    }

    public void changeSpawnRate()
    {
        //when spawnRate changes, allow to scream again.
        setScream(true);
        roundOn();
        if (targetSpawnTime > 0.5f)
        {
            targetSpawnTime -= 0.5f;
            targetsCounter = 1;
        }
    }

    private void roundOn()
    {
        timeSinceRoundOn = 0;
        roundCounter++;
        countDown.textVisibility(true);
        countDown.setText("Round " + roundCounter.ToString());
        isRoundOn = true;
    }

    public void rematch()
    {
        SceneManager.LoadScene(0);
    }
    public bool canScream(){
        return !isFirstSpawn && _canScream;
    }
    public void setScream(bool value){
        _canScream = value;
    }
}