using Player;
using UnityEngine;

namespace Commands
{
    public enum Weapon { Gun = 0, Sword}

    public class WeaponSwapCommand : Command
    {

        private readonly PlayerScript _player;

        private readonly Weapon _weapon;

        public WeaponSwapCommand(PlayerScript player, Weapon weapon, KeyCode key) : base(key)
        {
            _player = player;
            _weapon = weapon;
        }

        public override void GetKeyDown()
        {
            _player.PlayerActions.TrySwapWeapon(_weapon);
        }
    }
}