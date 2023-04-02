using System.Collections.Generic;
using System.Linq;
using Characters;
using Photon.Pun;
using Player;
using TMPro;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        // singleton pattern
        public static GameManager Instance;
        
        public PlayerScript Player { get; set; }

        [SerializeField] private GameObject spawnButton, connectedPlayersView, sceneCamera, respawnUI, 
            leaveScreen, feedBox, feedTextPrefab, outOfLivesUI;
        [SerializeField] private ConnectedPlayer connectedPlayer;
        [SerializeField] private TMP_Text pingRate, respawnTimer;
        [SerializeField] private float timeAmount = 5;
        [SerializeField] private Transform[] spawnPositions;
        [SerializeField] private LayerMask playerLayer;
        
        private Dictionary<CharactersEnum, int> _teamCounts;
        private Dictionary<CharactersEnum, int> _teamSpawnPositions;
        private int _numTeams;

        private string _characterPrefabName;
        private bool _startRespawn;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _characterPrefabName = Characters.Characters.AvailableCharacters[(CharactersEnum)PhotonNetwork.LocalPlayer
                .CustomProperties["Character"]].PrefabName;
            connectedPlayer.AddLocalPlayer();
            connectedPlayer.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
            ProcessTeams();
            Physics2D.IgnoreLayerCollision(playerLayer, playerLayer);
        }
    
        private void Update()
        {
            if (_startRespawn) StartRespawn();
            if (Input.GetKeyDown(KeyCode.Escape)) ToggleLeaveScreen();
            connectedPlayersView.SetActive(Input.GetKey(KeyCode.Tab));
            var ping = PhotonNetwork.GetPing();
            pingRate.text = "Ping: " + ping;
            pingRate.color = ping > 100 ? Color.red : new Color(0.6588235f, 0.8078431f, 1f) ;
        }
        public void SpawnPlayer()
        {
            spawnButton.SetActive(false);
            sceneCamera.SetActive(false);
            Vector2 spawnLocation = spawnPositions[_teamSpawnPositions[(CharactersEnum)PhotonNetwork.LocalPlayer
                .CustomProperties["Character"]]].position;
            PhotonNetwork.Instantiate(_characterPrefabName, spawnLocation, Quaternion.identity);
        }

        private void StartRespawn()
        {
            timeAmount -= Time.deltaTime;
            respawnTimer.text = "Respawn in: " + timeAmount.ToString("F0");
            if (!(timeAmount <= 0)) return;
            respawnUI.SetActive(false);
            _startRespawn = false;
            Vector3 spawnLocation = spawnPositions[_teamSpawnPositions[(CharactersEnum)PhotonNetwork.LocalPlayer
                .CustomProperties["Character"]]].position;
            StartCoroutine(Player.PlayerUtilities.RespawnCoroutine(spawnLocation));
        }

        public void EnableRespawn()
        {
            timeAmount = 5;
            _startRespawn = true;
            respawnUI.SetActive(true);
        }

        private void ToggleLeaveScreen()
        {
            leaveScreen.SetActive(!leaveScreen.activeSelf);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }

        public void OnPlayerOutOfLives()
        {
            outOfLivesUI.SetActive(true);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
            go.transform.SetParent(feedBox.transform);
            go.GetComponent<TMP_Text>().text = player.NickName + " has joined the game";
            Destroy(go, 3f);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            connectedPlayer.RemovePlayerFromList(player.NickName);
            _teamCounts[(CharactersEnum) player.CustomProperties["Character"]]--;
            GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
            go.transform.SetParent(feedBox.transform);
            go.GetComponent<TMP_Text>().text = player.NickName + " has left the game";
            Destroy(go, 3f);
        }

        private void ProcessTeams()
        {
            var teamCounts = new Dictionary<CharactersEnum, int>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var playerTeam = (CharactersEnum) player.CustomProperties["Character"];
                if (teamCounts.ContainsKey(playerTeam)) teamCounts[playerTeam]++;
                else teamCounts.Add(playerTeam, 1);
            }
            _teamCounts = teamCounts;
            
            _teamSpawnPositions = teamCounts.Keys
                .OrderBy(team => (int)team)
                .Select((team, i) => new { Team = team, Index = i })
                .ToDictionary(team => team.Team, team => team.Index % spawnPositions.Length);
        }
    }
}
