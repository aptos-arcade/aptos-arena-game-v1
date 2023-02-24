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
        commands.Add(new AttackCommand(player, KeyCode.RightShift));
        commands.Add(new WeaponSwapCommand(player, WEAPON.FISTS, KeyCode.Alpha1));
        commands.Add(new WeaponSwapCommand(player, WEAPON.GUN, KeyCode.Alpha2));
        commands.Add(new WeaponSwapCommand(player, WEAPON.SWORD, KeyCode.Alpha3));
    }

    public void HandleInput()
    {
        if(player.PlayerComponents.PhotonView.IsMine)
        {
            player.PlayerStats.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), player.PlayerComponents.RigidBody.velocity.y);

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
        RaycastHit2D hit = Physics2D.Raycast(
            player.transform.position,
            Vector2.down,
            player.PlayerComponents.Collider.bounds.extents.y + 0.2f,
            player.PlayerComponents.Ground
        );
        return hit.collider != null;
    }

    public void HandleAir()
    {
        if(IsFalling())
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Fall");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Fall");
        }
    }

    public bool IsFalling()
    {
        return player.PlayerComponents.RigidBody.velocity.y < 0 && !IsGrounded();
    }
}
