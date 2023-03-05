using System.Collections;
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
        
        public GameObject LocalPlayer { get; set; }

        [SerializeField] private GameObject selectPlayerCanvas, spawnCanvas, portal, connectedPlayersView, sceneCamera, 
            respawnUI, leaveScreen, feedBox, feedTextPrefab;
        [SerializeField] private ConnectedPlayer connectedPlayer;
        [SerializeField] private TMP_Text pingRate, respawnTimer;
        [SerializeField] private float timeAmount = 5;

        private string _characterPrefabName;
        private bool _startRespawn;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            connectedPlayer.AddLocalPlayer();
            connectedPlayer.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
        }
    
        private void Update()
        {
            if (_startRespawn) StartRespawn();
            if (Input.GetKeyDown(KeyCode.Escape)) ToggleLeaveScreen();
            connectedPlayersView.SetActive(Input.GetKey(KeyCode.Tab));
            pingRate.text = "Ping: " + PhotonNetwork.GetPing();
        }

        public void SelectCharacter(string characterPrefabName)
        {
            _characterPrefabName = characterPrefabName;
            selectPlayerCanvas.SetActive(false);
            spawnCanvas.SetActive(true);
        }

        public void SpawnPlayer()
        {
            spawnCanvas.SetActive(false);
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            Vector2 spawnLocation = new Vector2(Random.Range(-8f, 8f), 7);
            GameObject portalObj = PhotonNetwork.Instantiate(portal.name, spawnLocation, Quaternion.identity);
            var curPos = sceneCamera.transform.position;
            sceneCamera.transform.position = new Vector3(spawnLocation.x, curPos.y, curPos.z);
            yield return new WaitForSeconds(2.5f);
            PhotonNetwork.Destroy(portalObj);
            PhotonNetwork.Instantiate(_characterPrefabName, spawnLocation, Quaternion.identity);
            sceneCamera.SetActive(false);
        }

        private void StartRespawn()
        {
            timeAmount -= Time.deltaTime;
            respawnTimer.text = "Respawn in: " + timeAmount.ToString("F0");
            if (!(timeAmount <= 0)) return;
            respawnUI.SetActive(false);
            _startRespawn = false;
            StartCoroutine(LocalPlayer.GetComponent<PlayerScript>().PlayerUtilities.RespawnCoroutine());
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
            GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
            go.transform.SetParent(feedBox.transform);
            go.GetComponent<TMP_Text>().text = player.NickName + " has left the game";
            Destroy(go, 3f);
        }
    }
}
