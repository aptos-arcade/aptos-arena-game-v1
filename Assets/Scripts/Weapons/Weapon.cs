using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon: MonoBehaviourPun
{
    [PunRPC]
    public void Equip()
    {
        this.gameObject.SetActive(true);
    }

    [PunRPC]
    public void Unequip()
    {
        this.gameObject.SetActive(false);
    }
}
