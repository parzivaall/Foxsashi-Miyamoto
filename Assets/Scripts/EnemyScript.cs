using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Animator animator;
    public GameObject deathExplosion;
    private Collider2D gemCollider = null;

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
            gemCollider = collision;
            StartCoroutine(RunEveryTwoSeconds());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gem"))
        {
            gemCollider = null;
            StopCoroutine(RunEveryTwoSeconds());
        }
    }

    IEnumerator RunEveryTwoSeconds()
    {
        while (true)
        {
            if (gemCollider != null)
            {
                animator.SetTrigger("IsAttacking");
                EnvManager.Instance.setHealth(-10);
                Debug.Log(EnvManager.Instance.getHealth());
                if (EnvManager.Instance.getHealth() <= 0)
                {
                    Instantiate(deathExplosion, gemCollider.transform.position, Quaternion.identity);
                    Destroy(gemCollider.gameObject);
                }
            } else
            {
                Debug.LogWarning("Gem collider is null");
            }
            yield return new WaitForSeconds(2f);
        }
    }

    void OnDestroy()
    {
        StopCoroutine(RunEveryTwoSeconds());
        Instantiate(deathExplosion, this.transform.position, Quaternion.identity);
        EnvManager.Instance.addScore(1);
    }
}

