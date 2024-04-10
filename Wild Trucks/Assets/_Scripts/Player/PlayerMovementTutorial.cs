using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UmbraEvolution.PerLayerCameraCulling;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float speedMultiplier;
    public float jumpCooldown;
    public float airMultiplier;
    //public float time;
    [Header("Curves")]
    public AnimationCurve speedVTime;
    public AnimationCurve jumpVTime;
    public AnimationCurve jumpForwardVTime;

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

    [HideInInspector]
    public bool isSide = false;

    private PerLayerCulling pCull;
    float jumpForce;
    float i;
    float j;
    float k;
    float multy;

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

        pCull = GetComponentInChildren<PerLayerCulling>();
    }

    bool call = true;
    bool once = true;

    private void LateUpdate()
    {
        pCull.TriggerUpdate();
    }
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

            //Debug.LogError("Speed Maintained");

            once = true;
        }

        if (!grounded)
        {
            call = true;
            if (once)
            {
                StartCoroutine("DownForce");
                once = false;
            }
            
        }

        if (isSide)
        {
            float x = rb.velocity.y - 1;
            Mathf.Clamp(x, -1, 50);
            rb.velocity = new Vector3(rb.velocity.x, -1f, rb.velocity.z);

            //Debug.LogError("Speed Negative");
        }
        else
        {
           // rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        stats.text = "Multiplier is:" + multy +"\n" + "Speed is:" + j + "\n" + "JumpMulty is:" + jumpForce;

        
    }

    IEnumerator DownForce()
    {
        yield return new WaitForSeconds(1.3f);
        if(!grounded)
            rb.AddForce(-40f * transform.up, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private bool hozIn = false;
    private bool verIn = false;
    private void MyInput()
    {
        horizontalInput = fixedJoystick.Horizontal;
        verticalInput = fixedJoystick.Vertical;

        if(hozIn)
        {
            horizontalInput = 0;
        }
        if (verIn)
        {
            verticalInput = 0;
        }
    }

  

    public void JumpCommand()
    {
        if ((readyToJump && grounded) || isSide)
        {
            readyToJump = false;

            Jump(isSide);
            isSide = false;

            //print("JUMPING");
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

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
            if(colliderObj.transform.name == "SideFrontCollider")
            {
                isSide = true;
                Debug.LogError("Side Jump");
            }
        }

    }

    

    private void Jump(bool sideTouch)
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        j = rb.velocity.magnitude;
        jumpForce = Mathf.Clamp(jumpVTime.Evaluate(j), 10.3f, 40f);
        k = Mathf.Clamp(jumpForwardVTime.Evaluate(jumpForce), 1f, 5f);

        if (isSide)
        {
            rb.AddForce(transform.up * jumpForce * 1.05f, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        rb.AddForce(transform.forward * jumpForce * k , ForceMode.Impulse);
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

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name == "SideFrontCollider")
        {
            rb.velocity = new Vector3(rb.velocity.x, -2, rb.velocity.z);
            hozIn = true;
        }
        if (collision.gameObject.name == "FrontCollider")
        {
            rb.velocity = new Vector3(rb.velocity.x, -2, rb.velocity.z);
            verIn = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "SideFrontCollider")
        {
            hozIn = false;
        }
        if (collision.gameObject.name == "FrontCollider")
        {
            verIn = false;
        }
    }

    public void HomeScreen()
    {
        gameManager.nextLevel();
    }
}