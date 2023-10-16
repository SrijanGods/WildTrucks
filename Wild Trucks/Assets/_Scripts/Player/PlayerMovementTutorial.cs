using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float speedMultiplier;
    public float jumpForce = 8f;
    public float jumpCooldown;
    public float airMultiplier;
    //public float time;
    public AnimationCurve speedVTime;

    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    [Header("Debug Tools")]
    public TextMeshProUGUI stats;

    [Header("UI")]
    public GameObject deathPanel;
    public GameObject winPanel;
    public Button restartButton;
    public Button nextLevelButton;

    FixedJoystick fixedJoystick;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    LevelManager levelManager;
    GameManager gameManager;

    int mSpeed;

    private void Start()
    {
        Application.targetFrameRate = 60;

        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;

        readyToJump = true;

        fixedJoystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FixedJoystick>();

        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        mSpeed = 60;

        restartButton.onClick.AddListener(gameManager.restartGame);
        nextLevelButton.onClick.AddListener(gameManager.nextLevel);

        
    }

    bool call = true;

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();

        if(grounded && call)
        {
            call = false;
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.3f);
            SpeedControl(hit.collider.gameObject);

            Debug.LogError("Speed Maintained");
        }

        if (!grounded)
        {
            call = true;
        }

        stats.text = "Multiplier is:" + multy +"\n" + "Speed is:" + i + "\n" + "JumpMulty is:" + jumpForce;
        /*
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        */
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = fixedJoystick.Horizontal;
        verticalInput = fixedJoystick.Vertical;

    }

    public void JumpCommand()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            //print("JUMPING");
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    //VARAIBLES
    float i;
    float multy;

    private void MovePlayer()
    {
        // calculate movement direction
        if (grounded)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput/8;
        }
        else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        i = rb.velocity.magnitude;
        multy = Mathf.Clamp(speedVTime.Evaluate(i), 3f, 15f);
        //multy = 10f;

        // on ground
        if (grounded)
        {
            /*if (rb.velocity.magnitude < mSpeed * 0.7)
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else*/
            rb.AddRelativeForce(moveDirection.normalized * moveSpeed * speedMultiplier * multy, ForceMode.Force);
        }

        // in air
        
        else if (!grounded)
            if (rb.velocity.magnitude < mSpeed)
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * multy * airMultiplier, ForceMode.Force);
            else
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * airMultiplier * multy, ForceMode.Force);
        
    }

    private void SpeedControl(GameObject colliderObj)
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(colliderObj.transform.parent != null)
        {
            if(colliderObj.transform.parent.gameObject.tag == "Truck")
            {
                Vector3 speed = colliderObj.transform.parent.GetComponent<TruckPathFollow>().movementSpeed;
                rb.velocity = new Vector3(speed.x, rb.velocity.y, speed.z);
            }
        }


        //print(flatVel.magnitude);
        //limit velocity if needed
        /*
        if (flatVel.magnitude > 60f)
        {
            print("Hmm");
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        */

    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        i = rb.velocity.magnitude;
        //jumpForce = Mathf.Clamp(speedVTime.Evaluate(i)/2, 4.5f, 7f);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Death")
        {
            gameManager.PauseGame();
            if (collision.gameObject.name == "Finish")
            {
                winPanel.SetActive(true);
                return;
            }

            deathPanel.SetActive(true);

        }
    }

    public void HomeScreen()
    {
        gameManager.nextLevel();
    }
}