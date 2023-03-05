using Animations;
using GameManagement;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerScript : MonoBehaviourPun
    {

        [SerializeField]
        private PlayerStats playerStats;

        [SerializeField]
        private PlayerComponent playerComponent;

        [SerializeField]
        private PlayerReferences playerReferences;

        private PlayerUtilities _playerUtilities;

        private PlayerActions _playerActions;
    
        public PlayerComponent PlayerComponents => playerComponent;
        public PlayerStats PlayerStats => playerStats;
        public PlayerActions PlayerActions => _playerActions;
        public PlayerUtilities PlayerUtilities => _playerUtilities;
        public PlayerReferences PlayerReferences => playerReferences;

        // Start is called before the first frame update
        private void Awake()
        {
            if (photonView.IsMine)
            {
                if (!GameManager.Instance) return;
                GameManager.Instance.LocalPlayer = gameObject;

                var playerPosition = transform.position;
                playerReferences.PlayerCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, playerReferences.PlayerCamera.transform.position.z);
                playerReferences.PlayerCamera.SetActive(true);
                playerReferences.PlayerCamera.transform.SetParent(null, true);
                playerReferences.NameTag.text = PhotonNetwork.NickName;
                playerReferences.NameTag.color = new Color(0.6588235f, 0.8078431f, 1f);
            }
            else
            {
                playerReferences.NameTag.text = photonView.Owner.NickName;
                playerReferences.NameTag.color = Color.red;
            }
        }


        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("Start");
            _playerActions = new PlayerActions(this);
            _playerUtilities = new PlayerUtilities(this);

            playerStats.Speed = playerStats.WalkSpeed;
            playerStats.PlayerName = PhotonNetwork.NickName;

            AnyStateAnimation[] animations = new AnyStateAnimation[]
            {
                new AnyStateAnimation(Rig.Body, "Body_Idle", "Body_Attack"),
                new AnyStateAnimation(Rig.Body, "Body_Walk", "Body_Attack", "Body_Jump"),
                new AnyStateAnimation(Rig.Body, "Body_Jump"),
                new AnyStateAnimation(Rig.Body, "Body_Fall", "Body_Attack"),
                new AnyStateAnimation(Rig.Body, "Body_Attack"),

                new AnyStateAnimation(Rig.Legs, "Legs_Idle", "Legs_Attack"),
                new AnyStateAnimation(Rig.Legs, "Legs_Walk", "Legs_Jump"),
                new AnyStateAnimation(Rig.Legs, "Legs_Jump"),
                new AnyStateAnimation(Rig.Legs, "Legs_Fall"),
                new AnyStateAnimation(Rig.Legs, "Legs_Attack")
            };

            playerComponent.Animator.AnimationTriggerEvent += PlayerActions.Shoot;
            playerComponent.Animator.AddAnimations(animations);
            PlayerUtilities.GetSpriteRenderers();

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
            }
        }

        private void FixedUpdate()
        {
            PlayerActions.Move(transform);
        }
    
        private void OnDeath()
        {
            PhotonNetwork.Instantiate(playerReferences.ExplosionPrefab.name, transform.position, Quaternion.identity);
            GameManager.Instance.EnableRespawn();
            photonView.RPC("ShowDeath", RpcTarget.AllBuffered);
        }
    }
}
