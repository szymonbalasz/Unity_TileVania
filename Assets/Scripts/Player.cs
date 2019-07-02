using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //cached references
    Rigidbody2D myRigidBody;

    [Header("Movement")]
    [SerializeField] float playerMoveSpeed = 5f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * playerMoveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }
}
