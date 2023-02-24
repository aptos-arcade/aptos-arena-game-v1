using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : Command
{

    private PlayerScript player;

    public JumpCommand(PlayerScript player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyDown()
    {
        player.PlayerActions.Jump();
    }
}
