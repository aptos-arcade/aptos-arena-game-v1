using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MonoBehaviourPun, IWeapon
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
