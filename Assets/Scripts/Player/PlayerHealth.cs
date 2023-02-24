using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{

    [SerializeField]
    private PlayerScript player;

    private Image healthBar;

    private float currentHealth;

    private void Start()
    {
        currentHealth = player.PlayerStats.Health;
        healthBar = GetComponent<Image>();
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / player.PlayerStats.Health;
        if (photonView.IsMine && currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        GameManager.instance.EnableRespawn();
        player.PlayerComponents.PhotonView.RPC("OnDeath", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Revive()
    {
        currentHealth = player.PlayerStats.Health;
        healthBar.GetComponent<Image>().fillAmount = 1;

        player.PlayerComponents.PhotonView.RPC("OnRevive", RpcTarget.AllBuffered);
    }

    //[PunRPC]
    //public void KilledBy(string name)
    //{
    //    GameObject go = Instantiate(player.PlayerReferences.KillFeedPrefab, new Vector2(0, 0), Quaternion.identity);
    //    go.transform.SetParent(GameManager.instance.killFeedBox.transform);
    //    go.GetComponent<TMP_Text>().text = "You got killed by: " + name;
    //    go.GetComponent<TMP_Text>().color = Color.red;
    //    Destroy(go, 3);
    //}

    //[PunRPC]
    //public void YouKilled(string name)
    //{
    //    GameObject go = Instantiate(player.PlayerReferences.KillFeedPrefab, new Vector2(0, 0), Quaternion.identity);
    //    go.transform.SetParent(GameManager.instance.killFeedBox.transform);
    //    go.GetComponent<TMP_Text>().text = "You killed: " + name;
    //    go.GetComponent<TMP_Text>().color = Color.green;
    //    Destroy(go, 3);
    //}
}
