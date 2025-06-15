using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    float videoDuration = 147;

    void Update()
    {
        videoDuration -= Time.deltaTime;
        if (videoDuration < 0)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
