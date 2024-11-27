using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    public GameObject deathExplosion;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AttackCheck");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {   
            Instantiate(deathExplosion, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            
        }
    }
}
