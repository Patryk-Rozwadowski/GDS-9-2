using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject CanvasCredits;
    [SerializeField] GameObject CanvasOptions;

    private void Awake()
    {
        CanvasCredits = GetComponent<Canvas>().gameObject;
        CanvasOptions = GetComponent<Canvas>().gameObject;
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowCredits()
    {
        CanvasCredits.SetActive(true);
    }

    public void HideCredits()
    {
        CanvasCredits.SetActive(false);
    }

    public void ShowOptions()
    {
        CanvasOptions.SetActive(true);
    }

    public void HideOptions()
    {
        CanvasOptions.SetActive(false);
    }

    public void QuiteGame()
    {
        Application.Quit();
    }


}
