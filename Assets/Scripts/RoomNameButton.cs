using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class RoomNameButton : MonoBehaviour
{
    public TMP_Text roomName;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => JoinRoomByName(roomName.text));
    }

    void JoinRoomByName(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }
}
