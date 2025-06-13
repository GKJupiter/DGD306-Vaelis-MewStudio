using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAreaScript : MonoBehaviour
{
    public GameObject tutorialPopup; // UI Canvas or Text

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialPopup.SetActive(false);
        }
    }
}
