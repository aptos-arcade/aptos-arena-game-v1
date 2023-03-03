using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Explosion : MonoBehaviourPun
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
