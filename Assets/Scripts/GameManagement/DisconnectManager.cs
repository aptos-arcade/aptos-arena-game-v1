using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class DisconnectManager : MonoBehaviourPunCallbacks
    {

        [SerializeField] private GameObject disconnectUI;
        [SerializeField] private GameObject menuButton;
        [SerializeField] private GameObject reconnectButton;
        [SerializeField] private TMP_Text statusText;
    
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable) return;
            disconnectUI.SetActive(true);
            
            switch(SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    reconnectButton.SetActive(true);
                    statusText.text = "Lost connection to Photon, please try to reconnect";
                    break;
                case 1:
                    menuButton.SetActive(true);
                    statusText.text = "Lost connection to Photon, please try to reconnect in the main menu";
                    break;
            }
        }

        //called by photon
        public override void OnConnectedToMaster()
        {
            if (!disconnectUI.activeSelf) return;
            menuButton.SetActive(false);
            reconnectButton.SetActive(false);
            disconnectUI.SetActive(false);
        }

        public void OnClick_TryConnect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void OnClick_Menu()
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}
