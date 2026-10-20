using UnityEngine;
using UnityEngine.Audio;


public class SoundManagerMino : BuyableBehavior
{
    private AudioSource _audioSource;
    private static SoundManagerMino instance;

    [SerializeField] private AudioMixer _audioMixer;

    private const string PlayerParam = "Player";
    private const string MusicParam = "Music";
    private const string EnnemiParam = "Ennemies";
    private const string BossParam = "Boss";

    public void SetMusic(float value) => SetVolume(MusicParam, value);
    public void SetPlayer(float value) => SetVolume(PlayerParam, value);
    public void SetBoss(float value) => SetVolume(BossParam, value);
    public void SetEnnemi(float value) => SetVolume(EnnemiParam, value);
    
    

    private void SetVolume(string param, float value)
    {
        // _audioMixer.SetFloat(param, Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f);
        _audioMixer.SetFloat(param,value);
    }

    public static SoundManagerMino Instance
    {
        get
        {
            return instance;
        }
    }

    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if (action.Type != UnitType.Npc) return;

        switch (action.Category)
        {
            case UpgradeCategory.Sound:
                SetMusic(0);
                break;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        SetVolume(BossParam, -80);
        SetVolume(EnnemiParam, -80);
        SetVolume(PlayerParam, -80);
        SetVolume(MusicParam, -80);

        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);


    }


    public void PlaySoundPositional(AudioClip clip, Vector3 position)
    {
        // only for one shots
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayMusic(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        source.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {

    }
}
