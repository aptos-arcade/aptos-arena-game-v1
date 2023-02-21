using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class Cowboy : MonoBehaviourPun
{

    public GameObject playerCamera;

    public SpriteRenderer sprite;

    public Animator anim;

    public float movementSpeed = 5;

    public PhotonView pv;

    private bool canMove = true;

    public GameObject bulletPrefab;
    public GameObject bulletSpawnPointRight;
    public GameObject bulletSpawnPointLeft;

    public TMP_Text playerText;

    public bool inputsDisabled = false;

    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public float jumpForce;

    public string playerName;

    // Start is called before the first frame update
    void Awake()
    {
        if(photonView.IsMine)
        {
            GameManager.instance.localPlayer = this.gameObject;
            playerCamera.SetActive(true);
            playerCamera.transform.SetParent(null, false);
            playerText.text = "You: " + PhotonNetwork.NickName;
            playerText.color = Color.green;
            playerName = PhotonNetwork.NickName;
        }
        else
        {
            playerText.text = photonView.Owner.NickName;
            playerText.color = Color.red;
        };
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !inputsDisabled)
        {
            UpdateMovement();
            UpdateAnimation();
            CheckShoot();
        }
    }

    void UpdateMovement()
    {
        float horizontalVelocity = 0;
        float verticalVelocity = rb.velocity.y;
        if (canMove)
        {
            horizontalVelocity = Input.GetAxisRaw("Horizontal") * movementSpeed;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            verticalVelocity = jumpForce;
        }
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    void UpdateAnimation()
    {
        anim.SetBool("IsMoving", Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow));
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0 && !anim.GetBool("isShooting"))
        {
            playerCamera.GetComponent<CameraFollow2D>().offset = new Vector3(horizontalInput * 1.3f, 1.5f, 0);
            if (horizontalInput > 0) pv.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
            else pv.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }
    }

    void CheckShoot()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !anim.GetBool("IsMoving"))
        {
            canMove = false;
            Shoot();
        }
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            canMove = true;
            anim.SetBool("isShooting", false);
        }
    }

    private void Shoot()
    {
        if (sprite.flipX)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(bulletSpawnPointLeft.transform.position.x, bulletSpawnPointLeft.transform.position.y), Quaternion.identity, 0);
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
            bullet.GetComponent<PhotonView>().RPC("SetIsLeft", RpcTarget.AllBuffered);
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(bulletSpawnPointRight.transform.position.x, bulletSpawnPointRight.transform.position.y), Quaternion.identity, 0);
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
        }
        anim.SetBool("isShooting", true);
    }

    [PunRPC]
    void FlipSprite_Right()
    {
        sprite.flipX = false;
    }

    [PunRPC]
    void FlipSprite_Left()
    {
        sprite.flipX = true;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, bc.bounds.extents.y + 0.5f);
    }
}
