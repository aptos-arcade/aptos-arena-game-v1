using Player;
using UnityEngine;

namespace Commands
{
    public class JumpCommand : Command
    {

        private readonly PlayerScript _player;

        public JumpCommand(PlayerScript player, KeyCode key) : base(key)
        {
            _player = player;
        }

        public override void GetKeyDown()
        {
            _player.PlayerActions.Jump();
        }
    }
}
