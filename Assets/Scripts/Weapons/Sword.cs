using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : Weapon
{

    [SerializeField]
    private float damage;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private Collider2D _collider;

    [SerializeField]
    private float launchX;

    [SerializeField]
    private float launchY;


    public float Damage { get => damage; }
    public float KnockbackForce { get => knockbackForce; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
        if (playerScript != null && !playerScript.photonView.IsMine)
        {
            Vector2 hitDirection = new Vector2(playerScript.transform.position.x > transform.position.x ? launchX : -launchX, launchY);
            photonView.RPC("Disable", RpcTarget.AllBuffered);
            playerScript.photonView.RPC(
                "OnStrike",
                RpcTarget.AllBuffered,
                hitDirection,
                knockbackForce,
                damage
            );
        }
    }

    [PunRPC]
    public void Disable()
    {
        _collider.enabled = false;
    }
}
