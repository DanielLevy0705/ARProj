using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TargetsSpawn : MonoBehaviour
{
    public Text text;

    public GameObject target;
    public GameObject arCamera;
    Vector3 cameraPos;
    float x, y;
    
    public void locateTarget()
    {
        cameraPos = arCamera.transform.position;
        x = Random.Range(cameraPos.x - 0.5f, cameraPos.x + 0.5f);
        y = Random.Range(cameraPos.y - 0.5f, cameraPos.y + 0.5f);
        target.transform.position = new Vector3(x, y, cameraPos.z + 2.5f);
    }

    public void initializeTarget()
    {
        locateTarget();
        target.SetActive(true);
        text.text = target.transform.position.ToString();
    }
}