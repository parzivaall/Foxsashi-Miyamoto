using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject playerController;

    [Header("Camera Settings")]
    public Slider smoothingSlider;
    public Camera playerCamera;
    public float cameraSmoothing = 0.4f;
    public CameraScript cameraScript;

    [Header("Audio Settings")]
    public Slider volumeSlider;
    public float volume = 1f;

    void Start()
    {
        // Ensure menu is hidden at start
        pauseMenuUI.SetActive(false);

        if(playerCamera == null){
            playerCamera = Camera.main;
        }

        if (PlayerPrefs.HasKey("CameraSmoothing"))
        {
            cameraSmoothing = PlayerPrefs.GetFloat("CameraSmoothing");
            smoothingSlider.value = cameraSmoothing;
            cameraScript.smoothTime = cameraSmoothing;
        }
        else
        {
            // Default sensitivity
            cameraSmoothing = 0.4f;
            smoothingSlider.value = cameraSmoothing;
            cameraScript.smoothTime = cameraSmoothing;
        }

        if (PlayerPrefs.HasKey("Volume"))
        {
            volume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = volume;
            EnvManager.Instance.setVolume(volume);
        }
        else
        {
            // Default sensitivity
            volume = 1f;
            volumeSlider.value = volume;
            EnvManager.Instance.setVolume(volume);
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

        if (EnvManager.Instance.getVolume() != volume){
            EnvManager.Instance.setVolume(volume);
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
    //     mouseSensitivity = smoothingSlider.value;
    //     PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
    //     PlayerPrefs.Save();

    //     // Update the mouse sensitivity in your MouseLook script
    //     if (mouseLookScript != null)
    //     {
    //         mouseLookScript.mouseSensitivity = mouseSensitivity;
    //     }
    // }
    public void UpdateVolume(){
        volume = volumeSlider.value;
        EnvManager.Instance.setVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void UpdateCameraSmoothing(){
        cameraSmoothing = smoothingSlider.value;
        PlayerPrefs.SetFloat("CameraSmoothing", cameraSmoothing);
        PlayerPrefs.Save();

        if (cameraScript != null){
            cameraScript.smoothTime = cameraSmoothing;
        }
    }

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