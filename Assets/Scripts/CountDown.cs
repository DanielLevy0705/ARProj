using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private int counter = 4;
    private float timeoutCount = 0;
    public Text text;
    public GameObject textObject;
    public bool run = false;
    public bool isGo = false;
    public bool isFinished = false;

    void Update()
    {
        if (run)
        {
            timeoutCount += Time.deltaTime;
            if (timeoutCount >= 1f)
            {
                text.text = "after timecount is 1";
                if (counter == 4)
                {
                    text.text = "Game Is About To Start!";
                }
                else if (counter > 0)
                {
                    text.text = counter.ToString(); ;
                }
                else if(isGo)
                {
                    isFinished = true;
                    GameControl.Instance.countdownFinished();
                }
                else
                {
                    text.text = "GO!";
                    isGo = true;
                }
                timeoutCount = 0;
                counter--;
            }
            if (isFinished)
            {
                textObject.SetActive(false);
            }
        }
    }
}
