using System.Collections;
using System.Collections.Generic;
using Commands;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerUtilities
    {
        private readonly PlayerScript _player;

        private readonly List<Command> _commands = new();

        public PlayerUtilities(PlayerScript player)
        {
            _player = player;
            _commands.Add(new JumpCommand(player, KeyCode.UpArrow));
            _commands.Add(new DropCommand(player, KeyCode.DownArrow));
            _commands.Add(new ShootCommand(player, KeyCode.Alpha1));
            _commands.Add(new MeleeCommand(player, KeyCode.Alpha2));
            // _commands.Add(new WeaponSwapCommand(player, Weapon.Gun, KeyCode.Alpha1));
            // _commands.Add(new WeaponSwapCommand(player, Weapon.Sword, KeyCode.Alpha2));
        }

        public void HandleInput()
        {
            if (!_player.PlayerComponents.PhotonView.IsMine || !_player.PlayerState.CanMove) return;
            _player.PlayerState.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            foreach (Command command in _commands)
            {
                if (Input.GetKeyDown(command.Key))
                {
                    command.GetKeyDown();
                }

                if (Input.GetKeyUp(command.Key))
                {
                    command.GetKeyUp();
                }

                if (Input.GetKey(command.Key))
                {
                    command.GetKey();
                }
            }
        }

        public bool IsGrounded()
        {
            return IsOnGround() || IsOnPlatform();
        }

        public bool IsOnGround()
        {
            return _player.PlayerComponents.RigidBody.IsTouchingLayers(_player.PlayerComponents.Ground.value);
        }

        public bool IsOnPlatform()
        {
            return _player.PlayerComponents.RigidBody.IsTouchingLayers(_player.PlayerComponents.Platform.value);
        }

        public void HandleAir()
        {
            if(IsFalling())
            {
                _player.PlayerComponents.Animator.TryPlayAnimation("Body_Fall");
                _player.PlayerComponents.Animator.TryPlayAnimation("Legs_Fall");
            }
            if(IsGrounded())
            {
                _player.PlayerState.CanDoubleJump = true;
            }
        }

        private bool IsFalling()
        {
            return _player.PlayerComponents.RigidBody.velocity.y < 0 && !IsGrounded();
        }

        public void HandleStrike(Vector2 direction, float knockBack, float damage)
        {
            _player.PlayerComponents.RigidBody.AddForce(direction.normalized * (knockBack * Mathf.Pow(_player.PlayerState.DamageMultiplier, _player.PlayerStats.KnockBackPower)));
            _player.PlayerState.DamageMultiplier += damage;
            _player.PlayerReferences.DamageDisplay.text = ((_player.PlayerState.DamageMultiplier - 1) * 100).ToString("F0") + "%";
            _player.StartCoroutine(_player.PlayerComponents.PlayerCamera.Shake(0.2f, 0.1f));
        }

        public void StartHurt()
        {
            _player.PlayerComponents.BodyCollider.enabled = false;
            foreach (SpriteRenderer renderer in _player.PlayerComponents.PlayerSprites)
            {
                renderer.color = Color.red;
            }
        }

        public void EndHurt()
        {
            _player.PlayerComponents.BodyCollider.enabled = true;
            foreach (SpriteRenderer renderer in _player.PlayerComponents.PlayerSprites)
            {
                renderer.color = Color.white;
            }
        }
    
        public IEnumerator RespawnCoroutine()
        {
            var spawnPosition = new Vector2(Random.Range(-8, 8), 7);
            _player.photonView.RPC("RelocatePlayer", RpcTarget.AllBuffered, spawnPosition);
            GameObject portal = PhotonNetwork.Instantiate(
                _player.PlayerReferences.Portal.name,
                spawnPosition,
                Quaternion.identity
            );
            yield return new WaitForSeconds(2.5f);
            PhotonNetwork.Destroy(portal);
            _player.photonView.RPC("OnRevive", RpcTarget.AllBuffered);
        }

        public void GetSpriteRenderers()
        {
            foreach (Transform transform in _player.PlayerReferences.PlayerMesh.transform)
            {
                _player.PlayerComponents.PlayerSprites.Add(transform.GetComponent<SpriteRenderer>());
            }
        }

        public void HidePlayer()
        {
            _player.PlayerComponents.PlayerSprites.ForEach(sprite => sprite.enabled = false);
        }
        
        public void ShowPlayer()
        {
            _player.PlayerComponents.PlayerSprites.ForEach(sprite => sprite.enabled = true);
        }
    }
}
