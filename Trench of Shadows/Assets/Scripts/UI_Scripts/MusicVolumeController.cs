using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI slider
    public AudioSource musicSource; // Reference to the AudioSource

    void Start()
    {
        // Ensure slider value matches current volume at start
        if (musicSource != null && volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume; // Set AudioSource volume
        }
    }
}
