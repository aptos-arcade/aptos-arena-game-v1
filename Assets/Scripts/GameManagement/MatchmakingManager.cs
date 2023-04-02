using System.Collections.Generic;
using Characters;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameManagement
{
    public class MatchmakingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject matchmakingUI;

        [SerializeField] private Image progressBarFill;

        [SerializeField] private TMP_Text numPlayersText;

        private Dictionary<CharactersEnum, int> _playerTeams = new();

        private void Start()
        {
            UpdateMatchmakingUI();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            var playerTeam = (CharactersEnum)newPlayer.CustomProperties["Character"];
            if (_playerTeams.ContainsKey(playerTeam))
            {
                _playerTeams[playerTeam]++;
            }
            else
            {
                _playerTeams.Add(playerTeam, 1);
            }

            UpdateMatchmakingUI();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            var playerTeam = (CharactersEnum)otherPlayer.CustomProperties["Character"];
            _playerTeams[playerTeam]--;
            UpdateMatchmakingUI();
        }

        private void UpdateMatchmakingUI()
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

            if (playerCount >= maxPlayers)
            {
                matchmakingUI.SetActive(false);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                numPlayersText.text = playerCount + "/" + maxPlayers + " Players";
                progressBarFill.fillAmount = (float)playerCount / maxPlayers;

                var playerTeams = new Dictionary<CharactersEnum, int>();
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    var playerTeam = (CharactersEnum)player.CustomProperties["Character"];
                    if (playerTeams.ContainsKey(playerTeam)) playerTeams[playerTeam]++;
                    else playerTeams.Add(playerTeam, 1);
                }

                var customRoomProperties = new Hashtable();
                foreach (var (playerTeam, numPlayers) in playerTeams)
                {
                    if(numPlayers > maxPlayers / 2) customRoomProperties.Add(playerTeam.ToString(), false);
                }
                PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            }
        }
    }
}