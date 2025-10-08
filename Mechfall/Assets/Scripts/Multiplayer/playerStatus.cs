using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Player status manager for PVP, manages the player movement, animations, health, win and lose panels, gameover, stealth etc
// heavily usess rpcs to synch the status of each player in both users' devices

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int health = 100;
    public bool damageImmune = false;
    public int lives = 1;
    public bool isDead = false;
    public bool jumpable = false;
    public Animator animator;
    public bool isFacingRight = true;
    private Rigidbody2D rb;
    public Animator glow;
    public float moveSpeed = 12f;
    public float jumpForce = 5f;
    private bool jump;
    private Material targetMaterial;
    private Color baseGlowColor;
    private float currentSpeed = 0f;
    public float acceleration = 50f;
    public float deceleration = 50f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float moveInput = 0f;

    public Transform wing;

    public bool inStealth;
    public GameObject stealthPanel;

    public GameObject beamalight;
    private Coroutine stealthCoroutine;

    public float dashSpeed = 10f;
    public float dashTime = 0.5f;

    private bool isDashing = false;
    private float dashTimer;

    public SpriteRenderer muzz;

    public bool gameover;
    public GameObject losepanel;
    public GameObject winpanel;

    //initialise player status on start, adds a light prefab to the player, makes the local camera follow player
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        glow = transform.Find("glow")?.GetComponent<Animator>();
        wing = transform.Find("wing");
        muzz = transform.Find("muzzflash")?.GetComponent<SpriteRenderer>();
        inStealth = false;
        if (photonView.IsMine)
        {
            GameObject light = Resources.Load<GameObject>("OnlySelfSeeLight");
            if (light != null)
            {
                GameObject lightInstance = Instantiate(light, transform);
                lightInstance.transform.localPosition = new Vector3(0f, 0f, 0);
            }
        }

        camerapvpmove cameraFollow = Camera.main.GetComponent<camerapvpmove>();
        if (cameraFollow != null)
        {
            cameraFollow.target = this.transform;
        }

        Transform glowTransform = transform.Find("glow");
        if (glowTransform != null)
        {
            SpriteRenderer renderer = glowTransform.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                targetMaterial = renderer.material;
                baseGlowColor = targetMaterial.GetColor("_GlowColor");
            }
        }

    }

    // makes sure inputs only work on own player, not the other player that has the same scripts on it, also inputs dont work if player is dead.
    void Update()
    {
        if (!photonView.IsMine) return;
        if (isDead) return;

        //if fall off map, take 25 damage and return to middle
        if (transform.position.y < -15f)
        {
            transform.position = new Vector3(0f, 1f, 0f);
            RPC_TakeDamage(25);
        }

        moveInput = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
            //animator.SetBool("isRunning", true);
            //glow.SetBool("isRunning", true);
            photonView.RPC("RPC_RunYesAnim", RpcTarget.AllBuffered);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
            //animator.SetBool("isRunning", true);
            //glow.SetBool("isRunning", true);
            photonView.RPC("RPC_RunYesAnim", RpcTarget.AllBuffered);
        }
        else
        {
            //animator.SetBool("isRunning", false);
            //glow.SetBool("isRunning", false);
            photonView.RPC("RPC_RunNoAnim", RpcTarget.AllBuffered);
        }

        if (moveInput == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (moveInput > 0 && !isFacingRight)
        {
            photonView.RPC("RPC_Flip", RpcTarget.AllBuffered);
        }
        else if (moveInput < 0 && isFacingRight)
        {
            photonView.RPC("RPC_Flip", RpcTarget.AllBuffered);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpable == true)
        {
            jump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            photonView.RPC("RPC_JumpAnim", RpcTarget.AllBuffered);
            //animator.SetTrigger("jump");
            //glow.SetTrigger("jump");
        }

        // stealth only works in pvp
        if (SceneManager.GetActiveScene().name == "PVP" && Input.GetKeyDown(KeyCode.G))
        {
            if (inStealth == false)
            {
                Stealth(false);
                if (photonView.IsMine && stealthPanel != null)
                {
                    stealthPanel.SetActive(true);
                }
                photonView.RPC("RPC_Stealth", RpcTarget.Others, true);
                inStealth = true;
                stealthCoroutine = StartCoroutine(StealthTakeDamage());
            }
            else
            {
                Stealth(false);
                if (photonView.IsMine && stealthPanel != null)
                {
                    stealthPanel.SetActive(false);
                }
                photonView.RPC("RPC_Stealth", RpcTarget.Others, false);
                inStealth = false;
                if (stealthCoroutine != null)
                {
                    StopCoroutine(stealthCoroutine);
                    stealthCoroutine = null;
                }
            }
        }

        //pseudodash, not really a dash
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTime;
            photonView.RPC("RPC_wingonoff", RpcTarget.AllBuffered);
            photonView.RPC("RPC_DashAnim", RpcTarget.AllBuffered);
        }



    }

    public void RunNoAnim()
    {
        animator.SetBool("isRunning", false);
        glow.SetBool("isRunning", false);


    }

    [PunRPC]
    void RPC_RunNoAnim()
    {
        RunNoAnim();
    }


    public void DashAnim()
    {
        animator.SetTrigger("dash");
        glow.SetTrigger("dash");

    }
    [PunRPC]
    void RPC_DashAnim()
    {
        DashAnim();
    }


    public void RunYesAnim()
    {
        animator.SetBool("isRunning", true);
        glow.SetBool("isRunning", true);



    }

    [PunRPC]
    void RPC_RunYesAnim()
    {
        RunYesAnim();
    }


    public void JumpAnim()
    {
        animator.SetTrigger("jump");
        glow.SetTrigger("jump");

    }

    [PunRPC]
    void RPC_JumpAnim()
    {
        JumpAnim();
    }

    // for smoother movement and jump, Mostly AI tuned
    void FixedUpdate()
    {

        Vector2 force = new Vector2(moveInput * acceleration, 0f);
        rb.AddForce(force);


        if (Mathf.Abs(rb.linearVelocity.x) > moveSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * moveSpeed, rb.linearVelocity.y);
        }


        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }


        if (isDashing)
        {
            float direction = transform.localScale.x > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }


    }
    public void TakeDamage(int damage)
    {

        if (damageImmune == true) return;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }


        photonView.RPC("UpdateHealthRPC", RpcTarget.AllBuffered, health);
    }


    [PunRPC]
    void UpdateHealthRPC(int newHealth)
    {
        health = newHealth;
    }

    [PunRPC]
    public void RPC_TakeDamage(int damage)
    {
        TakeDamage(damage);
    }

    void Die()
    {


        photonView.RPC("RPC_Die", RpcTarget.AllBuffered);

        Stealth(false);
        if (photonView.IsMine && stealthPanel != null)
        {
            stealthPanel.SetActive(false);
        }
        photonView.RPC("RPC_Stealth", RpcTarget.Others, false);
        inStealth = false;
        StopCoroutine(StealthTakeDamage());


        if (lives <= 0)
        {
            gameover = true;
            isDead = true;
            lives = 0;
            health = 0;

        }
        else if (lives > 0)
        {
            damageImmune = true;
            lives--;
            photonView.RPC("RPC_StartCooldown", RpcTarget.AllBuffered);
        }
    }


    [PunRPC]
    void RPC_Die()
    {

        muzz.enabled = false;

        photonView.RPC("RPC_SetGlowOff", RpcTarget.AllBuffered);
        isDead = true;
        animator.SetTrigger("death");
        glow.SetTrigger("death");

    }

    // when die and revive, show a shining prefab that is a particle system
    [PunRPC]
    void RPC_BeamLight()
    {
        StartCoroutine(BeamLight());
    }
    private IEnumerator BeamLight()
    {
        beamalight.SetActive(true);
        ParticleSystem ps = beamalight.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(3f);
        beamalight.SetActive(false);
    }

    // for revive visual sequence
    private IEnumerator Cooldown()
    {

        yield return new WaitForSeconds(1.5f);
        photonView.RPC("RPC_BeamLight", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(3f);
        photonView.RPC("RPC_SetGlowOn", RpcTarget.AllBuffered);




        isDead = false;
        health = 100;
        damageImmune = false;
    }

    [PunRPC]
    public void RPC_StartCooldown()
    {
        StartCoroutine(Cooldown());
    }


    // for turning wing off, wing is on when the status is not jumpable, pseudo state for in air
    [PunRPC]
    public void RPC_wingonoff()
    {

        StartCoroutine(DelayWingOff(2f));

    }


    private IEnumerator DelayWingOff(float delay)
    {
        wing.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        wing.gameObject.SetActive(false);
    }



    public void WallslideOn()
    {
        photonView.RPC("RPC_wallslideOn", RpcTarget.AllBuffered);
    }

    public void WallslideOff()
    {
        photonView.RPC("RPC_wallslideOff", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_wallslideOn()
    {
        animator.SetBool("wallslide", true);
        glow.SetBool("wallslide", true);
    }

    [PunRPC]
    public void RPC_wallslideOff()
    {
        animator.SetBool("wallslide", false);
        glow.SetBool("wallslide", false);
    }

    public void SetGlowOff()
    {
        if (targetMaterial.name == "NO GLOW")
        {
            return;
        }
        targetMaterial.SetColor("_GlowColor", baseGlowColor * 0f);
        muzz.enabled = false;
    }

    public void SetGlowOn()
    {
        if (targetMaterial.name == "NO GLOW")
        {
            return;
        }
        targetMaterial.SetColor("_GlowColor", baseGlowColor * 1f);
    }



    [PunRPC]
    public void RPC_SetGlowOn()
    {
        SetGlowOn();
    }


    [PunRPC]
    public void RPC_SetGlowOff()
    {
        SetGlowOff();
    }





    // flips the transform positions
    public void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    [PunRPC]
    public void RPC_Flip()
    {
        Flip();
    }


    void Stealth(bool isInvisible)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.name == "muzzflash")
            {
                continue;
            }
            renderer.enabled = !isInvisible;
        }


    }

    [PunRPC]
    void RPC_Stealth(bool isInvisible)
    {
        if (!photonView.IsMine)
        {
            Stealth(isInvisible);
        }
    }

    IEnumerator StealthTakeDamage()
    {

        while (inStealth == true)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, 3);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ShowLose()
    {
        Time.timeScale = 0f;
        losepanel.SetActive(true);

    }

    public void ShowWin()
    {
        Time.timeScale = 0f;
        winpanel.SetActive(true);

    }

    public void HideLose()
    {
        Time.timeScale = 1f;
        losepanel.SetActive(false);
        UserSession.Instance.PvPLose++;
    }

    public void HideWin()
    {
        Time.timeScale = 1f;
        winpanel.SetActive(false);
        UserSession.Instance.PvPWin++;

    }

    public void LeaveRoom()
    {
        StartCoroutine(LeaveRoutine());

    }

    private IEnumerator LeaveRoutine()
    {
        yield return UserSession.Instance.SaveDataToFireStore();

        SceneManager.LoadScene("Lobby");
    }
}
