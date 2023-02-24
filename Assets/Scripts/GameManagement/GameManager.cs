using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public ConnectedPlayer connectedPlayer;
    public GameObject connectedPlayersView;

    public GameObject playerPrefab;

    public GameObject canvas;

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

    void Awake()
    {
        instance = this;
        canvas.SetActive(true);
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

    void SpawnPlayer()
    {
        float randomValue = Random.Range(-3, 3);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, 10), Quaternion.identity, 0);
        canvas.SetActive(false);
        sceneCamera.SetActive(false);
    }

    public void RelocatePlayer()
    {
        float randomPos = Random.Range(-3, 3);
        localPlayer.transform.localPosition = new Vector2(randomPos, 2);
    }

    void StartRespawn()
    {
        timeAmount -= Time.deltaTime;
        respawnTimer.text = "Respawn in: " + timeAmount.ToString("F0");
        if(timeAmount <= 0)
        {
            RelocatePlayer();
            respawnUI.SetActive(false);
            startRespawn = false;
            localPlayer.GetComponent<PlayerScript>().PlayerReferences.HealthBar.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }

    public void EnableRespawn()
    {
        timeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }

    public void ToggleLeaveScreen()
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
