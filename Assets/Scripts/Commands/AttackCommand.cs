using Player;
using UnityEngine;

namespace Commands
{
    public class AttackCommand : Command
    {

        private readonly PlayerScript _player;

        public AttackCommand(PlayerScript player, KeyCode key) : base(key)
        {
            _player = player;
        }

        public override void GetKeyDown()
        {
            _player.PlayerActions.Attack();
        }
    }
}
