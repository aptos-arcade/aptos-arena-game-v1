using Characters;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Striker: MonoBehaviourPun
    {
        [SerializeField] private float damage;
        public float Damage => damage;
        
        [SerializeField] private float knockBackForce;
        public float KnockBackForce => knockBackForce;
        
        [SerializeField] private Vector2 knockBackDirection;
        protected Vector2 KnockBackDirection => knockBackDirection;

        public Vector2 KnockBackSignedDirection { get; set; }
        
        [SerializeField] private GameObject hitEffect;

        private void Start()
        {
            KnockBackSignedDirection = KnockBackDirection;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var colPlayer = col.gameObject.GetComponent<PlayerScript>();
            if (!photonView.IsMine || colPlayer.photonView.IsMine || IsSameTeam(colPlayer)) return;
            PhotonNetwork.Instantiate(hitEffect.name, col.transform.position, Quaternion.identity);
            colPlayer.PlayerUtilities.StrikerCollision(this);
            OnStrike();
        }

        private bool IsSameTeam(MonoBehaviourPun colPlayer)
        {
            return (CharactersEnum)colPlayer.photonView.Owner.CustomProperties["Character"] ==
                (CharactersEnum)photonView.Owner.CustomProperties["Character"];
        }

        protected virtual void OnStrike() {}
    }
}