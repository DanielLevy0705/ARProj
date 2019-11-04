using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMic : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource src;
    public float max = 0;
    public float testSound;
    public static float MicLoudness;
    private string _device;
    private AudioClip _clipRecord/* = new AudioClip()*/;
    private int _sampleWindow = 128;
    private bool _isInitialized;
    public AudioSource Pingsrc;

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
        src.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        MicLoudness = LevelMax();
        testSound = MicLoudness*1000;
        if (testSound > max) {
            max = testSound;
        }
        if (testSound < 250)
        {
            return;
        }
        var paddle1 = GameObject.Find("PADDLE");
        var paddle2 = GameObject.Find("PADDLE (1)");
        var sphere = GameObject.Find("Sphere");

        var tran = paddle1.GetComponent<Transform>();
        var tran1 = paddle2.GetComponent<Transform>();
        var vel = sphere.GetComponent<Rigidbody>().velocity;
        var z = sphere.GetComponent<Rigidbody>().velocity.z;
        var spherePos = sphere.GetComponent<Transform>().position;
        if (z < 0 && spherePos.z-tran.position.z < 5 && spherePos.z - tran.position.z > 0)
        {
            paddle1.GetComponent<Transform>().position = new Vector3
                (spherePos.x, spherePos.y, paddle1.GetComponent<Transform>().position.z);
            sphere.GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, -vel.z);
            Pingsrc.Play();
        }
        else
        {
            if (tran1.position.z - spherePos.z < 5 && tran1.position.z - spherePos.z >0)
            {
                paddle2.GetComponent<Transform>().position = new Vector3
                    (spherePos.x, spherePos.y, paddle2.GetComponent<Transform>().position.z);
                sphere.GetComponent<Rigidbody>().velocity = new Vector3(vel.x, vel.y, -1*vel.z);
                Pingsrc.Play();
            }
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
