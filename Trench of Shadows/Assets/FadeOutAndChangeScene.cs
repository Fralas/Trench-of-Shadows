using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeOutAndChangeScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;  
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private string nextSceneName;

    private void Awake()
    {
        if (uiCanvasGroup == null)
            uiCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeOut()
    {
        if (uiCanvasGroup != null)
            StartCoroutine(FadeOutAndLoadScene());
        //else
            //Debug.LogError("CanvasGroup non assegnato a " + gameObject.name);
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            uiCanvasGroup.alpha = 1 - (timer / fadeDuration);
            yield return null;
        }

        uiCanvasGroup.alpha = 0;
        SceneManager.LoadScene(nextSceneName);
    }
}
