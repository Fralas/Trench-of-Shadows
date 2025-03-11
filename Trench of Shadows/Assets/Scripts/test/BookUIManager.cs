using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BookUIManager : MonoBehaviour
{
    [SerializeField] private Image bookImage;
    [SerializeField] private Sprite[] bookPages;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;
    [SerializeField] private Sprite lastPageIcon;
    [SerializeField] private Sprite defaultNextPageIcon;
    [SerializeField] private string nextSceneName;

    private int currentPage = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void NextPage()
    {
        if (currentPage < bookPages.Length - 1)
        {
            currentPage++;
            UpdateUI();
        }
        else
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        bookImage.sprite = bookPages[currentPage];

        prevPageButton.interactable = currentPage > 0;

        if (currentPage == bookPages.Length - 1)
        {
            nextPageButton.image.sprite = lastPageIcon;
        }
        else
        {
            nextPageButton.image.sprite = defaultNextPageIcon;
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
