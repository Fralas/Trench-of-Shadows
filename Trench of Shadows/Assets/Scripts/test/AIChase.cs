using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    private float distance;


    void Start()
    {
        // Se il player non è assegnato manualmente, prova a trovarlo
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        // Se ancora non trova il Player, prova a riassegnarlo una volta
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            // Se ancora non lo trova, interrompi il codice
            if (player == null)
            {
                return;
            }
        }

        // Segue il player solo se esiste
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
