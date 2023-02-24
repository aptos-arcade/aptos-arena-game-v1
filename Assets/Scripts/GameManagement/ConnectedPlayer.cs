using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectedPlayer : MonoBehaviour
{
    public GameObject currentPlayerPrefab;
    public GameObject currentPlayersGrid;

    public void AddLocalPlayer()
    {
        GameObject obj = Instantiate(currentPlayerPrefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(currentPlayersGrid.transform);
        obj.GetComponentInChildren<TMP_Text>().text = "You: " + PhotonNetwork.NickName;
        obj.GetComponentInChildren<TMP_Text>().color = Color.green;
    }

    [PunRPC]
    public void UpdatePlayerList(string name)
    {
        GameObject obj = Instantiate(currentPlayerPrefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(currentPlayersGrid.transform);
        obj.GetComponentInChildren<TMP_Text>().text = name;
    }

    public void RemovePlayerFromList(string name)
    {
        foreach(TMP_Text playerName in currentPlayersGrid.GetComponentsInChildren<TMP_Text>())
        {
            if(name == playerName.text)
            {
                Destroy(playerName.transform.parent.gameObject);
            }
        }
    }
}
