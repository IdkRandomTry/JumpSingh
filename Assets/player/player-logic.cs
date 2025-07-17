using UnityEngine;

public class jump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float minJumpPixelsY = 16f;      // Minimum jump height in pixels
    public float maxJumpPixelsY = 96f;
    public float minJumpPixelsX = 16f;      // Minimum jump height in pixels
    public float maxJumpPixelsX = 112f;     // Maximum jump height in pixels
    public float pixelsPerUnit = 32;      // Pixels Per Unit
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
    public float gravity;                  // Gravity in units per second squared

    [Header("Visuals")]
    public Sprite NormalSprite;
    public Sprite MidairSprite;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = Mathf.Abs(rb.gravityScale * Physics2D.gravity.y);
        PrecalculateJumpVelocities();
        sr = GetComponent<SpriteRenderer>();
    }

    void PrecalculateJumpVelocities()
    {
        jumpVelocityY = new float[discreteJumps];
        jumpVelocityX = new float[discreteJumps];

        for (int i = 0; i < discreteJumps; i++)
        {
            // Calculate jump height for this level
            float jumpHeightPixels = Mathf.Lerp(minJumpPixelsY, maxJumpPixelsY, i / ((float)discreteJumps - 1.0f));
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
        LookSharp();
        CheckGrounded();
        JumpLogic();
        ClampVelocity();
    }

    void CheckGrounded()
    {
        // Need to add collision detection logic here
        isGrounded = (Mathf.Abs(rb.linearVelocity.y) < 0.2);
        if (isGrounded)
        {
            sr.sprite = NormalSprite;
        }
        else
        {
            sr.sprite = MidairSprite;
        }
    }

    void LookSharp()
    {
        // Flip the sprite based on movement direction
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            sr.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            sr.flipX = false;
        }
    }

    void JumpLogic()
    {
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
            currentJumpLevel = Mathf.FloorToInt(chargePercent * ((float)discreteJumps - 1.0f));
        }

        // Release jump
        if (isCharging && Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                rb.linearVelocity = new Vector2(jumpVelocityX[currentJumpLevel], jumpVelocityY[currentJumpLevel]);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                rb.linearVelocity = new Vector2(-1.0f * jumpVelocityX[currentJumpLevel], jumpVelocityY[currentJumpLevel]);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, jumpVelocityY[currentJumpLevel]);
            }
            isCharging = false;
        }
    }

    void ClampVelocity()
    {
        // Clamp velocity to prevent excessive speed
        if (Mathf.Abs(rb.linearVelocity.x) > jumpVelocityX[discreteJumps - 1])
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * jumpVelocityX[discreteJumps - 1], rb.linearVelocity.y);
        }
        if (Mathf.Abs(rb.linearVelocity.y) > jumpVelocityY[discreteJumps - 1])
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sign(rb.linearVelocity.y) * jumpVelocityY[discreteJumps - 1]);
        }
    }
}

