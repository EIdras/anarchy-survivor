using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private AudioSource playerEffectsSource;
    private AudioSource effectsSource;
    private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip UISelectClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField] private AudioClip shieldBrokenClip;
    [SerializeField] private AudioClip grenadeExplosionClip;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip menuMusic;

    public const string MusicVolumeKey = "MusicVolume";
    public const string EffectsVolumeKey = "EffectsVolume";
    private float musicVolume;
    private float effectsVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadSettings();
    }
    
    public void InitializeAudioSources(AudioSource playerEffects, AudioSource effects, AudioSource music)
    {
        Debug.Log("Initializing audio sources : PLAYER=" + playerEffects + "| EFFECTS=" + effects + "| MUSIC" + music);
        playerEffectsSource = playerEffects;
        effectsSource = effects;
        musicSource = music;
        LoadVolumes(playerEffectsSource != null, effectsSource != null, musicSource != null);
    }

    private void PlayPlayerSound(AudioClip clip)
    {
        playerEffectsSource.PlayOneShot(clip);
    }

    private void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }
    
    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }
    
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }


    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlayUISelectSound()
    {
        PlaySound(UISelectClip);
    }

    public void PlayShootSound()
    {
        PlayPlayerSound(shootClip);
    }

    public void PlayEnemyHitSound()
    {
        PlaySound(enemyHitClip);
    }
    
    public void PlayPlayerHitSound()
    {
        PlayPlayerSound(playerHitClip);
    }
    
    public void PlayPlayerShieldBrokenSound()
    {
        PlayPlayerSound(shieldBrokenClip);
    }

    public void PlayEnemyDeathSound()
    {
        PlaySound(enemyDeathClip);
    }

    public void PlayPlayerDeathSound()
    {
        PlayPlayerSound(playerDeathClip);
    }

    public void PlayGrenadeExplosionSound()
    {
        PlaySound(grenadeExplosionClip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
        PlayerPrefs.SetFloat(EffectsVolumeKey, volume);
    }

    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        effectsVolume = PlayerPrefs.GetFloat(EffectsVolumeKey, 0.5f);
    }
    
    private void LoadVolumes(bool hasPlayerEffectsSource, bool hasEffectsSource, bool hasMusicSource)
    {
        LoadSettings();
        if (hasPlayerEffectsSource)
        {
            playerEffectsSource.volume = effectsVolume;
        }
        if (hasEffectsSource)
        {
            effectsSource.volume = effectsVolume;
        }
        if (hasMusicSource)
        {
            musicSource.volume = musicVolume;
        }
    }
}