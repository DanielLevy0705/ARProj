﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementManager : MonoBehaviour
{
    private ARRaycastManager rayManager;
    public Camera arCamera;
    private Touch touch;
    private Vector2 touchPose,endTouchPose;
    private bool first = true;
    private bool firstSwipe = false;
    private bool swiped = false;
    private bool gotPosition = false;
    private int numOfFrames = 0;
    public GameObject dart;
    private GameObject dartInstance;
    private CollisionDetector dartFront;
    private Vector3 pos;
    private float camZpos;
    private float xPos=0,yPos=0,zPos = 0.025f,xRot = 190,yRot =0,zRot = -135;
    private float rotChange = 0;

    public AudioSource throwSound;
    public AudioSource hitSound;
    public AudioSource missSound;

    void Start()
    {
    }

    void Update()
    {
        pos = new Vector3(arCamera.transform.position.x,arCamera.transform.position.y-0.5f,arCamera.transform.position.z+0.7f);
        if(!first){
            SwipeDart();
        }
        if(!swiped){
            dartInstance.transform.position = pos;
        }
        if(dartFront.collided){
            OnFrontCollision();
            dartFront.collided = false;
        }
    }
    public void createDart(){
            first = false;
            //create the dart object.
            dartInstance = Instantiate(dart,pos,Quaternion.identity);
            //get the dart Front.
            dartFront = dartInstance.transform.GetChild(1).gameObject.GetComponent<CollisionDetector>();
            //change the dart rotation.
            dartInstance.transform.rotation = Quaternion.Euler(xRot,yRot,zRot);
    }
    private void SwipeDart(){
        if(Input.touchCount>0){
            if(!firstSwipe){
                GetSwipe();
            }
        }
        if(swiped){
            //normalize the arrow position.
            if(!gotPosition){
                gotPosition = true;
                //get distance between swipe start and end.
                xPos = Mathf.Sqrt((touchPose.x-endTouchPose.x)*(touchPose.x-endTouchPose.x));
                if(touchPose.x > endTouchPose.x){
                    xPos = -xPos;
                }
                yPos = Mathf.Sqrt((touchPose.y-endTouchPose.y)*(touchPose.y-endTouchPose.y));
                if(touchPose.y > endTouchPose.y){
                    yPos = -yPos;
                }
                //normalize x and y.
                xPos = xPos/30000f;
                yPos = yPos/40000f;
                numOfFrames = 0;
                camZpos = GameControl.Instance.camZPos;
            }
            //make the dart move.
            if(dartInstance.transform.position.z < camZpos+2.55f){
                dartInstance.transform.position += new Vector3(xPos,yPos,zPos);
                //get to 180 degrees rotation.
                if(numOfFrames<74){
                    rotChange+= (10/74);
                    numOfFrames++;
                    dartInstance.transform.rotation = Quaternion.Euler(xRot-rotChange,yRot,zRot);
                }
            }else{
                missSound.Play();
                GameControl.Instance.miss();
                initDart();
            } 
        }
    }
    private void GetSwipe(){
        
        touch = Input.GetTouch(0);
        switch(touch.phase){
            case TouchPhase.Began:
                touchPose = touch.position;
                break;
            case TouchPhase.Ended:
                endTouchPose = touch.position;
                if(touchPose != endTouchPose){
                    throwSound.Play();
                    firstSwipe = true;
                    swiped = true;
                }
                break;
        }
    }
    public bool isDartSwiped(){
        return swiped;
    }
    private void initDart(){
        dartInstance.transform.position = pos;
        dartInstance.transform.rotation = Quaternion.Euler(xRot,yRot,zRot);
        firstSwipe = false;
        swiped = false;
        gotPosition = false;
    }
    private void OnFrontCollision(){
        hitSound.Play();
        GameControl.Instance.score();
        initDart();
    }
}