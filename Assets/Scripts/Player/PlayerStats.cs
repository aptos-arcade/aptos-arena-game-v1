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
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;


    private WEAPON weapon;

    private float health = 1;

    private string playerName;

    public float WalkSpeed { get => walkSpeed; }
    public float JumpForce { get => jumpForce; }
    public WEAPON Weapon { get => weapon; set => weapon = value; }
    public float Health { get => health; set => health = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
}
