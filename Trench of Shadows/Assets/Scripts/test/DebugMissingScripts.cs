using UnityEngine;

public class DebugMissingScripts : MonoBehaviour
{
    [ContextMenu("Check Missing Scripts")]
    void CheckMissingScripts()
    {
        Component[] components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null)
            {
                Debug.LogWarning("GameObject con script mancante: " + gameObject.name, gameObject);
            }
        }
    }
}
