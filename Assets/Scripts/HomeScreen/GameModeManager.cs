using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace HomeScreen
{
    public class GameModeManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject connectingScreen, gameModeSelectScreen;
        
        [SerializeField] private Button casualButton, rankedButton;

        private void Awake()
        {
            if(!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                connectingScreen.SetActive(false);
                gameModeSelectScreen.SetActive(true);
            }
            casualButton.onClick.AddListener(OnClickCasualButton);
            rankedButton.onClick.AddListener(OnClickRankedButton);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Connected to Lobby");
            connectingScreen.SetActive(false);
            gameModeSelectScreen.SetActive(true);
        }

        public void OnClickCasualButton()
        {
            gameModeSelectScreen.SetActive(false);
            PhotonNetwork.LoadLevel(1);
        }

        public void OnClickRankedButton()
        {
            gameModeSelectScreen.SetActive(false);
            PhotonNetwork.LoadLevel(2);
        }
    }
}
