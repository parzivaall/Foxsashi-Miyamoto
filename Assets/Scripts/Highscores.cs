using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscores : MonoBehaviour
{
    public List<int> highscores = new List<int>();
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++){
            if (PlayerPrefs.HasKey("Highscore: " + (i+1))){
                highscores.Add(PlayerPrefs.GetInt("Highscore: " + (i+1), 0));
            } else{
                highscores.Add(0);
            }
        }
        highscores.Sort();
        highscores.Reverse();
        for (int i = 0; i < 10; i++){
            text.text += (i + 1) + ": " + highscores[i] + " Birds Plucked\n";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}