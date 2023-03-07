using Player;
using UnityEngine;

namespace Commands
{
    public class ShootCommand : Command
    {

        private readonly PlayerScript _player;

        public ShootCommand(PlayerScript player, KeyCode key) : base(key)
        {
            _player = player;
        }

        public override void GetKeyDown()
        {
            _player.PlayerActions.TrySwapWeapon(Weapon.Gun);
            _player.PlayerActions.Attack();
        }
    }
}
