using System.Runtime.InteropServices;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aptos
{
    public class WalletUIController : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text addressText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button backButton;
        
        [DllImport ("__Internal")]
        private static extern void WalletScreenLoad ();

        private void Awake()
        {
            WalletManager.OnConnectEvent += OnLogin;
        }

        private void Start()
        {
            continueButton.onClick.AddListener(OnContinue);
            backButton.onClick.AddListener(OnBack);
            if (WalletManager.Instance.IsLoggedIn)
            {
                OnLogin(WalletManager.Instance.Address);
            }
            #if UNITY_WEBGL && !UNITY_EDITOR
                WalletScreenLoad();
            #endif
        }

        private void OnLogin(string accountAddress)
        {
            addressText.text = "Connected: " + WalletManager.Ellipsis(accountAddress, 8);
            PhotonNetwork.NickName = WalletManager.Ellipsis(accountAddress, 8);
            continueButton.interactable = true;
        }
        
        private void OnContinue()
        {
            PhotonNetwork.LoadLevel(4);
        }

        private void OnBack()
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}