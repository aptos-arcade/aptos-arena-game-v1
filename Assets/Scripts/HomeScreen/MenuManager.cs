using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomeScreen
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject connectingScreen, userNameScreen, connectScreen, createUserNameButton;

        [SerializeField] private TMP_InputField userNameInput, createRoomInput;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
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
            userNameScreen.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(1);
        }

        #region UIMethods

        public void OnClickCreateNameButton()
        {
            PhotonNetwork.NickName = userNameInput.text;
            userNameScreen.SetActive(false);
            connectScreen.SetActive(true);
        }

        public void OnNameFieldChange()
        {
            createUserNameButton.GetComponent<Button>().interactable = userNameInput.text.Length >= 2;
        }

        public void OnClickCreateRoom()
        {
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions { MaxPlayers = 4 });
        }

        #endregion
    }
}
