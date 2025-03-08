using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the slider UI

    void Start()
    {
        // Ensure the slider starts at the current volume level
        volumeSlider.value = AudioListener.volume;

        // Add a listener to handle the slider change event
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // This method is called when the slider value changes
    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // Adjust the global volume
    }
}
