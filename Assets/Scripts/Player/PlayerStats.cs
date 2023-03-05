using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerStats
    {

        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public bool CanDoubleJump { get; set; } = true;
        public bool CanMove { get; set; } = true;

        [SerializeField] private float jumpForce;
        public float JumpForce => jumpForce;

        [SerializeField] private float doubleJumpForce;
        public float DoubleJumpForce => doubleJumpForce;

        [SerializeField] private float walkSpeed;
        public float WalkSpeed => walkSpeed;

        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;

        [SerializeField] private float deceleration;
        public float Deceleration => deceleration;

        [SerializeField] private float velPower;
        public float VelPower => velPower;

        [SerializeField] private float knockBackPower;
        public float KnockBackPower => knockBackPower;


        private Commands.Weapon _weapon;
        public Commands.Weapon Weapon { get => _weapon; set => _weapon = value; }

        private float _damageMultiplier = 1;
        public float DamageMultiplier { get => _damageMultiplier; set => _damageMultiplier = value; }

        private string _playerName;
        public string PlayerName { get => _playerName; set => _playerName = value; }

    }
}