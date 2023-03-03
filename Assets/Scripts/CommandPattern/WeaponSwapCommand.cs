using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum WEAPON { GUN = 0, SWORD}

public class WeaponSwapCommand : Command
{

    private PlayerScript player;

    private WEAPON weapon;

    public WeaponSwapCommand(PlayerScript player, WEAPON weapon, KeyCode key) : base(key)
    {
        this.player = player;
        this.weapon = weapon;
    }

    public override void GetKeyDown()
    {
        player.PlayerActions.TrySwapWeapon(weapon);
    }
}
