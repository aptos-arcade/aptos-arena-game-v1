using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListing : MonoBehaviourPunCallbacks
{

    public Transform grid;
    public GameObject roomNamePrefab;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room added");
        foreach(RoomInfo room in roomList)
        {
            if(room.RemovedFromList)
            {
                DeleteRoom(room);
            }
            else
            {
                AddRoom(room);
            }
        }
    }

    void AddRoom(RoomInfo room)
    {
        print("Add Room: " + room.Name);
        GameObject obj = Instantiate(roomNamePrefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(grid.transform, false);
        obj.GetComponentInChildren<TMP_Text>().text = room.Name;
    }

    void DeleteRoom(RoomInfo room)
    {
        print("Delete room: " + room.Name);

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
