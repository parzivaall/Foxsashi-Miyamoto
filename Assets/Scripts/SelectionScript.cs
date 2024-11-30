using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    public GameObject selection;
    // Start is called before the first frame update
    void Start()
    {
        selection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSelection(){
        selection.SetActive(true);
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
