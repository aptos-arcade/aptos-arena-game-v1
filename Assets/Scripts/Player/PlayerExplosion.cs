using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerExplosion : MonoBehaviourPun
    {
        private void Start()
        {
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(1);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
