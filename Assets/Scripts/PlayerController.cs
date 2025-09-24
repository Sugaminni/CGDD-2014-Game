using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform cameraPivot;

    [Header("Speeds")] 
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 7.5f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float minPitch = -75f;
    [SerializeField] private float maxPitch = 75f;

    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.1f;

    private CharacterController cc;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float yVel;
    private float pitch;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Ability to look around
        float yaw = lookInput.x * mouseSensitivity;
        float pitchDelta = -lookInput.y * mouseSensitivity;
        transform.Rotate(0f, yaw, 0f, Space.Self);

        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        if (cameraPivot != null)            // guard against missing reference
            cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);

        // Movement
        Vector3 input = new Vector3(moveInput.x, 0f, moveInput.y);
        if (input.sqrMagnitude > 1f) input.Normalize(); // prevent faster diagonal
        Vector3 world = transform.TransformDirection(input);
        float speed = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed
                      ? sprintSpeed : walkSpeed; // supports Both/Old input settings too

        // Jumping & Gravity
        if (cc.isGrounded && yVel < 0f) yVel = -2f;     // small stick-to-ground force
        if (cc.isGrounded && (Keyboard.current != null ? Keyboard.current.spaceKey.wasPressedThisFrame
                                                       : Input.GetKeyDown(KeyCode.Space)))
        {
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        yVel += gravity * Time.deltaTime;

        Vector3 vel = world * speed + Vector3.up * yVel;
        cc.Move(vel * Time.deltaTime);
    }

    // Input System
    void OnMove(InputValue v) => moveInput = v.Get<Vector2>();   // action name: "Move"
    void OnLook(InputValue v) => lookInput = v.Get<Vector2>();   // action name: "Look"
}
