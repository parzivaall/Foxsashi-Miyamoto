using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGem : MonoBehaviour
{
    public GameObject titleGem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy(){
        if (this.transform.position.x > 0 ){
            Instantiate(titleGem, new Vector3(this.transform.position.x - 8, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        } else {
            Instantiate(titleGem, new Vector3(this.transform.position.x + 8, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        }
        
    }
}
