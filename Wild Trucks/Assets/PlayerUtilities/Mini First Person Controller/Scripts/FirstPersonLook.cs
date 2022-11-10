using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    private FixedTouchInput fixedTouch;

    Vector2 velocity;
    Vector2 velocityy;
    Vector2 frameVelocity;
    Vector2 frameVelocityy;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        //Cursor.lockState = CursorLockMode.Locked;

        fixedTouch = GameObject.FindGameObjectWithTag("TouchField").GetComponent<FixedTouchInput>();
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(fixedTouch.TouchDist.x, fixedTouch.TouchDist.y);
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        frameVelocityy = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / (smoothing * 15));
        velocity += frameVelocity;
        velocityy += frameVelocityy;
        velocity.y = Mathf.Clamp(velocityy.y, -20, 5);
        velocity.x = Mathf.Clamp(velocity.x, -50, 50);

        // Rotate camera up-down and controller left-right from velocity.
        //transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
