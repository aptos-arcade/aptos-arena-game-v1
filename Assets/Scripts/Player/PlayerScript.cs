using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun
{

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private PlayerComponent playerComponent;

    [SerializeField]
    private PlayerReferences playerReferences;

    private PlayerUtilities playerUtilities;

    private PlayerActions playerActions;

    private PlayerHealth playerHealth;

    public PlayerComponent PlayerComponents { get => playerComponent; }
    public PlayerStats PlayerStats { get => playerStats; }
    public PlayerActions PlayerActions { get => playerActions; }
    public PlayerUtilities PlayerUtilities { get => playerUtilities; }
    public PlayerReferences PlayerReferences { get => playerReferences; }
    public PlayerHealth PlayerHealth { get => playerHealth; }

    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.localPlayer = this.gameObject;

            playerReferences.PlayerCamera.SetActive(true);
            playerReferences.PlayerCamera.transform.SetParent(null, false);
            playerReferences.Nametag.text = PhotonNetwork.NickName;
            playerReferences.Nametag.color = new Color(0.6588235f, 0.8078431f, 1f);

        }
        else
        {
            Debug.Log("Not Mine");
            playerReferences.Nametag.text = photonView.Owner.NickName;
            playerReferences.Nametag.color = Color.red;
        };
    }


    // Start is called before the first frame update
    private void Start()
    {
        playerActions = new PlayerActions(this);
        playerUtilities = new PlayerUtilities(this);

        playerStats.Speed = playerStats.WalkSpeed;
        playerStats.PlayerName = PhotonNetwork.NickName;

        AnyStateAnimation[] animations = new AnyStateAnimation[]
        {
            new AnyStateAnimation(RIG.BODY, "Body_Idle", "Body_Attack"),
            new AnyStateAnimation(RIG.BODY, "Body_Walk", "Body_Attack", "Body_Jump"),
            new AnyStateAnimation(RIG.BODY, "Body_Jump"),
            new AnyStateAnimation(RIG.BODY, "Body_Fall"),
            new AnyStateAnimation(RIG.BODY, "Body_Attack"),

            new AnyStateAnimation(RIG.LEGS, "Legs_Idle", "Legs_Attack"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Walk", "Legs_Jump"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Jump"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Fall"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Attack")
        };

        playerComponent.Animator.AnimationTriggerEvent += PlayerActions.Shoot;
        playerComponent.Animator.AddAnimations(animations);
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerUtilities.HandleInput();
        PlayerUtilities.HandleAir();
        if(transform.position.y < -10)
        {
            transform.position = new Vector3(0, 0, 0);
            playerReferences.HealthBar.GetComponent<PlayerHealth>().OnDeath();
        }
    }

    void FixedUpdate()
    {
        PlayerActions.Move(transform);
    }

    [PunRPC]
    public void OnDeath()
    {
        playerActions.TrySwapWeapon(WEAPON.FISTS);

        playerComponent.RigidBody.velocity = Vector2.zero;
        playerComponent.RigidBody.gravityScale = 0;
        playerComponent.Collider.enabled = false;
        playerComponent.Renderer.enabled = false;

        playerReferences.PlayerCanvas.SetActive(false);
    }

    [PunRPC]
    public void OnRevive()
    {
        playerComponent.RigidBody.gravityScale = 5;
        playerComponent.Collider.enabled = true;
        playerComponent.Renderer.enabled = true;

        playerReferences.PlayerCanvas.SetActive(true);
    }
}
