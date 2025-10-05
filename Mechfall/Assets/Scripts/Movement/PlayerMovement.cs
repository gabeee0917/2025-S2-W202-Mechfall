using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    #region Variables
    //movement variables
    public float HorizontalVelo { get; private set; }
    private bool isFacingRight;

    //collision check variables
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool headBumped;

    //jump variables
    public float VerticalVelo { get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numJumpsUsed;

    //apex variables
    private float apexJumpPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //jump buffer variables
    private float jumpBufferTimer;
    private bool releasedDuringBuffer;

    //coyote time variables
    private float coyoteTimer;

    //dash variables
    private bool isDashing;
    private bool isAirDashing;
    private float dashTimer;
    private float dashGroundTimer;
    private int numDashesUsed;
    private Vector2 dashDirection;
    private bool isDashFastFalling;
    private float dashFastFallTime;
    private float dashFastFallReleaseSpeed;

    [Header("SoundFX")]
    private AudioSource AudioSource;
    [SerializeField] private AudioClip jumpSound;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
        LandCheck();
        DashCheck();

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Dash();
        Jump();
        Fall();

        if (isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
        }
        ApplyVelo();
    }

    private void ApplyVelo()
    {
         // Clamp Fall Speed
        VerticalVelo = Mathf.Clamp(VerticalVelo, -MoveStats.MaxFallSpeed, 50f);
        rb.linearVelocity = new Vector2(HorizontalVelo, VerticalVelo);
    }

    #region Movement
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {

        if (Mathf.Abs(moveInput.x) >= MoveStats.MoveThreshold)
        {
            TurnCheck(moveInput);

            float targetVelocity = 0f;
            if (InputManager.RunHeld)
            {
                targetVelocity = moveInput.x * MoveStats.MaxRunSpeed;
                animator.SetBool("isRunning", true);

            }
            else
            {
                targetVelocity = moveInput.x * MoveStats.MaxWalkSpeed;
                animator.SetBool("isWalking", true);
            }

            HorizontalVelo = Mathf.Lerp(HorizontalVelo, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if (Mathf.Abs(moveInput.x) < MoveStats.MoveThreshold)
        {
            HorizontalVelo = Mathf.Lerp(HorizontalVelo, 0f, deceleration * Time.fixedDeltaTime);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Dash

    private void DashCheck()
    {
        if (InputManager.DashPressed)
        {
            //ground dash
            if (isGrounded && dashGroundTimer < 0 && !isDashing)
            {
                InitiateDash();
            }
            //air dash
            else if (!isGrounded && !isDashing && numDashesUsed < MoveStats.NumOfDashes)
            {
                isAirDashing = true;
                InitiateDash();
            }
        }
    }

    private void InitiateDash()
    {
        dashDirection = InputManager.Movement;
        Vector2 closestDirection = Vector2.zero;
        float minDist = Vector2.Distance(dashDirection, MoveStats.DashDirections[0]);

        for (int i = 0; i < MoveStats.DashDirections.Length; i++)
        {
            //skip if we hit direction
            if (dashDirection == MoveStats.DashDirections[i])
            {
                closestDirection = dashDirection;
                break;
            }

            float distance = Vector2.Distance(dashDirection, MoveStats.DashDirections[i]);

            //check diagonal distance and apply the bias
            bool isDiagonal = (Mathf.Abs(MoveStats.DashDirections[i].x) == 1 && Mathf.Abs(MoveStats.DashDirections[i].y) == 1);
            if (isDiagonal)
            {
                distance -= MoveStats.DiagonalDashBias;
            }
            else if (distance < minDist)
            {
                minDist = distance;
                closestDirection = MoveStats.DashDirections[i];
            }
        }

        // handle no direction
        if (closestDirection == Vector2.zero)
        {
            if (isFacingRight)
            {
                closestDirection = Vector2.right;
            }
            else
            {
                closestDirection = Vector2.left;
            }
        }
        dashDirection = closestDirection;
        numDashesUsed++;
        isDashing = true;
        dashTimer = 0f;
        dashGroundTimer = MoveStats.DashOnGroundTime;
    }

    private void Dash()
    {
        if (isDashing || isAirDashing)
        {
            //stop dash timer
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= MoveStats.DashTime)
            {
                isAirDashing = false;
                isDashing = false;

                if (!isJumping)
                {
                    dashFastFallTime = 0f;
                    dashFastFallReleaseSpeed = VerticalVelo;

                    if (!isGrounded)
                    {
                        isDashFastFalling = true;
                    }
                }
                return;
            }

            HorizontalVelo = MoveStats.DashSpeed * dashDirection.x;

            if (dashDirection.y > 0f)
            {
                float dashVert = MoveStats.DashSpeed * dashDirection.y;
                if (dashVert > VerticalVelo)
                {
                    VerticalVelo = dashVert;
                }
            }
            //Dash Cut Time
            else if (isDashFastFalling)
            {
                if (VerticalVelo > 0f)
                {
                    if (dashFastFallTime < MoveStats.UpwardsCancelDashTime)
                    {
                        VerticalVelo = Mathf.Lerp(dashFastFallReleaseSpeed, 0f, (dashFastFallTime / MoveStats.UpwardsCancelDashTime));
                    }
                    else if (dashFastFallTime >= MoveStats.UpwardsCancelDashTime)
                    {
                        VerticalVelo += MoveStats.Gravity * MoveStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                    }
                    dashFastFallTime += Time.fixedDeltaTime;
                }
                else
                {
                    VerticalVelo += MoveStats.Gravity * MoveStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
            }
        }
    }

    private void ResetDashes()
    {
        numDashesUsed = 0;
    }

    #endregion

    #region Jump

    private void JumpChecks()
    {
        // When we press Jump
        if (InputManager.jumpPressed)
        {
            jumpBufferTimer = MoveStats.JumpBufferTime;
            releasedDuringBuffer = false;
            //animator.SetBool("Jump", true);
            animator.SetTrigger("JumpTrig");
        }

        // When we release Jump
        if (InputManager.jumpReleased)
        {
            if (jumpBufferTimer > 0f)
            {
                releasedDuringBuffer = true;
            }

            if (isJumping && VerticalVelo > 0f)
            {
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelo = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = VerticalVelo;
                }
            }
        }

        // Initiate Jump with Buffering and Coyote Time
        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);
            jumpBufferTimer = 0f;

            if (releasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = VerticalVelo;
            }
        }

        // Double Jump
        else if (jumpBufferTimer > 0f && isJumping && numJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        numJumpsUsed += numberOfJumpsUsed;
        VerticalVelo = MoveStats.InitialJumpVelocity;
        SoundManager.instance.playSound(jumpSound);
    }

    private void Jump()
    {
        // Apply Gravity While Jumping
        if (isJumping)
        {
            // Check For Head Bump
            if (headBumped)
            {
                isFastFalling = true;
            }

            // Gravity on Ascending
            if (VerticalVelo >= 0f)
            {
                // Apex Controls
                apexJumpPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelo);
                if (apexJumpPoint > MoveStats.ApexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelo = 0f;
                        }
                        else
                        {
                            VerticalVelo = -0.01f;
                        }
                    }
                }

                // Gravity on Ascending But not past apex threshold
                else
                {
                    VerticalVelo += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }

            // Gravity on Descending
            else if (!isFastFalling)
            {
                VerticalVelo += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelo < 0f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        // Jump Cut
        if (isFastFalling)
        {
            if (fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelo += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelo = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / MoveStats.TimeForUpwardsCancel));
            }
            fastFallTime += Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Landing

    private void LandCheck()
    {
        // Landed
        if ((isJumping || isFalling) && isGrounded && VerticalVelo <= 0f)
        {
            ResetDashes();
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numJumpsUsed = 0;
            VerticalVelo = 0f;
        }
    }

    private void Fall()
    {
        // Normal Gravity While Falling
        if (!isGrounded && !isJumping)
        {
            if (!isFalling)
            {
                isFalling = true;
            }

            VerticalVelo += MoveStats.Gravity * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Collision Checks

    private void checkGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, MoveStats.GroundDectectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDectectionRayLength, MoveStats.GroundLayer);
        if (groundHit.collider != null)
        {
            isGrounded = true;
        }
        else { isGrounded = false; }
        //animator.SetBool("Jump", !isGrounded);

    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDectectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDectectionRayLength, MoveStats.GroundLayer);
        if (headHit.collider != null)
        {
            headBumped = true;
        }
        else { headBumped = false; }

    }
    private void CollisionChecks()
    {
        checkGrounded();
        BumpedHead();

    }

    #endregion

    #region Timers
    private void CountTimers()
    {
        // Jump Buffer
        jumpBufferTimer -= Time.deltaTime;

        //Jump Coyote Timer
        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else { coyoteTimer = MoveStats.JumpCoyoteTime; }

        //Dash Timer
        if (isGrounded)
        {
            dashGroundTimer -= Time.fixedDeltaTime;
        }
    }
    #endregion

}
