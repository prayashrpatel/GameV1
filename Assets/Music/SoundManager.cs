using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] Slider musicSlider;

    [Header("---------- Audio Clip ----------")]
    public AudioClip Intro;

    private void Start()
    {
        if (musicSource == null || musicSlider == null)
        {
            Debug.LogError("AudioSource or Slider is not assigned in the inspector.");
            return;
        }

        // Set the music clip and play it
        musicSource.clip = Intro;
        musicSource.Play();

        // Load saved volume settings
        Load();

        // Add listener to the slider to handle volume change
        musicSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    public void ChangeVolume()
    {
        // Update the volume based on the slider's value
        // Assuming the slider value range is from 0 to 10
        musicSource.volume = musicSlider.value / 10;
        SFXSource.volume = musicSlider.value / 10; // Apply the same volume to SFX if needed
        Save();
    }

    private void Load()
    {
        // Load the saved volume or set it to default (5) if not set
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 5);
        }

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        musicSource.volume = musicSlider.value / 10;
        SFXSource.volume = musicSlider.value / 10;
    }

    private void Save()
    {
        // Save the current volume setting
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }
}