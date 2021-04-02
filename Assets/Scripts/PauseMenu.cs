using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused;

    public GameObject PauseMenuUI;
    [SerializeField] GameObject InformatioScreen;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void ShowStats() {
        InformatioScreen.SetActive(true);
    }

    public void HideStats() {
        InformatioScreen.SetActive(false);
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause() {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }
}