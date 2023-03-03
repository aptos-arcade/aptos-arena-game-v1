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
    
    public PlayerComponent PlayerComponents => playerComponent;
    public PlayerStats PlayerStats => playerStats;
    public PlayerActions PlayerActions => playerActions;
    public PlayerUtilities PlayerUtilities => playerUtilities;
    public PlayerReferences PlayerReferences => playerReferences;

    // Start is called before the first frame update
    private void Awake()
    {
        if (photonView.IsMine)
        {
            if(GameManager.instance)
            {
                GameManager.instance.localPlayer = this.gameObject;

                playerReferences.PlayerCamera.SetActive(true);
                playerReferences.PlayerCamera.transform.SetParent(null, false);
                playerReferences.Nametag.text = PhotonNetwork.NickName;
                playerReferences.Nametag.color = new Color(0.6588235f, 0.8078431f, 1f);
            }
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
        Debug.Log("Start");
        playerActions = new PlayerActions(this);
        playerUtilities = new PlayerUtilities(this);

        playerStats.Speed = playerStats.WalkSpeed;
        playerStats.PlayerName = PhotonNetwork.NickName;

        AnyStateAnimation[] animations = new AnyStateAnimation[]
        {
            new AnyStateAnimation(RIG.BODY, "Body_Idle", "Body_Attack"),
            new AnyStateAnimation(RIG.BODY, "Body_Walk", "Body_Attack", "Body_Jump"),
            new AnyStateAnimation(RIG.BODY, "Body_Jump"),
            new AnyStateAnimation(RIG.BODY, "Body_Fall", "Body_Attack"),
            new AnyStateAnimation(RIG.BODY, "Body_Attack"),

            new AnyStateAnimation(RIG.LEGS, "Legs_Idle", "Legs_Attack"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Walk", "Legs_Jump"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Jump"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Fall"),
            new AnyStateAnimation(RIG.LEGS, "Legs_Attack")
        };

        playerComponent.Animator.AnimationTriggerEvent += PlayerActions.Shoot;
        playerComponent.Animator.AddAnimations(animations);

        playerReferences.DamageDisplay.text = ((playerStats.DamageMultiplier - 1) * 100) + "%";
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerUtilities.HandleInput();
        PlayerUtilities.HandleAir();
        if (photonView.IsMine && playerStats.CanMove && (Mathf.Abs(transform.position.x) > 30 || Mathf.Abs(transform.position.y) > 16))
        {
            OnDeath();
        };
    }

    void FixedUpdate()
    {
        PlayerActions.Move(transform);
    }

    private void OnDeath()
    {
        GameObject explosion = PhotonNetwork.Instantiate(playerReferences.ExplosionPrefab.name, transform.position, Quaternion.identity);
        GameManager.instance.EnableRespawn();
        photonView.RPC("ShowDeath", RpcTarget.AllBuffered);
    }


    [PunRPC]
    public void ShowDeath()
    {
        playerActions.TrySwapWeapon(WEAPON.FISTS);

        playerComponent.RigidBody.velocity = Vector2.zero;
        playerComponent.RigidBody.gravityScale = 0;
        playerComponent.FootCollider.enabled = false;

        playerStats.Direction = Vector2.zero;
        playerStats.CanMove = false;

        playerReferences.PlayerCanvas.SetActive(false);
    }

    [PunRPC]
    public void OnRevive()
    {
        float randomPos = Random.Range(-4, 4);
        transform.localPosition = new Vector2(randomPos, 6);

        playerComponent.RigidBody.gravityScale = 5;
        playerComponent.FootCollider.enabled = true;

        playerStats.CanMove = true;
        playerStats.DamageMultiplier = 1;

        playerReferences.DamageDisplay.text = "0%";
        playerReferences.PlayerCanvas.SetActive(true);
    }

    [PunRPC]
    public void OnStrike(Vector2 direction, float knockback, float damage)
    {
        playerUtilities.HandleStrike(direction, knockback, damage);
        StartCoroutine(HurtCoroutine());
    }

    [PunRPC]
    public void EndHurt()
    {
        playerUtilities.EndHurt();
    }

    IEnumerator HurtCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        playerComponent.PhotonView.RPC("EndHurt", RpcTarget.AllBuffered);
    }
}
