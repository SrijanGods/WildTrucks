using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public float speedOnAir = 2;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    public FixedJoystick fixedJoystick;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        fixedJoystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FixedJoystick>();
    }

    void FixedUpdate()
    {
            // Update IsRunning from input.
            IsRunning = canRun;

            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity =new Vector2( fixedJoystick.Horizontal * targetMovingSpeed * 0.4f, fixedJoystick.Vertical * targetMovingSpeed);

            // Apply movement.
            //rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x + rigidbody.velocity.x, rigidbody.velocity.y, targetVelocity.y + rigidbody.velocity.z );
        /*    
        float correctedVel = 0;
            if (transform.GetComponent<PlayerPhysics>().truck.transform.GetComponent<Rigidbody>().velocity.z > rigidbody.velocity.z)
            {
               correctedVel = transform.GetComponent<PlayerPhysics>().truck.transform.GetComponent<Rigidbody>().velocity.z - rigidbody.velocity.z;
            correctedVel *= 2;
            }
            else
            {
               correctedVel = 0;
            }
            print(correctedVel);
        */
            rigidbody.velocity = new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}