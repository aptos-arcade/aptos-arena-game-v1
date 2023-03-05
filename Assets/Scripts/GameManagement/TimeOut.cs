using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameManagement
{
    public class TimeOut : MonoBehaviourPun
    {

        [SerializeField] private GameObject timeOutUI;
        [SerializeField] private float idleTime = 60;
        [SerializeField] private float timer = 5;
        [SerializeField] private TMP_Text timeOutText;
        private bool _timeOver;

        // Update is called once per frame
        private void Update()
        {
            if (!_timeOver)
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
                        _timeOver = true;
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

        private void OpenTimeoutUI()
        {
            timeOutUI.SetActive(true);
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }
    }
}
