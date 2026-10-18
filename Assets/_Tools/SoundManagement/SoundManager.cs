using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("SoundManager");
                _instance = obj.AddComponent<SoundManager>();
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    public int GeneralSoundVolume { get; private set; } = 100;
    public int MusicVolume { get; private set; } = 100;
    public int SfxVolume { get; private set; } = 100;
    public int VoiceVolume { get; private set; } = 100;
    public int AmbientVolume { get; private set; } = 100;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GeneralSoundVolume = PlayerPrefs.GetInt("GeneralSoundVolume", 100);
        MusicVolume = PlayerPrefs.GetInt("MusicVolume", 100);
        SfxVolume = PlayerPrefs.GetInt("SfxVolume", 100);
        VoiceVolume = PlayerPrefs.GetInt("VoiceVolume", 100);
        AmbientVolume = PlayerPrefs.GetInt("AmbientVolume", 100);

        PlayerPrefs.SetInt("GeneralSoundVolume", GeneralSoundVolume);
        PlayerPrefs.SetInt("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("SfxVolume", SfxVolume);
        PlayerPrefs.SetInt("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetInt("AmbientVolume", AmbientVolume);
    }

    public void SetVolume(Slider slider)
    {
        switch (slider.name)
        {
            case "GSSlider":
                GeneralSoundVolume = (int)slider.value;
                PlayerPrefs.SetInt("GeneralSoundVolume", GeneralSoundVolume);
                break;
            case "MSlider":
                MusicVolume = (int)slider.value;
                PlayerPrefs.SetInt("MusicVolume", MusicVolume);
                break;
            case "SFXSlider":
                SfxVolume = (int)slider.value;
                PlayerPrefs.SetInt("SfxVolume", SfxVolume);
                break;
            case "VLSlider":
                VoiceVolume = (int)slider.value;
                PlayerPrefs.SetInt("VoiceVolume", VoiceVolume);
                break;
            case "ASSlider":
                AmbientVolume = (int)slider.value;
                PlayerPrefs.SetInt("AmbientVolume", AmbientVolume);
                break;
            default:
                Debug.LogWarning("Unknown slider name: " + slider.name);
                break;
        }
    }
}
