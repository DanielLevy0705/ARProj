using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TargetsSpawn : MonoBehaviour
{
    public GameObject target;
    public int maxTargets = 20;
    public Text text;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits;
    private bool isFirstSpawn = true;
    public GameObject arCamera;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        hits = new List<ARRaycastHit>();
    }

    void Update()
    {
        if(isFirstSpawn)
        {

            Vector3 cameraPos = arCamera.transform.position;
            if(arRaycastManager.Raycast(cameraPos, hits, TrackableType.PlaneWithinPolygon))
            {
                text.text = "yes";
                TargetsDrop();
                isFirstSpawn = false;
            }
            else
            {
                text.text = "no";
            }
        }
    }

    public void TargetsDrop()
    {
        int targetsCount = 0;
        float x, y;
        Vector3 cameraPos;
        while (targetsCount < maxTargets)
        {
            text.text = arCamera.transform.position.ToString();
            cameraPos = arCamera.transform.position;
            x = Random.Range(cameraPos.x - 0.5f, cameraPos.x + 0.5f);
            y = Random.Range(cameraPos.y - 0.5f, cameraPos.y + 0.5f);
            target.transform.position = new Vector3(x, y, cameraPos.z+2.5f);
            target.SetActive(true);
            targetsCount++;
        }
    }
}
