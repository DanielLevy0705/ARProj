using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

//uncomment comments to check targets!
public class AudioMic : MonoBehaviour
{
    public Text text;
    public float max = 0;
    public GameObject[] targets;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;
    public GameObject target5;
    public PlacementManager dart;
    public float testSound;
    public static float MicLoudness;
    private string _device;
    private float timeSinceScream = 0;
    private float screamTime = 0.5f;
    private AudioClip _clipRecord/* = new AudioClip()*/;
    private int _sampleWindow = 128;
    private bool _isInitialized;
    //private int index = 0;
    private bool screamed = false;
    private Vector3 newSize = new Vector3(2.5f,2.5f,1.8f);
    private Vector3 originalSize = new Vector3(1.8f,1.8f,1.8f);

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
        int micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1);
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
        targets = new GameObject[5];
        targets[0] = target1;
        targets[1] = target2;
        targets[2] = target3;
        targets[3] = target4;
        targets[4] = target5;
    }

    // Update is called once per frame
    void Update()
    {
        //count time since scream.
        if(screamed){
            timeSinceScream += Time.deltaTime;
        }
        //if time passed and dart is not in mid swipe
        if(timeSinceScream>screamTime && !dart.isDartSwiped()){
            //set screamed,scale and timeSinceScream to original value.
            screamed = false;
            // switch(index){
            //     case 0:
            //         target1.transform.localScale = originalSize;
            //         break;
            //     case 1:
            //         target2.transform.localScale = originalSize;
            //         break;
            //     case 2:
            //         target3.transform.localScale = originalSize;
            //         break;
            //     case 3:
            //         target4.transform.localScale = originalSize;
            //         break;
            //     case 4:
            //         target5.transform.localScale = originalSize;
            //         break;
            // }
            timeSinceScream = 0;
        }
        //get microphone input.
        MicLoudness = LevelMax();
        testSound = MicLoudness*1000;
        if (testSound > max) {
            max = testSound;
        }
        //if its not loud enough return.
        if (testSound < 200)
        {
            return;
        }
        text.text = testSound.ToString();
        // if(GameControl.Instance.canScream() && dart.isDartSwiped()){
        //     GameControl.Instance.setScream(false);
        //     screamed = true;
        //     timeSinceScream = 0;
        //     index = GameControl.Instance.target.targetIndex();
        //     switch(index){
        //         case 0:
        //             target1.transform.localScale = newSize;
        //             break;
        //         case 1:
        //             target2.transform.localScale = newSize;
        //             break;
        //         case 2:
        //             target3.transform.localScale = newSize;
        //             break;
        //         case 3:
        //             target4.transform.localScale = newSize;
        //             break;
        //         case 4:
        //             target5.transform.localScale = newSize;
        //             break;
        //     }
        //}
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
