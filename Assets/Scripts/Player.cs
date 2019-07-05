using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //cache
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    float gravityScaleAtStart;

    //states
    bool isAlive = true;

    //config
    [Header("Movement")]
    [SerializeField] float playerMoveSpeed = 5f;
    [SerializeField] float playerJumpSpeed = 5f;
    [SerializeField] float playerClimbSpeed = 5f;
    [SerializeField] float climbingHorizontalSlowDown = 1f;
    float tempMoveSpeed;

    [Header("Death")]
    [SerializeField] Vector2 playerDeathEffect = new Vector2(25f, 25f);


    void Start()
    {
        tempMoveSpeed = playerMoveSpeed;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = myRigidBody.gravityScale;

        FindObjectOfType<GameSession>().ResetOverlays();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * playerMoveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("Running", true);
        }
        else
        {
            myAnimator.SetBool("Running", false);
        }
    }

    private void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, playerJumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            playerMoveSpeed = tempMoveSpeed;
            return;            
        }

        playerMoveSpeed = climbingHorizontalSlowDown;
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * playerClimbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = (Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon);
        if (playerHasVerticalSpeed)
        {
            myAnimator.SetBool("Climbing", true);
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = playerDeathEffect;
            isAlive = false;
            FindObjectOfType<GameSession>().processPlayerDeath();
        }
    }


}
