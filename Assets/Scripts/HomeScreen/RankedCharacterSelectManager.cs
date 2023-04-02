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
        
        [SerializeField] private GameObject noRankedCharactersText;

        [SerializeField] private Button backButton;

        [DllImport ("__Internal")]
        static extern void RankedCharacterSelectScreenLoad ();

        public void Start()
        {
            accountAddressText.text = "Connected: " + WalletManager.Ellipsis(WalletManager.Instance.Address, 8);
            backButton.onClick.AddListener(OnBack);
            
            #if UNITY_WEBGL && !UNITY_EDITOR
                  RankedCharacterSelectScreenLoad();
            #endif
        }
        
        public void AddCharacter(int characterEnumValue)
        {
            noRankedCharactersText.SetActive(false);
            var characterEnum = (CharactersEnum) characterEnumValue;
            var character = Instantiate(characterCardPrefab, characterGrid.transform);
            character.GetComponent<CharacterCard>().InitializeCharacterCard(
                characterSprites[characterEnumValue], 
                Characters.Characters.AvailableCharacters[characterEnum].DisplayName,
                characterEnum
            );
        }

        private void OnBack()
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
}
