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
    private Collider2D collider;

    [SerializeField]
    private AnyStateAnimator animator;

    [SerializeField]
    private LayerMask ground;

    [SerializeField]
    private PhotonView photonView;

    [SerializeField]
    private SpriteRenderer renderer;

    public Rigidbody2D RigidBody { get => rigidBody; }

    public AnyStateAnimator Animator { get => animator; }

    public LayerMask Ground { get => ground; }

    public Collider2D Collider { get => collider; }

    public PhotonView PhotonView { get => photonView; set => photonView = value; }

    public SpriteRenderer Renderer { get => renderer; set => renderer = value; }
}
