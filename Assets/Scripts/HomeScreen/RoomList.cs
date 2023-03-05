using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace HomeScreen
{
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

        private void AddRoom(RoomInfo room)
        {
            GameObject roomObject = Instantiate(roomNamePrefab, new Vector2(0, 0), Quaternion.identity);
            roomObject.transform.SetParent(grid.transform, false);
            roomObject.GetComponentInChildren<TMP_Text>().text = room.Name;
        }

        private void DeleteRoom(RoomInfo room)
        {
            for(int i = 0; i < grid.childCount; i++)
            {
                if(grid.GetChild(i).GetComponentInChildren<TMP_Text>().text == room.Name)
                    Destroy(grid.GetChild(i).transform);
            }
        }
    }
}
