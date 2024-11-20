using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;  

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;  
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuCanvas.activeSelf)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Stop game time
        
        // Show the cursor when pausing the game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor so it can move freely
    }
    
    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f; 
        
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;  
    }


    public void ReturnToStartMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); 
    }
}
