using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    [Header("Camera Settings")]
    public Slider smoothingSlider;
    private Camera playerCamera;
    public float cameraSmoothing = 0.4f;
    private CameraScript cameraScript;

    [Header("Audio Settings")]
    public Slider volumeSlider;
    public float volume = 1f;

    [Header("Difficulty Settings")]
    public Toggle hardToggle;
    public bool hard = false;

    void Start()
    {
        // Ensure menu is hidden at start
        pauseMenuUI.SetActive(false);

        if(playerCamera == null){
            playerCamera = Camera.main;
            cameraScript = playerCamera.GetComponent<CameraScript>();
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

        if (PlayerPrefs.HasKey("Hard"))
        {
            hard = PlayerPrefs.GetInt("Hard") == 1;
            hardToggle.isOn = hard;
            EnvManager.Instance.setHard(hard);
        }
        else
        {
            hard = false;
            hardToggle.isOn = hard;
            EnvManager.Instance.setHard(hard);
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
            
    }

    public void ToggleHardMode()
    {
        hard = !hard;
        EnvManager.Instance.setHard(hard);
        // Store the value of hard in the player prefs as an int, where true = 1 and false = 0
        PlayerPrefs.SetInt("Hard", hard ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void GameOver(){
        EnvManager.Instance.setHealth(-100);
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