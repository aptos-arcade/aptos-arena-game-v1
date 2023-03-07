using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Sword : Weapon
    {
        
        [SerializeField] private PlayerScript owner;

        [SerializeField] private float damage;

        [SerializeField] private float knockBackForce;

        [SerializeField] private Collider2D swordCollider;

        [SerializeField] private float launchX;

        [SerializeField] private float launchY;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine) return;

            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript == null || playerScript.photonView.IsMine) return;
            Vector2 hitDirection = new Vector2(playerScript.transform.position.x > transform.position.x ? launchX : -launchX, launchY);
            photonView.RPC("Disable", RpcTarget.AllBuffered);
            playerScript.photonView.RPC(
                "OnStrike",
                RpcTarget.AllBuffered,
                hitDirection,
                knockBackForce * owner.PlayerStats.Strength,
                damage
            );
        }

        [PunRPC]
        public void Disable()
        {
            swordCollider.enabled = false;
        }
    }
}
