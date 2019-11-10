using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class AudioMic : MonoBehaviour
{
    public bool scream;
    public float max = 0;
    public float testSound;
    public static float MicLoudness;
    private string _device;
    private AudioClip _clipRecord/* = new AudioClip()*/;
    private int _sampleWindow = 128;
    private bool _isInitialized;
    public bool canScream;

    void InitMic()
    {
        if (_device == null)
        {
            _device = Microphone.devices[0];
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
            Debug.Log(_clipRecord);
        }
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0)
        {
            return 0;
        }
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            // The user authorized use of the microphone.
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.Microphone);
        }
        scream = false;
        canScream = true;
    }

    // Update is called once per frame
    void Update()
    {
        MicLoudness = LevelMax();
        testSound = MicLoudness*1000;
        if (testSound > max) {
            max = testSound;
        }
        if (testSound < 550)
        {
            return;
        }
        if(canScream)
        {
            scream = true;
            canScream = false;
        }
    }

    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestory()
    {
        StopMicrophone();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }

        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }
}
