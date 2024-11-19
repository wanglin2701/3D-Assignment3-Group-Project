using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Debug.Log("Back button clicked.");
        SceneManager.LoadScene("MainMenu");
    }
}
