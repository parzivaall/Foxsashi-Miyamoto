using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject playerController;
    public Slider sensitivitySlider;

    public Camera playerCamera;
    private float mouseSensitivity = 1f;
    
    //public FirstPersonCamera mouseLookScript;

    void Start()
    {
        // Ensure menu is hidden at start
        pauseMenuUI.SetActive(false);

        if(playerCamera == null){
            playerCamera = Camera.main;
        }

        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
            sensitivitySlider.value = mouseSensitivity;
        }
        else
        {
            // Default sensitivity
            mouseSensitivity = 1f;
            sensitivitySlider.value = mouseSensitivity;
        }

        
    }

    void Update()
    {
        // Check for Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if(playerController != null){
            playerController.GetComponent<MonoBehaviour>().enabled = true;
        }

        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        if(playerCamera != null && playerCamera.GetComponent<MonoBehaviour>() != null){
            playerCamera.GetComponent<MonoBehaviour>().enabled = true;
        }
    }

     public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    // public void UpdateSensitivity(float newSensitivity)
    // {
    //     mouseSensitivity = sensitivitySlider.value;
    //     PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
    //     PlayerPrefs.Save();

    //     // Update the mouse sensitivity in your MouseLook script
    //     if (mouseLookScript != null)
    //     {
    //         mouseLookScript.mouseSensitivity = mouseSensitivity;
    //     }
    // }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        if(playerController != null){
            playerController.GetComponent<MonoBehaviour>().enabled = false;
        }
            
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        

        if(playerCamera != null && playerCamera.GetComponent<MonoBehaviour>() != null){
            playerCamera.GetComponent<MonoBehaviour>().enabled = false;
        }
            
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}