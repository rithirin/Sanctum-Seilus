
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PPScript : UdonSharpBehaviour
{
    private Animator _ppAnimator;
    [Header("PostProcessing Controller")]
    [Header("Current Hue and saturation")]
    public float hue;
    public float saturation;
    
    [Header("Slider hue and Saturation")]
    [Tooltip("Hue Slider UdonBehavoir")]
    public UdonBehaviour otherHue;
    [Tooltip("Saturation Slider UdonBehavoir")]
    public UdonBehaviour otherSaturation;
    
    [Header("Follow Cube position")]
    public Transform followCube;
    [Tooltip("Hue Slider Handle")]
    public Transform z;
    [Tooltip("Saturation Slider Handle")]
    public Transform y;
    
    [Header("AudioLink")]
    [Tooltip("Audio Link Script")]
    public UdonBehaviour audioLink;
    public int _bandHue;
    public int _bandHue2;
    public int _bandSaturation;
    [Range(0, 127)]
    public int delay;
    public bool toggleAudioLink;
    [Header("Amplitudes")]
    public float _amplitudeHue;
    public float _amplitudeSaturation;
    [Tooltip("Multiplys the power of the amlitudes")]
    public float amplitudePower;
    public float minSaturation;

    private int _dataIndexHue;
    private int _dataIndexHue2;

    private int _dataIndexSaturation;

    public bool debugMode;
    

    void Start()
    {
        _dataIndexHue = (_bandHue * 128) + delay; // sets the Data index for audio link
        _dataIndexHue2 = (_bandHue2 * 128) + delay; // sets the Data index for audio link

        _dataIndexSaturation = (_bandSaturation * 128) + delay; // sets the Data index for audio link
        
        hue = 0;
        saturation = 0;
        _ppAnimator = gameObject.GetComponent<Animator>(); // default animator stuff
        _ppAnimator.SetFloat("Hue", hue);
        _ppAnimator.SetFloat("Saturation", saturation);
        if (debugMode == true) Debug.Log("PPScript StartUpCompleat");
    }

    void Update()
    {
        if (!toggleAudioLink) return;
        Color[] audioData = (Color[])audioLink.GetProgramVariable("audioData");
        if(audioData.Length != 0)       // check for audioLink initialization
        {
            _amplitudeHue = audioData[_dataIndexHue].grayscale + audioData[_dataIndexHue2].grayscale / 2 * amplitudePower;
            _amplitudeSaturation = audioData[_dataIndexSaturation].grayscale * amplitudePower + minSaturation;
            SetColorsAudioLink();
        }
    }

    public void SetColors()
    {
        if (toggleAudioLink) return;
        if (debugMode == true) Debug.Log("PPScript Colors Set");
        hue = (float) otherHue.GetProgramVariable("currentState") * 3;
        saturation = (float) otherSaturation.GetProgramVariable("currentState") * 3;
        _ppAnimator.SetFloat("Hue", hue);
        _ppAnimator.SetFloat("Saturation", saturation);
        
        //sets follower cube
        followCube.position = new Vector3(followCube.position.x, y.position.y, z.position.z );
    }

    public void SetColorsAudioLink()
    {
        _ppAnimator.SetFloat("Hue", _amplitudeHue);
        _ppAnimator.SetFloat("Saturation", _amplitudeSaturation);
    }

    public void ToggleAudioLink()
    {
        if (!toggleAudioLink)
        {
            toggleAudioLink = true;
        }
        else
        {
            toggleAudioLink = false;
            SetColors();
        }
    }
}
