using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvManager : MonoBehaviour
{
    public static EnvManager Instance;
    private int score = 0;
    private int maxHealth = 100;
    private int health;
    public AudioSource damageSFX;
    public AudioSource gameOverSFX;
    private string currentSceneName;
    public TextAnimator textAnimator;
    public MusicOrganizer musicOrganizer;
    public List<int> highscores = new List<int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentSceneName = SceneManager.GetActiveScene().name;

        // Play the music for the initial scene
        musicOrganizer.PlaySceneMusic(currentSceneName);

        // Subscribe to scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;

        resetHealth();

        for (int i = 0; i < 3; i++){
            if (PlayerPrefs.HasKey("Highscore: " + (i+1))){
                highscores.Add(PlayerPrefs.GetInt("Highscore: " + (i+1), 0));
            } else{
                highscores.Add(0);
            }
        }
    }

    void start(){
        resetHealth();
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene change event when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene has changed
        if (scene.name != currentSceneName)
        {
            // Update current scene name
            currentSceneName = scene.name;

            // Play the appropriate music for the new scene
            musicOrganizer.PlaySceneMusic(currentSceneName);
        }
    }

    public void setVolume(float volume){
        damageSFX.volume = volume;
        gameOverSFX.volume = volume;
        musicOrganizer.setVolume(volume);
    }

    public void setHealth(int damage)
    {
        health += damage;
        textAnimator.AnimateAlert("Health: " + health);
        damageSFX.Play();
        if (health <= 0) { 
            gameOverSFX.Play();
            if (score > highscores[0])
            {
                highscores[0] = score;
            }
            highscores.Sort();
            for (int i = 0; i < 3; i++){
                PlayerPrefs.SetInt("Highscore: " + (i+1), highscores[i]);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            textAnimator.SkipAnimation();
            textAnimator.AnimateText("Game Over!");
            Invoke("loadMenu", 3f);
        } else if (health > 100) { health = 100; }
    }

    public void resetHealth(){
        health = maxHealth;
    }

    void loadMenu()
    {
        
        setScore(0);
        resetHealth();
        LoadScene(3);
        
    }

    public int getScore(){
        return score;
    }
    public void setScore(int amount)
    {
        score = amount;
    }

    public void addScore(int amount)
    {
        score += amount;
        textAnimator.AnimateAlert("Score: " + score);
    }

    public int getHealth()
    {
        return health;
    }

    public void LoadScene(int level){

        SceneManager.LoadScene(level);
    }

    public void text(string textToAppear){
        textAnimator.AnimateText(textToAppear);
    }


}
