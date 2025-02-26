using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject homeUI;
    public GameObject settingsUI;
    public GameObject keybindsUI;
    public GameObject savesUI;
    public Button playBox;
    public Button exitBox;
    public Button settingsButton;
    public Button returnButton;
    public Button keyBindBox;
    public Button keybindsReturnButton;
    public Button savesButton;
    public Button savesReturnButton;

    [SerializeField] private string targetSceneName; // Set this in Inspector

    void Start()
    {
        playBox.onClick.AddListener(PlayGame);
        exitBox.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(OpenSettings);
        returnButton.onClick.AddListener(CloseSettings);
        keyBindBox.onClick.AddListener(OpenKeybinds);
        keybindsReturnButton.onClick.AddListener(CloseKeybinds);
        savesButton.onClick.AddListener(OpenSaves);
        savesReturnButton.onClick.AddListener(CloseSaves);
    }

    void PlayGame()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Loading scene: " + targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("❌ No scene name assigned! Set targetSceneName in Inspector.");
        }
    }

    void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    void OpenSettings()
    {
        homeUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    void CloseSettings()
    {
        settingsUI.SetActive(false);
        homeUI.SetActive(true);
    }

    void OpenKeybinds()
    {
        settingsUI.SetActive(false);
        keybindsUI.SetActive(true);
    }

    void CloseKeybinds()
    {
        keybindsUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    void OpenSaves()
    {
        homeUI.SetActive(false);
        savesUI.SetActive(true);
    }

    void CloseSaves()
    {
        savesUI.SetActive(false);
        homeUI.SetActive(true);
    }
}
