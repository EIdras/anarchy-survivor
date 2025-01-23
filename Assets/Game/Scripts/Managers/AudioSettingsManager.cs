using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSoundSource;
    [SerializeField] private bool isMenu;
    
    private SoundManager soundManager;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(SoundManager.MusicVolumeKey, 0.5f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat(SoundManager.EffectsVolumeKey, 0.5f);

        Debug.Log("Music volume: " + musicVolumeSlider.value);
        Debug.Log("Effects volume: " + effectsVolumeSlider.value);
        
        soundManager = SoundManager.Instance;
        if (isMenu)
        {
            soundManager.InitializeAudioSources(null, uiSoundSource, musicSource);
            soundManager.PlayMenuMusic();
        }
        
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    private void SetMusicVolume(float volume)
    {
        Debug.Log("Setting music volume to " + volume);
        soundManager.SetMusicVolume(volume);
    }

    private void SetEffectsVolume(float volume)
    {
        Debug.Log("Setting effects volume to " + volume);
        soundManager.SetEffectsVolume(volume);
    }
    
    public void PlayUISelectSound()
    {
        soundManager.PlayUISelectSound();
    }
}