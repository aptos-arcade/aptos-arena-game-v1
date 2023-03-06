using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace HomeScreen
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private string roomName;

        [SerializeField] private byte maxPlayers;

        [SerializeField] private TMP_Text roomNameText;
        
        [SerializeField] private UnityEngine.UI.Button joinRoomButton;

        private void Start()
        {
            roomNameText.text = roomName + " (" + maxPlayers + " Player" + (maxPlayers > 1 ? "s" : "") + ")";
            joinRoomButton.onClick.AddListener(OnClickJoinRoom);
        }
        
        private void OnClickJoinRoom()
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions {MaxPlayers = maxPlayers});
        }
    }
}
