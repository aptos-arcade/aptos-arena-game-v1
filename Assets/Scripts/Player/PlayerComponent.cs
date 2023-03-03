using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class PlayerComponent
{
    [SerializeField] private Rigidbody2D rigidBody;
    public Rigidbody2D RigidBody => rigidBody;

    [SerializeField] private BoxCollider2D footCollider;
    public Collider2D FootCollider => footCollider;


    [SerializeField] private PolygonCollider2D bodyCollider;
    public PolygonCollider2D BodyCollider { get => bodyCollider; set => bodyCollider = value; }


    [SerializeField] private AnyStateAnimator animator;
    public AnyStateAnimator Animator => animator;


    [SerializeField] private LayerMask ground;
    public LayerMask Ground => ground;


    [SerializeField] private LayerMask platform;
    public LayerMask Platform => platform;

    [SerializeField] private PhotonView photonView;
    public PhotonView PhotonView { get => photonView; set => photonView = value; }


    [SerializeField] private CameraFollow2D playerCamera;
    public CameraFollow2D PlayerCamera => playerCamera;
    
    private List<SpriteRenderer> _playerSprites = new();
    public List<SpriteRenderer> PlayerSprites { get => _playerSprites; set => _playerSprites = value; }
}
