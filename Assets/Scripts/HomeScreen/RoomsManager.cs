using Photon.Pun;

namespace HomeScreen
{
    public class RoomsManager : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(6);
        }
    }
}
