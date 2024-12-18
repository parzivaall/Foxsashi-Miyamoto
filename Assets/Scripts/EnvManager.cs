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
    
    private SpawnPoints spawnPoints;
    public GameObject[] enemyTypes;
    public float spawnTimer = 5f;

    private bool hard;

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

        if (PlayerPrefs.HasKey("Hard")){
            hard = PlayerPrefs.GetInt("Hard") == 1;
        }

        for (int i = 0; i < 10; i++){
            if (PlayerPrefs.HasKey("Highscore: " + (i+1))){
                highscores.Add(PlayerPrefs.GetInt("Highscore: " + (i+1), 0));
            } else{
                highscores.Add(0);
            }
        }
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            setVolume(volume);
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
        if (scene.name != "MainMenu" || scene.name != "Shop" || scene.name != "GameOver"){

            try
            {
                spawnPoints = GameObject.Find("SpawnPoints").GetComponent<SpawnPoints>();
                if (spawnPoints != null){
                    StartCoroutine(Spawner());
                }
                textAnimator.AnimateWarning("Start!");
                StartCoroutine(instructions());
            }
            catch
            {
                Debug.LogError("No SpawnPoints found in scene");
            }
            
        } else {
            spawnPoints = null;
        }
    }

    IEnumerator instructions(){
        yield return new WaitForSeconds(2f);
        textAnimator.AnimateText("Defend the crystal!");
    }

    public void setVolume(float volume){
        damageSFX.volume = volume;
        gameOverSFX.volume = volume;
        musicOrganizer.setVolume(volume);
    }

    public float getVolume(){
        return musicOrganizer.getVolume();
    }

    public void setHealth(int damage)
    {
        if (currentSceneName == "MainMenu" || currentSceneName == "Shop") {
            if (damage > 0) {
                health = 1;
                return;
            } else {
                health = 0;
                return;
            }
        }
        health += damage;
        textAnimator.AnimateWarning("Health: " + health);
        damageSFX.Play();
        if (health <= 0) {
            spawnTimer = 5f;
            StopAllCoroutines();
            gameOverSFX.Play();
            if (score > highscores[0])
            {
                highscores[0] = score;
            }
            highscores.Sort();
            for (int i = 0; i < 10; i++){
                PlayerPrefs.SetInt("Highscore: " + (i+1), highscores[i]);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.attackOff();
                    playerMovement.enabled = false;
                }
            }
            textAnimator.SkipAnimation();
            textAnimator.AnimateWarning("Game Over!");
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
        LoadScene(4);
        
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

    public void warning(string textToAppear){
        textAnimator.AnimateWarning(textToAppear);
    }

    public void alert(string textToAppear){
        textAnimator.AnimateAlert(textToAppear);
    }

    private IEnumerator Spawner(){
        while(true){
            yield return new WaitForSeconds(spawnTimer);
            if (!hard){
                spawnTimer -= 0.1f;
            } else {
                spawnTimer -= 3f;
            }
            if(spawnTimer <= 0.1f){
                spawnTimer = 0.1f;
            }
            int ran = Random.Range(0, enemyTypes.Length);
            GameObject Enemy = enemyTypes[ran];
            Instantiate(Enemy, spawnPoints.GetRandomSpawnPoint(), Quaternion.identity);
        }
    }

    public void setHard(bool hardMode){
        this.hard = hardMode;
    }

}