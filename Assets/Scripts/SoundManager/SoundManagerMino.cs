using UnityEngine;

public class SoundManagerMino : MonoBehaviour
{
    private AudioSource _audioSource;
    private static SoundManagerMino instance;
    
    
    public static SoundManagerMino Instance
    {
        get
        {
            return instance;
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
    }

    public void PlaySoundPositional(AudioClip clip, Vector3 position)
    {
        // only for one shots
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayMusic(AudioClip clip)
    {
        _audioSource.Play();
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
