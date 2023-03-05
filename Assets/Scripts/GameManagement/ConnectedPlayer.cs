using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameManagement
{
    public class ConnectedPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject currentPlayerPrefab;
        [SerializeField] private GameObject currentPlayersGrid;

        public void AddLocalPlayer()
        {
            GameObject obj = Instantiate(currentPlayerPrefab, new Vector2(0, 0), Quaternion.identity);
            obj.transform.SetParent(currentPlayersGrid.transform);
            obj.GetComponentInChildren<TMP_Text>().text = "You: " + PhotonNetwork.NickName;
            obj.GetComponentInChildren<TMP_Text>().color = Color.green;
        }

        [PunRPC]
        public void UpdatePlayerList(string playerName)
        {
            GameObject obj = Instantiate(currentPlayerPrefab, new Vector2(0, 0), Quaternion.identity);
            obj.transform.SetParent(currentPlayersGrid.transform);
            obj.GetComponentInChildren<TMP_Text>().text = playerName;
        }

        public void RemovePlayerFromList(string playerName)
        {
            foreach(var currentPlayerName in currentPlayersGrid.GetComponentsInChildren<TMP_Text>())
            {
                if(playerName == currentPlayerName.text)
                {
                    Destroy(currentPlayerName.transform.parent.gameObject);
                }
            }
        }
    }
}
