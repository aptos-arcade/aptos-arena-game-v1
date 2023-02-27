using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCommand : Command
{

    private PlayerScript player;

    public DropCommand(PlayerScript player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyDown()
    {
        if (player.PlayerUtilities.IsOnPlatform() && !player.PlayerUtilities.IsOnGround())
        {
            player.PlayerActions.Drop();
        }
    }
}
