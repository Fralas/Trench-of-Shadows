using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI; // Assign your PauseUI in the Inspector
    public GameObject settingsUI; // Assign your SettingsUI in the Inspector
    public GameObject commandsUI; // Assign your CommandsUI in the Inspector
    public Button playBox; // Assign the PlayBox button in the Inspector
    public Button exitBox; // Assign the ExitBox button in the Inspector
    public Button settingsButton; // Assign the Settings button in the Inspector
    public Button returnButton; // Assign the Return button in the Inspector
    public Button keyBindBox; // Assign the KeyBindBox button in the Inspector
    public Button commandsReturnButton; // Assign the Return button in CommandsUI
    private bool isPaused = false;

    void Start()
    {
        playBox.onClick.AddListener(ResumeGame);
        exitBox.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(OpenSettings);
        returnButton.onClick.AddListener(CloseSettings);
        keyBindBox.onClick.AddListener(OpenCommands);
        commandsReturnButton.onClick.AddListener(CloseCommands);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (commandsUI.activeSelf)
            {
                CloseCommands();
            }
            else if (settingsUI.activeSelf)
            {
                CloseSettings();
            }
            else if (pauseUI.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                TogglePause();
            }
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void ResumeGame()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        settingsUI.SetActive(false);
        commandsUI.SetActive(false);
        Time.timeScale = 1f;
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
        pauseUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    void CloseSettings()
    {
        settingsUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    void OpenCommands()
    {
        settingsUI.SetActive(false);
        commandsUI.SetActive(true);
    }

    void CloseCommands()
    {
        commandsUI.SetActive(false);
        settingsUI.SetActive(true);
    }
}