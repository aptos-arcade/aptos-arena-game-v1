using System.Collections.Generic;
using System.Runtime.InteropServices;
using Aptos;
using Characters;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomeScreen
{
    public class RankedCharacterSelectManager : MonoBehaviour
    {
        [SerializeField] private GameObject characterGrid;

        [SerializeField] private GameObject characterCardPrefab;
        
        [SerializeField] private Sprite[] characterSprites;

        [SerializeField] private TMP_Text accountAddressText;
        
        [SerializeField] private TMP_Text noRankedCharactersText;

        [SerializeField] private Button backButton;
        
        private readonly List<GameObject> _characterCards = new();
        
        [DllImport ("__Internal")]
        static extern void RankedCharacterSelectScreenLoad ();
        
        private void Awake()
        {
            WalletManager.OnConnectEvent += UpdateConnectedAddress;
        }

        private void Start()
        {
            UpdateConnectedAddress(WalletManager.Instance.Address);
            backButton.onClick.AddListener(OnBack);
            
            #if UNITY_WEBGL && !UNITY_EDITOR
                  RankedCharacterSelectScreenLoad();
            #endif
        }
        
        private void UpdateConnectedAddress(string address)
        {
            accountAddressText.text = "Connected: " + WalletManager.Ellipsis(address, 8);
        }   

        public void AddCharacter(int characterEnumValue)
        {
            noRankedCharactersText.alpha = 0;
            var characterEnum = (CharactersEnum) characterEnumValue;
            var character = Instantiate(characterCardPrefab, characterGrid.transform);
            character.GetComponent<CharacterCard>().InitializeCharacterCard(
                characterSprites[characterEnumValue], 
                Characters.Characters.AvailableCharacters[characterEnum].DisplayName,
                characterEnum
            );
            _characterCards.Add(character);
        }

        public void RemoveCharacters()
        {
            foreach (var characterCard in _characterCards)
            {
                Destroy(characterCard);
            }
            noRankedCharactersText.alpha = 1;
        }

        private void OnBack()
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
}
