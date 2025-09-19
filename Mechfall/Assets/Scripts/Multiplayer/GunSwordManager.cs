using UnityEngine;
using System.Collections;
using Photon.Pun;

// For managing the gun and sword functions of the player in PVP
// While it may seem to collide with Te One's development features, this script being PVP specific required seperate development
public class GunSwordManager : MonoBehaviour
{
    public GameObject laserPrefab;

    public Transform shootingPoint;
    private PlayerStatus playerStatus;
    public Material playerMaterial;


    public float laserSpeed = 20f;
    public float laserLifetime = 5f;


    public GameObject objectToAppear;
    public GameObject swordHitBoxSprite;

    public int bullets = 10;
    private bool isFacingRight;


    public float laserCooldown = 1f;
    public float swordCooldown = 0.35f;
    private float nextLaserTime = 0f;
    private float nextSwordTime = 0f;

    private PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerStatus = GetComponent<PlayerStatus>();
    }
    void Update()
    {
        if (!photonView.IsMine) return;
        if (playerStatus.isDead) return;

        isFacingRight = playerStatus.isFacingRight;

        if (bullets > 0 && playerStatus.isDead == false && Input.GetKeyDown(KeyCode.S) && Time.time >= nextLaserTime)
        {
            playerStatus.animator.SetTrigger("attack2");
            playerStatus.glow.SetTrigger("attack2");
            nextLaserTime = Time.time + laserCooldown;
            StartCoroutine(ShootLaser());
            bullets--;
        }


        if (playerStatus.isDead == false && Input.GetKeyDown(KeyCode.A) && Time.time >= nextSwordTime)
        {
            playerStatus.animator.SetTrigger("attack");
            playerStatus.glow.SetTrigger("attack");
            nextSwordTime = Time.time + swordCooldown;
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator ShowAndHideObject()
    {
        photonView.RPC("ShowMuzzleFlashRPC", RpcTarget.All);

        yield return new WaitForSeconds(0.35f);

        photonView.RPC("HideMuzzleFlashRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ShowMuzzleFlashRPC()
    {
        SpriteRenderer sr = objectToAppear.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = true;
        }
    }

    [PunRPC]
    private void HideMuzzleFlashRPC()
    {
        SpriteRenderer sr = objectToAppear.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }
    }


    private IEnumerator ShootLaser()
    {


        StartCoroutine(ShowAndHideObject());

        GameObject laser = PhotonNetwork.Instantiate(laserPrefab.name, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        SpriteRenderer laserRenderer = laser.GetComponent<SpriteRenderer>();
        if (laserRenderer != null && playerMaterial != null)
        {
            laserRenderer.material = playerMaterial;
        }
        if (laserRb != null)
        {
            Vector2 direction = shootingPoint.right.normalized;
            float leftright = isFacingRight ? 1f : -1f;
            laserRb.linearVelocity = direction * leftright * laserSpeed;
        }

        yield return new WaitForSeconds(laserLifetime);
        if (laser != null)
        {
            PhotonNetwork.Destroy(laser);
        }
    }



    private IEnumerator SwingSword()
    {
        yield return new WaitForSeconds(0.15f);
        photonView.RPC("ActivateSwordRPC", RpcTarget.All);

        yield return new WaitForSeconds(0.35f);

        photonView.RPC("DeactivateSwordRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ActivateSwordRPC()
    {
        swordHitBoxSprite.SetActive(true);


        Renderer swordRenderer = swordHitBoxSprite.GetComponent<Renderer>();
        if (swordRenderer != null && playerMaterial != null)
        {
            swordRenderer.material = playerMaterial;
        }
    }

    [PunRPC]
    private void DeactivateSwordRPC()
    {
        swordHitBoxSprite.SetActive(false);
    }




}