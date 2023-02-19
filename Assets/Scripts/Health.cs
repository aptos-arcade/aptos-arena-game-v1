using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Health : MonoBehaviourPun
{
    public Image healthImage;
    public float health = 1;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D bc;
    public GameObject playerCanvas;

    public Cowboy playerScript;

    public GameObject killFeedTextPrefab;

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        health -= damage;
        healthImage.fillAmount = health;
        CheckHealth();
    }

    public void EnableInputs()
    {
        playerScript.inputsDisabled = false;
    }

    [PunRPC]
    public void CheckHealth()
    {
        if(photonView.IsMine && health <= 0)
        {
            GameManager.instance.EnableRespawn();
            playerScript.inputsDisabled = true;
            this.GetComponent<PhotonView>().RPC("OnDeath", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnDeath()
    {
        rb.gravityScale = 0;
        bc.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void Revive()
    {
        rb.gravityScale = 1;
        bc.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        health = 1;
        healthImage.fillAmount = health;
    }

    [PunRPC]
    public void KilledBy(string name)
    {
        GameObject go = Instantiate(killFeedTextPrefab, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.killFeedBox.transform);
        go.GetComponent<TMP_Text>().text = "You got killed by " + name;
        go.GetComponent<TMP_Text>().color = Color.red;
        Destroy(go, 3);
    }

    [PunRPC]
    public void YouKilled(string name)
    {
        GameObject go = Instantiate(killFeedTextPrefab, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.killFeedBox.transform);
        go.GetComponent<TMP_Text>().text = "You killed by " + name;
        go.GetComponent<TMP_Text>().color = Color.green;
        Destroy(go, 3);
    }
}
