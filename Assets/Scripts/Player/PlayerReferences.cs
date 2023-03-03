using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerReferences
{
    [SerializeField]
    private GameObject[] weaponObjects;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform gunBarrel;

    [SerializeField]
    private GameObject playerCanvas;

    [SerializeField]
    private GameObject killFeedPrefab;

    [SerializeField]
    private GameObject playerCamera;

    [SerializeField] private TMP_Text nametag;

    [SerializeField] private TMP_Text damageDisplay;

    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private GameObject playerSpriteTransform;

    public GameObject[] WeaponObjects { get => weaponObjects; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public Transform GunBarrel { get => gunBarrel; }
    public GameObject PlayerCanvas { get => playerCanvas; set => playerCanvas = value; }
    public GameObject KillFeedPrefab { get => killFeedPrefab; }
    public GameObject PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public TMP_Text Nametag { get => nametag; set => nametag = value; }
    public TMP_Text DamageDisplay { get => damageDisplay; set => damageDisplay = value; }
    
    public GameObject ExplosionPrefab { get => explosionPrefab; }
    
    public GameObject PlayerSpriteTransform { get => playerSpriteTransform; }
}
