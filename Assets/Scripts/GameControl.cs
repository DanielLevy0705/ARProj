using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public PlacementManager dart;
    public TargetsSpawn target;
    public CountDown countDown;

    private bool isCountDownFinished = false;

    public Text text;

    private float targetSpawnTime = 4f;
    private int lives = 5;
    private float timeSinceLastSpawn = 0;
    private int currentScore = 0;
    private int numberOfTargetsInLevel = 4;
    private int targetsCounter = 1;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits;
    private bool isFirstSpawn = true;
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
        text.text = "no";
    }

    public void countdownFinished()
    {
        
        initializeGame();
    }

    public void score()
    {
        currentScore += 10;
        //text.text = currentScore.ToString();
        spawnTarget();
    }

    public void miss()
    {
        lives--;
    }

    void Update()
    {
        if (isFirstSpawn)
        {
            if (findSurface())
            {
                text.text = "surface located";
                isFirstSpawn = false;
                countDown.run = true;
            }
        }
        else if (isCountDownFinished)
        {
            timeSinceLastSpawn += Time.deltaTime;
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

    private void spawnTarget()
    {
        target.locateTarget();
        timeSinceLastSpawn = 0;
        targetsCounter++;
        text.text = targetsCounter.ToString();
    }

    private void initializeGame()
    {
        dart.createDart();
        target.initializeTarget();
        isFirstSpawn = false;
        isCountDownFinished = true;
        countDown.run = false;
    }


    public bool findSurface()
    {
        return arRaycastManager.Raycast(arCamera.transform.position, hits, TrackableType.PlaneWithinPolygon);
    }

    public void changeSpawnRate()
    {
        text.text = "change spawn rate";
        if (targetSpawnTime > 0.5f)
        {
            targetSpawnTime -= 0.5f;
            targetsCounter = 1;
        }
    }
}