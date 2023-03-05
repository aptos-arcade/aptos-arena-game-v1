using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerActions
    {

        private readonly PlayerScript _player;

        public PlayerActions(PlayerScript player)
        {
            _player = player;
        }

        public void Move(Transform transform)
        {
            float targetSpeed = _player.PlayerStats.Direction.x * _player.PlayerStats.Speed;
            float speedDiff = targetSpeed - _player.PlayerComponents.RigidBody.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _player.PlayerStats.Acceleration : _player.PlayerStats.Deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, _player.PlayerStats.VelPower) * Mathf.Sign(speedDiff);
            _player.PlayerComponents.RigidBody.AddForce(movement * Vector2.right);

            if(_player.PlayerStats.Direction.x != 0)
            {
                int direction = _player.PlayerStats.Direction.x < 0 ? -1 : 1;
                transform.localScale = new Vector3(direction, 1, 1);
                _player.PlayerReferences.PlayerCanvas.transform.localScale = new Vector3(direction, 1, 1);
                _player.PlayerComponents.Animator.TryPlayAnimation("Body_Walk");
                _player.PlayerComponents.Animator.TryPlayAnimation("Legs_Walk");
            }
            else if(_player.PlayerComponents.RigidBody.velocity.magnitude < 0.01f)
            {
                _player.PlayerComponents.Animator.TryPlayAnimation("Body_Idle");
                _player.PlayerComponents.Animator.TryPlayAnimation("Legs_Idle");
            }
        }

        public void Jump()
        {
            if (_player.PlayerUtilities.IsGrounded())
            {
                _player.PlayerComponents.RigidBody.AddForce(new Vector2(0, _player.PlayerStats.JumpForce));
                _player.PlayerComponents.Animator.TryPlayAnimation("Legs_Jump");
                _player.PlayerComponents.Animator.TryPlayAnimation("Body_Jump");
            }
            else if(_player.PlayerStats.CanDoubleJump)
            {
                _player.PlayerComponents.RigidBody.velocity = new Vector2(_player.PlayerComponents.RigidBody.velocity.x, 0);
                _player.PlayerComponents.RigidBody.AddForce(new Vector2(0, _player.PlayerStats.DoubleJumpForce));
                _player.PlayerStats.CanDoubleJump = false;
            }
        }

        public void Attack()
        {
            _player.PlayerComponents.Animator.TryPlayAnimation("Legs_Attack");
            _player.PlayerComponents.Animator.TryPlayAnimation("Body_Attack");
        }

        public void TrySwapWeapon(Commands.Weapon weapon)
        {
            _player.PlayerStats.Weapon = weapon;
            _player.PlayerComponents.Animator.SetWeapon((int)_player.PlayerStats.Weapon);
            SwapWeapon();
        }

        private void SwapWeapon()
        {
            foreach (var weapon in _player.PlayerReferences.WeaponObjects)
            {
                if(weapon.activeSelf) weapon.GetComponent<PhotonView>().RPC("UnEquip", RpcTarget.AllBuffered);
            }

            _player.PlayerReferences.WeaponObjects[(int)_player.PlayerStats.Weapon].GetComponent<PhotonView>().RPC("Equip", RpcTarget.AllBuffered);
        }

        public void Drop()
        {
            _player.PlayerComponents.FootCollider.enabled = false;
            _player.StartCoroutine(ResetFeet());
        }

        IEnumerator ResetFeet()
        {
            yield return new WaitForSeconds(0.2f);
            _player.PlayerComponents.FootCollider.enabled = true;
        }

        public void Shoot(string animation)
        {
            if(animation == "Shoot" && _player.photonView.IsMine)
            {
                GameObject projectile = PhotonNetwork.Instantiate(_player.PlayerReferences.ProjectilePrefab.name, _player.PlayerReferences.GunBarrel.position, Quaternion.identity);
                Vector2 direction = new Vector2(_player.transform.localScale.x, 0);
                projectile.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, direction);
            }
        }
    }
}
