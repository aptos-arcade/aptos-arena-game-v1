using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomeScreen
{
    public class RoomButton : MonoBehaviour
    {
        public TMP_Text roomName;

        private void Start()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(() => JoinRoomByName(roomName.text));
        }

        private static void JoinRoomByName(string room)
        {
            PhotonNetwork.JoinRoom(room);
        }
    }
}
