using System.Collections;
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
    public GameObject dart;
    private GameObject dartInstance;
    public Text tapToPlaceText;
    public Text camPosition;
    float xPos=0,yPos=0;
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if(first){
            CreateDart();
        }
        SwipeDart();
    }
    private void CreateDart(){
            first = false;
            Vector3 pos = new Vector3(arCamera.transform.position.x,arCamera.transform.position.y-0.5f,arCamera.transform.position.z+0.7f);
            //create the dart object.
            dartInstance = Instantiate(dart,pos,Quaternion.identity);
            //change the dart rotation.
            dartInstance.transform.rotation = Quaternion.Euler(190,0,-135);
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
                //*****************************TEST TEXT************************
                tapToPlaceText.text = touchPose.ToString();
                camPosition.text = endTouchPose.ToString();
                //*****************************END OF TEST TEXT*****************
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
            }
            //make the dart move.
            dartInstance.transform.position += new Vector3(xPos,yPos,0.025f);
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
                    firstSwipe = true;
                    swiped = true;
                }
                break;
        }
    }
}