using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutscene : MonoBehaviour
{
    float videoDuration = 59;

    void Update()
    {
        videoDuration -= Time.deltaTime;
        if (videoDuration < 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
