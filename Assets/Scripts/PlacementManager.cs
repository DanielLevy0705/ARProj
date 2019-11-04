using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementManager : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private ARSessionOrigin aRSessionOrigin;
    private Touch touch;
    private Vector2 touchPose,endTouchPose;
    private bool first = true;
    public GameObject dart;
    public GameObject tapToPlaceText;
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
        aRSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    void Update()
    {
        if(first){
            if(Input.touchCount > 0){
                first = false;
                touch = Input.GetTouch(0);
                touchPose = touch.position;
                tapToPlaceText.SetActive(false);
                if(!dart.activeInHierarchy){
                    dart.SetActive(true);
                }
                Vector3 pose = new Vector3(aRSessionOrigin.transform.position.x + touchPose.x,
                aRSessionOrigin.transform.position.y+touchPose.y,aRSessionOrigin.transform.position.z+1);
                aRSessionOrigin.MakeContentAppearAt(dart.transform,pose,Quaternion.identity);
                
            }
        }
    }
}
