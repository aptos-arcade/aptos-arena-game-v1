using Animations;
using Characters;
using GameManagement;
using Photon.Pun;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerScript : MonoBehaviourPun
    {

        [SerializeField] private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        [SerializeField] private PlayerComponent playerComponent;
        public PlayerComponent PlayerComponents => playerComponent;

        [SerializeField] private PlayerReferences playerReferences;
        public PlayerReferences PlayerReferences => playerReferences;

        public PlayerActions PlayerActions { get; private set; }

        public PlayerUtilities PlayerUtilities { get; private set; }
        
        public PlayerState PlayerState { get; } = new();

        // Start is called before the first frame update
        private void Awake()
        {
            if (photonView.IsMine)
            {
                if (!GameManager.Instance) return;
                GameManager.Instance.Player = this;

                var playerPosition = transform.position;
                playerReferences.PlayerCamera.transform.position = new Vector3(playerPosition.x, playerPosition.y, playerReferences.PlayerCamera.transform.position.z);
                playerReferences.PlayerCamera.SetActive(true);
                playerReferences.PlayerCamera.transform.SetParent(null, true);
                
                playerReferences.NameTag.text = PhotonNetwork.NickName;
                playerReferences.NameTag.color = new Color(0.6588235f, 0.8078431f, 1f);
            }
            else
            {
                playerReferences.NameTag.color = (CharactersEnum)photonView.Owner.CustomProperties["Character"] == 
                                                 (CharactersEnum)PhotonNetwork.LocalPlayer.CustomProperties["Character"] 
                    ? new Color(0.6588235f, 0.8078431f, 1f) 
                    : Color.red;
                playerReferences.NameTag.text = photonView.Owner.NickName;
            }
        }


        // Start is called before the first frame update
        private void Start()
        {
            PlayerActions = new PlayerActions(this);
            PlayerUtilities = new PlayerUtilities(this);

            PlayerState.PlayerName = PhotonNetwork.NickName;
            PlayerState.Character = (CharactersEnum)PhotonNetwork.LocalPlayer.CustomProperties["Character"];

            AnyStateAnimation[] animations = {
                new(Rig.Body, "Body_Idle", "Body_Attack"),
                new(Rig.Body, "Body_Walk", "Body_Attack", "Body_Jump"),
                new(Rig.Body, "Body_Jump"),
                new(Rig.Body, "Body_Fall", "Body_Attack"),
                new(Rig.Body, "Body_Attack"),

                new(Rig.Legs, "Legs_Idle", "Legs_Attack"),
                new(Rig.Legs, "Legs_Walk", "Legs_Jump"),
                new(Rig.Legs, "Legs_Jump"),
                new(Rig.Legs, "Legs_Fall"),
                new(Rig.Legs, "Legs_Attack")
            };

            playerComponent.Animator.AnimationTriggerEvent += PlayerActions.Shoot;
            playerComponent.Animator.AddAnimations(animations);

            PlayerUtilities.GetSpriteRenderers();

            playerReferences.DamageDisplay.text = ((PlayerState.DamageMultiplier - 1) * 100) + "%";
            

            Debug.Log(transform.position);
            StartCoroutine(PlayerUtilities.RespawnCoroutine(transform.position));
        }

        // Update is called once per frame
        private void Update()
        {
            PlayerUtilities.HandleInput();
            PlayerUtilities.HandleAir();
            if (photonView.IsMine && PlayerState.CanMove && (Mathf.Abs(transform.position.x) > 30 || Mathf.Abs(transform.position.y) > 16))
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
            photonView.RPC("OnDeath", RpcTarget.AllBuffered);
            if(PlayerState.Lives == 0) GameManager.Instance.OnPlayerOutOfLives(); 
            else GameManager.Instance.EnableRespawn();
        }
    }
}
