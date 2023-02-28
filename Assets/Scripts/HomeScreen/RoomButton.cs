using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public TMP_Text roomName;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => JoinRoomByName(roomName.text));
    }

    void JoinRoomByName(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }
}
