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

    public bool isGrounded = false;

    private Rigidbody2D rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !inputsDisabled)
        {
            CheckInputs();
        }
    }

    private void CheckInputs()
    {
        if(canMove)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movementSpeed * Time.deltaTime * movement;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("IsMoving", true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.RightShift) && !anim.GetBool("IsMoving"))
        {
            Shoot();
        }
        else if(Input.GetKeyUp(KeyCode.RightShift))
        {
            anim.SetBool("isShooting", false);
            canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && !anim.GetBool("isShooting"))
        {
            playerCamera.GetComponent<CameraFollow2D>().offset = new Vector3(1.3f, 1.5f, 0);
            pv.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
            
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && !anim.GetBool("isShooting"))
        {
            playerCamera.GetComponent<CameraFollow2D>().offset = new Vector3(-1.3f, 1.5f, 0);
            pv.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMoving", false);
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
        canMove = false;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(new Vector2(0, jumpForce));
    }
}
