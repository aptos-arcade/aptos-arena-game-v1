using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TimeOut : MonoBehaviourPun
{

    [SerializeField]
    private GameObject timeOutUI;

    private float idleTime = 60f;
    private float timer = 5;

    [SerializeField]
    private TMP_Text timeOutText;

    private bool timeOver;

    // Update is called once per frame
    void Update()
    {
        if (!timeOver)
        {
            if (Input.anyKey)
            {
                idleTime = 60f;
            }

            idleTime -= Time.deltaTime;

            if (idleTime <= 0)
            {
                OpenTimeoutUI();
            }

            if (timeOutUI.activeSelf)
            {
                timer -= Time.deltaTime;
                timeOutText.text = "Disconnecting in: " + timer.ToString("F0");

                if (timer <= 0)
                {
                    timeOver = true;
                }
                else if (Input.anyKey)
                {
                    idleTime = 10;
                    timer = 5;
                    timeOutUI.SetActive(false);
                }
            }
        }
        else
        {
            LeaveRoom();
        }
    }

    void OpenTimeoutUI()
    {
        timeOutUI.SetActive(true);
    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
