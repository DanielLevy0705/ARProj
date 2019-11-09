using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class CountDown : MonoBehaviour
{
    private int counter = 4;
    private float timeoutCount = 0;
    public Text text;
    public GameObject textObject;
    public bool run = false;
    public bool isGo = false;
    public bool isFinished = false;
    public bool isFirst = false;

    void Update()
    {
        if (run)
        {
            if(isFirst)
            {
                text.fontSize = 130;
            }
            timeoutCount += Time.deltaTime;
            if (timeoutCount >= 1f)
            {
                if (counter == 4)
                {
                    setText("Game Is About To Start!");
                }
                else if (counter > 0)
                {
                    setText(counter.ToString());
                }
                else if(isGo)
                {
                    isFinished = true;
                    GameControl.Instance.countdownFinished();
                }
                else
                {
                    setText("GO!");
                    isGo = true;
                }
                timeoutCount = 0;
                counter--;
            }
            if (isFinished)
            {
                text.fontSize = 80;
                textVisibility(false);
            }
        }
    }

    public void randomColor()
    {
        text.material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    internal void textVisibility(bool v)
    {
        textObject.SetActive(v);
    }

    internal void setText(string v)
    {
        randomColor();
        text.text = v;
    }
}
