using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private GameObject CanvasCredits;
    [SerializeField] private GameObject CanvasHowToPlay;

    private void Awake() {
        CanvasCredits = GetComponent<Canvas>().gameObject;
        CanvasHowToPlay = GetComponent<Canvas>().gameObject;
    }


    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowCredits() {
        CanvasCredits.SetActive(true);
    }

    public void HideCredits() {
        CanvasCredits.SetActive(false);
    }

    public void ShowOptions() {
        CanvasHowToPlay.SetActive(true);
    }

    public void HideOptions() {
        CanvasHowToPlay.SetActive(false);
    }

    public void QuiteGame() {
        Application.Quit();
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}