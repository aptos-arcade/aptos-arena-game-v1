using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerRPCs : MonoBehaviourPun
    {
        private PlayerScript _player;
    
        // Start is called before the first frame update
        private void Start()
        {
            _player = GetComponent<PlayerScript>();
        }

        [PunRPC]
        public void RelocatePlayer(Vector2 newPosition)
        {
            _player.transform.position = newPosition;
        }

        [PunRPC]
        public void OnDeath()
        {
            _player.PlayerUtilities.HidePlayer();
            _player.PlayerReferences.PlayerCanvas.SetActive(false);
            _player.PlayerReferences.WeaponObjects[(int)_player.PlayerState.Weapon].GetComponent<PhotonView>().RPC("UnEquip", RpcTarget.AllBuffered);

            _player.PlayerComponents.RigidBody.velocity = Vector2.zero;
            _player.PlayerComponents.RigidBody.gravityScale = 0;
            _player.PlayerComponents.FootCollider.enabled = false;

            _player.PlayerState.Direction = Vector2.zero;
            _player.PlayerState.CanMove = false;
            _player.PlayerState.Lives--;
            _player.PlayerReferences.PlayerLives.GetChild(_player.PlayerState.Lives).GetComponent<PhotonView>().RPC("Hide", RpcTarget.AllBuffered);
            
        }

        [PunRPC]
        public void OnRevive()
        {
            _player.PlayerUtilities.ShowPlayer();
            _player.PlayerReferences.PlayerCanvas.SetActive(true);
            _player.PlayerReferences.WeaponObjects[(int)_player.PlayerState.Weapon].GetComponent<PhotonView>().RPC("Equip", RpcTarget.AllBuffered);

            _player.PlayerComponents.RigidBody.gravityScale = 5;
            _player.PlayerComponents.FootCollider.enabled = true;

            _player.PlayerState.CanMove = true;
            _player.PlayerState.DamageMultiplier = 1;

            _player.PlayerReferences.DamageDisplay.text = "0%";
        }

        [PunRPC]
        public void OnStrike(Vector2 direction, float knockBack, float damage)
        {
            _player.PlayerUtilities.HandleStrike(direction, knockBack, damage);
            StartCoroutine(HurtCoroutine());
        }
    
        IEnumerator HurtCoroutine()
        {
            _player.PlayerComponents.PhotonView.RPC("StartHurt", RpcTarget.AllBuffered);
            yield return new WaitForSeconds(0.25f);
            _player.PlayerComponents.PhotonView.RPC("EndHurt", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void StartHurt()
        {
            _player.PlayerUtilities.StartHurt();
        }

        [PunRPC]
        public void EndHurt()
        {
            _player.PlayerUtilities.EndHurt();
        }
    }
}
