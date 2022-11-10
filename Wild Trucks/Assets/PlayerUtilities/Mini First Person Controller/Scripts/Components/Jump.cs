using UnityEngine;
using UnityEngine.UI;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public float jumpStrengthSide = 2;
    public event System.Action Jumped;

    private bool canJump = false;

    public bool isAttached = false;

    [SerializeField]
    public bool isSideColl = false;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
        canJump = false;
    }

    public void CheckJump()
    {
        if((!groundCheck || groundCheck.isGrounded || isAttached))
            canJump = true;
    }

    Vector3 lPos = Vector3.zero;
    float lSpeed = 0;

    void FixedUpdate()
    {
        float speed = (transform.position - lPos).magnitude/Time.deltaTime;
        lPos = transform.position;

        //transform.GetComponent<PlayerPhysics>().SetRigidbody(speed);

        float acc = (speed - lSpeed) / Time.deltaTime;
        lSpeed = speed;

        if (isSideColl)
        {
            // Jump when the Jump button is pressed and we are on the ground.
            if (canJump && (!groundCheck || groundCheck.isGrounded || isAttached))
            {
                rigidbody.AddForce(Vector3.up * 100 * jumpStrengthSide * jumpStrength);
                rigidbody.AddForce(Vector3.forward * 100 * jumpStrength);
                Jumped?.Invoke();
                canJump = false;
            }
        }
        else
        {
            // Jump when the Jump button is pressed and we are on the ground.
            if (canJump && (!groundCheck || groundCheck.isGrounded || isAttached))
            {
                rigidbody.AddRelativeForce(new Vector3(0,1,1) * 100 * jumpStrength);
                //rigidbody.AddRelativeForce(Vector3.forward * 100 * speed * jumpStrength);
                Jumped?.Invoke();
                canJump = false;
            }
        }
    }
}
