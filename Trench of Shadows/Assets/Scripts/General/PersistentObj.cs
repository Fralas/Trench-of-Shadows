using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObj : MonoBehaviour
{
    public static PersistentObj Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)  
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene il player tra le scene
        }
        else
        {
            Destroy(gameObject); // Evita duplicati nei nuovi livelli
        }
    }
}
