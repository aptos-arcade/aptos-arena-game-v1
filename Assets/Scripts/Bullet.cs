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
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        PhotonView collisionPhotonView = collision.gameObject.GetComponent<PhotonView>();
        if (collisionPhotonView != null && (!collisionPhotonView.IsMine || collisionPhotonView.IsRoomView))
        {
            if(collisionPhotonView.CompareTag("Player"))
            {
                collisionPhotonView.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);
                collisionPhotonView.GetComponent<HurtEffect>().OnHit();

                if(collisionPhotonView.GetComponent<Health>().health <= 0)
                {
                    Player killedPlayer = collisionPhotonView.Owner;
                    collisionPhotonView.RPC("KilledBy", killedPlayer, killerName);
                    collisionPhotonView.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, killedPlayer.NickName);
                }
            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }
    }
}
