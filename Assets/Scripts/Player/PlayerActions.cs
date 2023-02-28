using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class PlayerActions
{

    private PlayerScript player;

    public PlayerActions(PlayerScript player)
    {
        this.player = player;
    }

    public void Move(Transform transform)
    {
        float targetSpeed = player.PlayerStats.Direction.x * player.PlayerStats.Speed;
        float speedDiff = targetSpeed - player.PlayerComponents.RigidBody.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player.PlayerStats.Acceleration : player.PlayerStats.Decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, player.PlayerStats.VelPower) * Mathf.Sign(speedDiff);
        player.PlayerComponents.RigidBody.AddForce(movement * Vector2.right);

        if(player.PlayerStats.Direction.x != 0)
        {
            int direction = player.PlayerStats.Direction.x < 0 ? -1 : 1;
            transform.localScale = new Vector3(direction, 1, 1);
            player.PlayerReferences.PlayerCanvas.transform.localScale = new Vector3(direction, 1, 1);
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Walk");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Walk");

        }
        else if(player.PlayerComponents.RigidBody.velocity.magnitude < 0.01f)
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Idle");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Idle");
        }
    }

    public void Jump()
    {
        if (player.PlayerUtilities.IsGrounded())
        {
            player.PlayerComponents.RigidBody.AddForce(new Vector2(0, player.PlayerStats.JumpForce));
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Jump");
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Jump");
        }
        else if(player.PlayerStats.CanDoubleJump)
        {
            player.PlayerComponents.RigidBody.velocity = new Vector2(player.PlayerComponents.RigidBody.velocity.x, 0);
            player.PlayerComponents.RigidBody.AddForce(new Vector2(0, player.PlayerStats.DoubleJumpForce));
            player.PlayerStats.CanDoubleJump = false;
        }
    }

    public void Attack()
    {
        player.PlayerComponents.Animator.TryPlayAnimation("Legs_Attack");
        player.PlayerComponents.Animator.TryPlayAnimation("Body_Attack");
    }

    public void TrySwapWeapon(WEAPON weapon)
    {
        player.PlayerStats.Weapon = weapon;
        player.PlayerComponents.Animator.SetWeapon((int)player.PlayerStats.Weapon);
        SwapWeapon();
    }

    public void SwapWeapon()
    {
        for (int i = 1; i < player.PlayerReferences.WeaponObjects.Length; i++)
        {
            if(player.PlayerReferences.WeaponObjects[i].activeSelf)
            {
                player.PlayerReferences.WeaponObjects[i].GetComponent<PhotonView>().RPC("Unequip", RpcTarget.AllBuffered);
            }
        }

        if(player.PlayerStats.Weapon > 0)
        {
            player.PlayerReferences.WeaponObjects[(int)player.PlayerStats.Weapon].GetComponent<PhotonView>().RPC("Equip", RpcTarget.AllBuffered);
        }
    }

    public void Drop()
    {
        player.PlayerComponents.FootCollider.enabled = false;
        player.StartCoroutine(ResetFeet());
    }

    IEnumerator ResetFeet()
    {
        yield return new WaitForSeconds(0.25f);
        player.PlayerComponents.FootCollider.enabled = true;
    }

    public void Shoot(string animation)
    {
        if(animation == "Shoot" && player.photonView.IsMine)
        {
            GameObject projectile = PhotonNetwork.Instantiate(player.PlayerReferences.ProjectilePrefab.name, player.PlayerReferences.GunBarrel.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().localPlayerObj = player.gameObject;
            Vector2 direction = new Vector2(player.transform.localScale.x, 0);
            projectile.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, direction);
        }
    }
}
