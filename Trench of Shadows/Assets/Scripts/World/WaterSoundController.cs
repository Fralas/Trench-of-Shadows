using UnityEngine;

public class WaterSoundController : MonoBehaviour
{
    public AudioSource waterAudioSource; 
    public Transform player; 
    public float maxDistance = 10f; 
    public float minDistance = 1f; 

    void Update()
    {
        AdjustVolumeBasedOnDistance();
    }

    void AdjustVolumeBasedOnDistance()
    {
        // Calculate the distance between the player and the water tile
        float distance = Vector3.Distance(player.position, transform.position);
        
        // Map the distance to a volume level (0.0f to 1.0f)
        float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));

        // Apply the volume to the AudioSource
        waterAudioSource.volume = volume;
    }
}

