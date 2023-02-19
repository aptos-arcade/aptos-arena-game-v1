using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public float speed = 8;

    public float destroyTime = 2;

    public bool isLeft;

    public float bulletDamage = 0.3f;

    public string killerName;
    public GameObject localPlayerObj;

    private void Start()
    {
        if (photonView.IsMine)
        {
            killerName = localPlayerObj.GetComponent<Cowboy>().playerName;
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if(isLeft)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.left);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime * Vector2.right);
        }
    }

    [PunRPC]
    public void SetIsLeft()
    {
        isLeft = true;
    }
    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine || target.IsRoomView))
        {
            if(target.CompareTag("Player"))
            {
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);
                target.GetComponent<HurtEffect>().OnHit();

                if(target.GetComponent<Health>().health <= 0)
                {
                    Player killedPlayer = target.Owner;
                    target.RPC("KilledBy", killedPlayer, killerName);
                    target.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, killedPlayer.NickName);
                }
            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }
    }
}
