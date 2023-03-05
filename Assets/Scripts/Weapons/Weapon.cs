using Photon.Pun;

namespace Weapons
{
    public class Weapon: MonoBehaviourPun
    {
        [PunRPC]
        public void Equip()
        {
            gameObject.SetActive(true);
        }

        [PunRPC]
        public void UnEquip()
        {
            gameObject.SetActive(false);
        }
    }
}
