using System;
using UnityEngine;

public class InteractWithBed : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f; // Range within which player can interact
    private GameObject player;
    private WorldTime.WorldTime worldTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        worldTime = FindObjectOfType<WorldTime.WorldTime>();
    }

    private void Update()
    {
        if (player == null || worldTime == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            TimeSpan currentTime = worldTime.GetCurrentTime();
            
            if (currentTime.Hours >= 18 || currentTime.Hours < 6)
            {
                worldTime.SkipNight();
                Debug.Log("You slept and woke up at 06:00 AM!");
            }
            else
            {
                Debug.Log("You can only sleep between 18:00 and 06:00.");
            }
        }
    }
}
