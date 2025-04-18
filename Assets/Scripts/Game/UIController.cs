using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject credits;

    public void PlayCLicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingsClicked()
    {
        settings.SetActive(true);
        main.SetActive(false);
    }

        public void SettingsUnclicked()
    {
        settings.SetActive(false);
        main.SetActive(true);
    }

    public void CreditsClicked()
    {
        credits.SetActive(true);
        main.SetActive(false);
    }

    public void CreditsUnclicked()
    {
        main.SetActive(true);
        credits.SetActive(false);
    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}