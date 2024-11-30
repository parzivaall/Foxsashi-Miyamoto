using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    private Rigidbody2D rb;
    public Animator animator;

    public float runSpeed = 40f;
    public float attackSpeed = 99999f;

    float horizontalMove = 0f;

    bool jump = false;
    bool crouch = false;
    bool attack = false;

    public GameObject attackCheck;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();

        attackCheck = GameObject.Find("AttackCheck");
        Debug.Log("AttackCheck"+ attackCheck.name);
        attackCheck.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")){
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Attack")){
            attack = true;
            animator.SetTrigger("IsAttacking");
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Crouch")){
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")){
            crouch = false;
        }
    }

    public void OnLandEvent(){
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouchEvent(bool crouch){
        animator.SetBool("IsCrouching", crouch);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
        
        if (attack){
            if(controller.IsFacingRight){
                rb.AddForce(new Vector2(attackSpeed, 0), ForceMode2D.Impulse);
            } else {
                rb.AddForce(new Vector2(-attackSpeed, 0), ForceMode2D.Impulse);
            }
            StartCoroutine(AttackDelay());
            attack = false;
        }
        
        
    }

    public void attackOff(){
        attackCheck.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator AttackDelay(){
        attackCheck.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        attackCheck.gameObject.SetActive(false);
    }
}
