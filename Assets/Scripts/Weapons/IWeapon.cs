using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public interface IWeapon
{
    [PunRPC]
    public void Equip();

    [PunRPC]
    public void Unequip();
}
