using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomList : MonoBehaviourPunCallbacks
{

    public Transform grid;
    public GameObject roomNamePrefab;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo room in roomList)
        {
            if(room.RemovedFromList) DeleteRoom(room);
            else AddRoom(room);
        }
    }

    void AddRoom(RoomInfo room)
    {
        GameObject roomObject = Instantiate(roomNamePrefab, new Vector2(0, 0), Quaternion.identity);
        roomObject.transform.SetParent(grid.transform, false);
        roomObject.GetComponentInChildren<TMP_Text>().text = room.Name;
    }

    void DeleteRoom(RoomInfo room)
    {
        int roomCount = grid.childCount;
        for(int i = 0; i < roomCount; i++)
        {
            if(grid.GetChild(i).GetComponentInChildren<TMP_Text>().text == room.Name)
            {
                Destroy(grid.GetChild(i).transform);
            };
        }
    }
}
