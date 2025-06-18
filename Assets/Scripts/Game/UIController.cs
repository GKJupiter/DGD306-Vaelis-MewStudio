// Your UIController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Crucial

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject settings;

    // Reference to your first button on the main menu
    [SerializeField] private UnityEngine.UI.Button main_FirstSelectedButton;

    // Reference to the 'Back' button in the Settings panel
    [SerializeField] private UnityEngine.UI.Button settings_BackButton;

    // Reference to the 'Back' button in the Credits panel
    [SerializeField] private UnityEngine.UI.Button credits_BackButton;

    // To remember which button opened the current pop-up
    private GameObject lastSelectedButtonBeforePopup;

    void OnEnable()
    {
        // When the main UI Controller becomes active, ensure the main menu's first button is selected
        // This handles the initial scene load
        if (main_FirstSelectedButton != null && main.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(main_FirstSelectedButton.gameObject);
        }
        // IMPORTANT: If you have a different initial panel active, you might need to select its default button here too.
        // For instance, if 'settings' starts active, you'd want to select settings_BackButton here.
    }

    public void PlayCLicked()
    {
        SceneManager.LoadScene("Opening Cinematic");
    }

    public void SettingsClicked()
    {
        // Store the currently selected button before opening the settings
        // This MUST be done *before* changing panel visibility, as currentSelectedGameObject
        // might become null if the selected object is disabled.
        lastSelectedButtonBeforePopup = EventSystem.current.currentSelectedGameObject;

        settings.SetActive(true);
        main.SetActive(false);

        // Set the initial selection to the Settings Back button
        if (settings_BackButton != null)
        {
            EventSystem.current.SetSelectedGameObject(settings_BackButton.gameObject);
        }
    }

    public void SettingsUnclicked()
    {
        settings.SetActive(false);
        main.SetActive(true);

        // Return selection to the button that was selected before opening settings
        if (lastSelectedButtonBeforePopup != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButtonBeforePopup);
        }
        else if (main_FirstSelectedButton != null) // Fallback if no button was previously selected
        {
            EventSystem.current.SetSelectedGameObject(main_FirstSelectedButton.gameObject);
        }
    }

    public void CreditsClicked()
    {
        // Store the currently selected button before opening the credits
        lastSelectedButtonBeforePopup = EventSystem.current.currentSelectedGameObject;

        credits.SetActive(true);
        main.SetActive(false);

        // Set the initial selection to the Credits Back button
        if (credits_BackButton != null)
        {
            EventSystem.current.SetSelectedGameObject(credits_BackButton.gameObject);
        }
    }

    public void CreditsUnclicked()
    {
        main.SetActive(true);
        credits.SetActive(false);

        // Return selection to the button that was selected before opening credits
        if (lastSelectedButtonBeforePopup != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButtonBeforePopup);
        }
        else if (main_FirstSelectedButton != null) // Fallback if no button was previously selected
        {
            EventSystem.current.SetSelectedGameObject(main_FirstSelectedButton.gameObject);
        }
    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}