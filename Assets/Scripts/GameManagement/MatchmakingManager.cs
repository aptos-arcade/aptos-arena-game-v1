using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameManagement
{
    public class MatchmakingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject matchmakingUI;
        
        [SerializeField] private GameObject characterPickerUI;
        
        [SerializeField] private Image progressBarFill;

        [SerializeField] private TMP_Text numPlayersText;
        
        private void Start()
        {
            UpdateMatchmakingUI();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            UpdateMatchmakingUI();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            UpdateMatchmakingUI();
        }

        private void UpdateMatchmakingUI()
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            if (playerCount == maxPlayers)
            {
                characterPickerUI.SetActive(true);
                matchmakingUI.SetActive(false);
            }
            else
            {
                numPlayersText.text = playerCount + "/" + maxPlayers + " Players";
                progressBarFill.fillAmount = (float)playerCount / maxPlayers;
            }
        }
    }
}