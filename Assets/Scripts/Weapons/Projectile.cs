using System.Collections;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeReference]
        private float speed;
        
        [SerializeField]
        private float destroyTime;

        [SerializeField]
        private float damage;

        [SerializeField]
        private float knockBackForce;

        private Vector2 _direction;
        
        private void Start()
        {
            StartCoroutine(DestroyBullet());
        }


        // Update is called once per frame
        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * _direction);
        }

        [PunRPC]
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        [PunRPC]
        public void Destroy()
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(destroyTime);
            photonView.RPC("Destroy", RpcTarget.AllBuffered);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine) return;

            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                Vector2 collisionDirection = playerScript.transform.position - transform.position;
                playerScript.photonView.RPC(
                    "OnStrike",
                    RpcTarget.AllBuffered,
                    collisionDirection,
                    knockBackForce,
                    damage
                );
                photonView.RPC("Destroy", RpcTarget.AllBuffered);
            }
        }
    }
}
