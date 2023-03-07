using Photon.Pun;
using UnityEngine.UI;

namespace Player
{
    public class PlayerLife : MonoBehaviourPun
    {

        private Image _heart;

        private void Start()
        {
            _heart = GetComponent<Image>();
        }

        [PunRPC]
        public void Hide()
        {
            _heart.enabled = false;
        }
    }
}