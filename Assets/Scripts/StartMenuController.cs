using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame button clicked.");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlayGame()
    {
        Debug.Log("StartGame button clicked.");
        SceneManager.LoadScene("HowToPlay");
    }
}
