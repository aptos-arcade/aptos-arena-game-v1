using System;
using Characters;
using ExitGames.Client.Photon;
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
            var teamSize = maxPlayers / 2;
            roomNameText.text = roomName + " (" + (teamSize > 0 ? (teamSize + "v" + teamSize) : "Solo") + ")" ;
            joinRoomButton.onClick.AddListener(OnClickJoinRoom);
        }
        
        private void OnClickJoinRoom()
        {
            var playerTeam = ((CharactersEnum)PhotonNetwork.LocalPlayer.CustomProperties["Character"]).ToString();
            
            var roomOptions = new RoomOptions
            {
                MaxPlayers = maxPlayers
            };
            var customRoomProperties = new Hashtable();
            var charEnums = Enum.GetNames(typeof(CharactersEnum));
            foreach(var charEnum in charEnums)
            {
                customRoomProperties.Add(charEnum, true);
            }
            roomOptions.CustomRoomProperties = customRoomProperties;
            roomOptions.CustomRoomPropertiesForLobby = charEnums;
            
            PhotonNetwork.JoinRandomOrCreateRoom(
                roomOptions: roomOptions,
                expectedMaxPlayers: maxPlayers,
                expectedCustomRoomProperties: new Hashtable(){ { playerTeam, true } }
            );
        }
    }
}
