using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilities
{

    PlayerScript player;

    private List<Command> commands = new List<Command>();

    public PlayerUtilities(PlayerScript player)
    {
        this.player = player;
        commands.Add(new JumpCommand(player, KeyCode.Space));
        commands.Add(new DropCommand(player, KeyCode.S));
        commands.Add(new DropCommand(player, KeyCode.DownArrow));
        commands.Add(new AttackCommand(player, KeyCode.RightShift));
        commands.Add(new WeaponSwapCommand(player, WEAPON.GUN, KeyCode.Alpha1));
        commands.Add(new WeaponSwapCommand(player, WEAPON.SWORD, KeyCode.Alpha2));
    }

    public void HandleInput()
    {
        if(player.PlayerComponents.PhotonView.IsMine && player.PlayerStats.CanMove)
        {
            player.PlayerStats.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            foreach (Command command in commands)
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
    }

    public bool IsGrounded()
    {
        return IsOnGround() || IsOnPlatform();
    }

    public bool IsOnGround()
    {
        return player.PlayerComponents.RigidBody.IsTouchingLayers(player.PlayerComponents.Ground.value);
    }

    public bool IsOnPlatform()
    {
        return player.PlayerComponents.RigidBody.IsTouchingLayers(player.PlayerComponents.Platform.value);
    }

    public void HandleAir()
    {
        if(IsFalling())
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Fall");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Fall");
        }
        if(IsGrounded())
        {
            player.PlayerStats.CanDoubleJump = true;
        }
    }

    private bool IsFalling()
    {
        return player.PlayerComponents.RigidBody.velocity.y < 0 && !IsGrounded();
    }

    public void HandleStrike(Vector2 direction, float knockback, float damage)
    {
        StartHurt();
        player.PlayerComponents.RigidBody.AddForce(direction.normalized * (knockback * Mathf.Pow(player.PlayerStats.DamageMultiplier, player.PlayerStats.KnockbackPower)));
        player.PlayerStats.DamageMultiplier += damage;
        player.PlayerReferences.DamageDisplay.text = ((player.PlayerStats.DamageMultiplier - 1) * 100).ToString("F0") + "%";
        player.StartCoroutine(player.PlayerComponents.PlayerCamera.Shake(0.2f, 0.1f));
    }

    private void StartHurt()
    {
        player.PlayerComponents.BodyCollider.enabled = false;
        foreach (SpriteRenderer renderer in player.PlayerComponents.PlayerSprites)
        {
            renderer.color = Color.red;
        }
    }

    public void EndHurt()
    {
        player.PlayerComponents.BodyCollider.enabled = true;
        foreach (SpriteRenderer renderer in player.PlayerComponents.PlayerSprites)
        {
            renderer.color = Color.white;
        }
    }

    public void GetSpriteRenderers()
    {
        foreach (Transform transform in player.PlayerReferences.PlayerSpriteTransform.transform)
        {
            player.PlayerComponents.PlayerSprites.Add(transform.GetComponent<SpriteRenderer>());
        }
    }
}
