using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class PlayerComponent
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private BoxCollider2D footCollider;

    [SerializeField]
    private PolygonCollider2D bodyCollider;

    [SerializeField]
    private AnyStateAnimator animator;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private LayerMask platform;

    [SerializeField]
    private PhotonView photonView;

    [SerializeField]
    private SpriteRenderer renderer;

    public Rigidbody2D RigidBody { get => rigidBody; }

    public AnyStateAnimator Animator { get => animator; }

    public LayerMask Ground { get => ground; }

    public Collider2D FootCollider { get => footCollider; }

    public PolygonCollider2D BodyCollider { get => bodyCollider; set => bodyCollider = value; }

    public PhotonView PhotonView { get => photonView; set => photonView = value; }

    public SpriteRenderer Renderer { get => renderer; set => renderer = value; }

    public LayerMask Platform { get => platform; }
}
