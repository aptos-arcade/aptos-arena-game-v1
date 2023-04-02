using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace HomeScreen
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private GameObject usernameScreen;

        [SerializeField] private Button usernameButton;
        
        [SerializeField] private TMP_InputField usernameInput;
        
        [SerializeField] private Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(OnBack);
        }

        public void OnClickCreateNameButton()
        {
            PhotonNetwork.NickName = usernameInput.text;
            usernameScreen.SetActive(false);
            PhotonNetwork.LoadLevel(3);
        }

        public void OnNameFieldChange()
        {
            usernameButton.interactable = usernameInput.text.Length >= 2;
        }
        
        public void OnBack()
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}