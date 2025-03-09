using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables; // Per controllare la Timeline

public class TransparencyController : MonoBehaviour
{
    [Range(0f, 1f)] public float transparency = 1f;
    public PlayableDirector timeline; // Riferimento alla Timeline

    private void UpdateTransparency()
    {
        if (timeline != null && timeline.state == PlayState.Playing)
            return; // Evita di sovrascrivere l'animazione

        Image[] images = GetComponentsInChildren<Image>(true);
        foreach (Image img in images)
        {
            Color color = img.color;
            color.a = transparency;
            img.color = color;
        }

        Text[] texts = GetComponentsInChildren<Text>(true);
        foreach (Text txt in texts)
        {
            Color color = txt.color;
            color.a = transparency;
            txt.color = color;
        }

        TMP_Text[] tmpTexts = GetComponentsInChildren<TMP_Text>(true);
        foreach (TMP_Text tmp in tmpTexts)
        {
            Color color = tmp.color;
            color.a = transparency;
            tmp.color = color;
        }
    }

    private void Update()
    {
        UpdateTransparency();
    }
}
