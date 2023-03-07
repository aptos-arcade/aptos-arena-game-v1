using Player;
using UnityEngine;

namespace Commands
{
    public class MeleeCommand : Command
    {
        private readonly PlayerScript _player;

        public MeleeCommand(PlayerScript player, KeyCode key) : base(key)
        {
            _player = player;
        }

        public override void GetKeyDown()
        {
            _player.PlayerActions.TrySwapWeapon(Weapon.Sword);
            _player.PlayerActions.Attack();
        }
    }
}
