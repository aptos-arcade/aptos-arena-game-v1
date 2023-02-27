using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{

    public Vector2 Direction { get; set; }

    public float Speed { get; set; }

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float doubleJumpForce;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float decceleration;

    [SerializeField]
    private float velPower;

    [SerializeField]
    private float knockbackPower;

    [SerializeField]
    private bool canMove = true;

    private WEAPON weapon;

    private float damageMultiplier = 1;

    private string playerName;

    public float WalkSpeed { get => walkSpeed; }
    public float JumpForce { get => jumpForce; }
    public float DoubleJumpForce { get => doubleJumpForce; }
    public WEAPON Weapon { get => weapon; set => weapon = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public float Acceleration { get => acceleration; }
    public float Decceleration { get => decceleration; }
    public float VelPower { get => velPower; }
    public bool CanDoubleJump { get; set; } = true;
    public bool CanMove { get => canMove; set => canMove = value; }
    public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
    public float KnockbackPower { get => knockbackPower; }
}