using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public ConnectedPlayer connectedPlayer;
    public GameObject connectedPlayersView;
    
    private string _characterPrefabName;

    [SerializeField] private GameObject _selectPlayerCanvas;

    [SerializeField] private GameObject _spawnCanvas;
    
    public GameObject sceneCamera;

    public TMP_Text pingrate;

    public float timeAmount = 5;
    private bool startRespawn;
    public TMP_Text respawnTimer;
    public GameObject respawnUI;

    public static GameManager instance = null;

    [HideInInspector]
    public GameObject localPlayer;

    public GameObject leaveScreen;

    public GameObject feedBox;
    public GameObject feedTextPrefab;

    public GameObject killFeedBox;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        connectedPlayer.AddLocalPlayer();
        connectedPlayer.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
    }
    
    private void Update()
    {
        if (startRespawn)
        {
            StartRespawn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }
        if(Input.GetKey(KeyCode.Tab))
        {
            connectedPlayersView.SetActive(true);
        }
        else
        {
            connectedPlayersView.SetActive(false);
        }
        pingrate.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }

    public void SelectCharacter(string characterPrefabName)
    {
        _characterPrefabName = characterPrefabName;
        _selectPlayerCanvas.SetActive(false);
        _spawnCanvas.SetActive(true);
    }

    private void SpawnPlayer()
    {
        float randomValue = Random.Range(-4, 4);
        PhotonNetwork.Instantiate(_characterPrefabName, new Vector2(randomValue, 6), Quaternion.identity, 0);
        _spawnCanvas.SetActive(false);
        sceneCamera.SetActive(false);
    }

    private void StartRespawn()
    {
        timeAmount -= Time.deltaTime;
        respawnTimer.text = "Respawn in: " + timeAmount.ToString("F0");
        if (!(timeAmount <= 0)) return;
        respawnUI.SetActive(false);
        startRespawn = false;
        localPlayer.GetComponent<PlayerScript>().photonView.RPC("OnRevive", RpcTarget.AllBuffered);
    }

    public void EnableRespawn()
    {
        timeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }

    private void ToggleLeaveScreen()
    {
        if(leaveScreen.activeSelf)
        {
            leaveScreen.SetActive(false);
        }
        else
        {
            leaveScreen.SetActive(true);
        }
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedBox.transform);
        go.GetComponent<TMP_Text>().text = player.NickName + " has joined the game";
        Destroy(go, 3f);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        connectedPlayer.RemovePlayerFromList(player.NickName);
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedBox.transform);
        go.GetComponent<TMP_Text>().text = player.NickName + " has left the game";
        Destroy(go, 3f);
    }
}
