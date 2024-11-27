using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Animator animator;
    public GameObject deathExplosion;
    int health = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAttackEvent(){
        animator.SetTrigger("IsAttacking");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gem"))
        {
            animator.SetTrigger("IsAttacking");
            health --;
            if(health <= 0){
                Instantiate(deathExplosion, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
            
        }
    }
}
