using UnityEngine;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class TopDownCarController : MonoBehaviour
{
    [Header("Vehicle Settings")]
    public float accelerationForce = 8f;
    public float reverseForce = 5f;
    public float turnSpeed = 160f;
    public float maxSpeed = 7f;

    private Rigidbody2D rb;
    private float moveInput;
    private float turnInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        LimitSpeed();
    }

    private void ReadInput()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        moveInput = 0f;
        turnInput = 0f;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveInput += 1f;

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveInput -= 1f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            turnInput -= 1f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            turnInput += 1f;
#else
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
#endif
    }

    private void ApplyEngineForce()
    {
        if (moveInput > 0)
        {
            rb.AddForce(transform.up * moveInput * accelerationForce, ForceMode2D.Force);
        }
        else if (moveInput < 0)
        {
            rb.AddForce(transform.up * moveInput * reverseForce, ForceMode2D.Force);
        }
    }

    private void ApplySteering()
    {
        if (Mathf.Abs(turnInput) < 0.01f)
            return;

        if (rb.linearVelocity.magnitude < 0.1f)
            return;

        float movingDirection = Vector2.Dot(rb.linearVelocity, transform.up) >= 0 ? 1f : -1f;

        float rotationAmount = -turnInput * turnSpeed * movingDirection * Time.fixedDeltaTime;

        rb.MoveRotation(rb.rotation + rotationAmount);
    }

    private void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}