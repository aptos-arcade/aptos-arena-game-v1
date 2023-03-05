using Player;
using UnityEngine;

namespace Commands
{
    public class DropCommand : Command
    {

        private readonly PlayerScript _player;

        public DropCommand(PlayerScript player, KeyCode key) : base(key)
        {
            _player = player;
        }

        public override void GetKeyDown()
        {
            if (_player.PlayerUtilities.IsOnPlatform() && !_player.PlayerUtilities.IsOnGround())
            {
                _player.PlayerActions.Drop();
            }
        }
    }
}
