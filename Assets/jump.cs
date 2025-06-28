using UnityEngine;

public class jump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float minJumpPixelsY = 32f;      // Minimum jump height in pixels
    public float maxJumpPixelsY = 112f;
    public float minJumpPixelsX = 32f;      // Minimum jump height in pixels
    public float maxJumpPixelsX = 112f;     // Maximum jump height in pixels
    public float pixelsPerUnit = 100f;      // Pixels Per Unit
    public float chargeTime = 1.5f;         // Time to reach max charge in seconds
    public int discreteJumps = 20;          // Number of discrete jump levels


    [Header("Debug")]
    public int currentJumpLevel = 0;        // Current charge level

    private float[] jumpVelocityY;             // Pre-calculate jump forces for nerds
    private float[] jumpVelocityX;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float chargeStartTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PrecalculateJumpVelocities();
    }

    void PrecalculateJumpVelocities()
    {
        jumpVelocityY = new float[discreteJumps];
        jumpVelocityX = new float[discreteJumps];
        float gravity = Mathf.Abs(Physics2D.gravity.y);

        for (int i = 0; i < discreteJumps; i++)
        {
            // Calculate jump height for this level
            float jumpHeightPixels = Mathf.Lerp(minJumpPixelsY, maxJumpPixelsY, i / ((float)discreteJumps-1.0f));
            float jumpHeightUnits = jumpHeightPixels / pixelsPerUnit;

            // Calculate required jump velocity
            jumpVelocityY[i] = Mathf.Sqrt(2 * gravity * jumpHeightUnits);          // Fiziks: v = sqrt(2 * g * h)
            
            // Calculate jump length for this level
            float jumpLengthPixels = Mathf.Lerp(minJumpPixelsX, maxJumpPixelsX, i / ((float)discreteJumps - 1.0f));
            float jumpLengthUnits = jumpLengthPixels / pixelsPerUnit;
            float jumpTime = 2.0f * jumpVelocityY[i] / gravity;                     // t = 2 * u / g

            jumpVelocityX[i] = jumpLengthUnits / jumpTime;                          // v = d / t
        }
    }

    void Update()
    {
        CheckGrounded();

        // Start charging
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            chargeStartTime = Time.time;
            currentJumpLevel = 0;
        }

        // While charging
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            float chargePercent = Mathf.Clamp01((Time.time - chargeStartTime) / chargeTime);
            currentJumpLevel = Mathf.FloorToInt(chargePercent * ((float) discreteJumps-1.0f));
        }

        // Release jump
        if (isCharging && Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                rb.linearVelocity = new Vector2(jumpVelocityX[currentJumpLevel], jumpVelocityY[currentJumpLevel]);
            }
            else if (Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
            {
                rb.linearVelocity = new Vector2(-1.0f*jumpVelocityX[currentJumpLevel], jumpVelocityY[currentJumpLevel]);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, jumpVelocityY[currentJumpLevel]);
            }
            isCharging = false;
        }
    }

    void CheckGrounded()
    {
        // Need to add collision detection logic here
        isGrounded = (Mathf.Abs(rb.linearVelocity.y) < 0.2);
    }
}

