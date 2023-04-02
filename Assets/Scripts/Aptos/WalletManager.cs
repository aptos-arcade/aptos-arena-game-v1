using UnityEngine;

namespace Aptos
{
    public class WalletManager : MonoBehaviour
    {

        public static WalletManager Instance;
        
        private string _accountAddress;
        public string Address => _accountAddress;
        public bool IsLoggedIn => _accountAddress != null;
        
        private const string AccountAddressKey = "AccountAddressKey";

        public delegate void OnConnect(string accountAddress);
        public static event OnConnect OnConnectEvent;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void SetAccountAddress(string accountAddress)
        {
            _accountAddress = accountAddress;
            PlayerPrefs.SetString(AccountAddressKey, accountAddress);
            OnConnectEvent?.Invoke(accountAddress);
        }

        // public void CreateNewWallet()
        // {
        //     var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        //     var wallet = new Wallet(mnemonic);
        //     _account = wallet.GetAccount(0);
        //     PlayerPrefs.SetString(MnemonicKey, mnemonic.ToString());
        //     OnConnect(_account.AccountAddress.ToString());
        // }

        // public bool LoadAccountFromCache()
        // {
        //     var mnemonic = PlayerPrefs.GetString(MnemonicKey);
        //     if (mnemonic == "") return false;
        //     try
        //     {
        //         var wallet = new Wallet(mnemonic);
        //         _account = wallet.GetAccount(0);
        //         OnConnect(_account.AccountAddress.ToString());
        //         return true;
        //     }
        //     catch
        //     {
        //         // ignored
        //     }
        //
        //     return false;
        // }

        public static string Ellipsis(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
}