using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

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

    // Start is called before the first frame update
    void Awake()
    {
        if(photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
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
            pv.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
            anim.SetBool("IsMoving", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && !anim.GetBool("isShooting"))
        {
            pv.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
            anim.SetBool("IsMoving", true);
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
            bullet.GetComponent<PhotonView>().RPC("SetIsLeft", RpcTarget.AllBuffered);
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(bulletSpawnPointRight.transform.position.x, bulletSpawnPointRight.transform.position.y), Quaternion.identity, 0);
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
}
