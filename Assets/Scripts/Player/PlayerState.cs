using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        public Vector2 Direction { get; set; }
        
        public bool CanDoubleJump { get; set; } = true;
        
        public bool CanMove { get; set; } = true;
        
        public Commands.Weapon Weapon { get; set; }
        
        public float DamageMultiplier { get; set; } = 1;

        public string PlayerName { get; set; }

        public int Lives { get; set; } = 3;
    }
}