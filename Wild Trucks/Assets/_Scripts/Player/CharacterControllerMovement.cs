using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    public Vector3 playerMovementInput;
    private Vector3 velocity;

    public CharacterController controller;
    public float speed;
    public float jumpForce;
    public float senstivity;
    public float gravity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(playerMovementInput);

        if (controller.isGrounded)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y -= gravity * 2f * Time.deltaTime;
        }

        controller.Move(moveVector * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }
}
