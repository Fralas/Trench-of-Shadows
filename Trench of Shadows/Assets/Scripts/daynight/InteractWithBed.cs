using System;
using UnityEngine;

public class InteractWithBed : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f; // Range within which player can interact
    [SerializeField] private GameObject cannotSleepMessage; // Object shown when sleeping is not allowed
    [SerializeField] private GameObject sleepPromptObject; // Physical GameObject that appears when in range

    private GameObject player;
    private WorldTime.WorldTime worldTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        worldTime = FindObjectOfType<WorldTime.WorldTime>();

        if (cannotSleepMessage != null) cannotSleepMessage.SetActive(false);
        if (sleepPromptObject != null) sleepPromptObject.SetActive(false); // Start hidden
    }

    private void Update()
    {
        if (player == null || worldTime == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance <= interactRange)
        {
            if (sleepPromptObject != null && !sleepPromptObject.activeSelf)
            {
                sleepPromptObject.SetActive(true); // Show prompt when near
            }

            if (Input.GetKeyDown(KeyCode.E))
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
                    ShowCannotSleepMessage();
                }
            }
        }
        else
        {
            if (sleepPromptObject != null && sleepPromptObject.activeSelf)
            {
                sleepPromptObject.SetActive(false); // Hide prompt when out of range
            }
        }
    }

    private void ShowCannotSleepMessage()
    {
        if (cannotSleepMessage != null)
        {
            cannotSleepMessage.SetActive(true);
            Invoke(nameof(HideCannotSleepMessage), 2f); // Hide after 2 seconds
        }
    }

    private void HideCannotSleepMessage()
    {
        if (cannotSleepMessage != null)
        {
            cannotSleepMessage.SetActive(false);
        }
    }
}
