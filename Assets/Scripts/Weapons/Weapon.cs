using System;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Weapon: MonoBehaviourPun
    {

        [SerializeField] private AudioSource audioSource;

        public void PlaySound()
        {
            audioSource.Play();
        }
        
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
