using UnityEngine;

public class Cartello : MonoBehaviour
{
    public GameObject targetObject; // Assegna l'oggetto da controllare nell'Inspector

    void Update()
    {
        if (targetObject != null)
        {
            if (targetObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("cartello non disp");
        }
    }
}
