using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{

    private PlayerScript player;

    public AttackCommand(PlayerScript player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyDown()
    {
        player.PlayerActions.Attack();
    }
}
