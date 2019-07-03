using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxCollider;

    [SerializeField] float enemyMoveSpeed = 1f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        myRigidBody.velocity = new Vector2(enemyMoveSpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipMoveDirection();
    }

    private void FlipMoveDirection()
    {
        enemyMoveSpeed *= -1;
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);        
    }
}
