using Characters;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace HomeScreen
{
    public class CharacterSelectManager : MonoBehaviour
    {

        [SerializeField] private GameObject characterGrid;

        [SerializeField] private GameObject characterCardPrefab;
        
        [SerializeField] private CharactersEnum[] availableCharacters;
        [SerializeField] private Sprite[] characterSprites;
        
        [SerializeField] private Button backButton;

        public void Start()
        {
            for (var i = 0; i < availableCharacters.Length; i++)
            {
                var character = Instantiate(characterCardPrefab, characterGrid.transform);
                character.GetComponent<CharacterCard>().InitializeCharacterCard(
                    characterSprites[i], 
                    Characters.Characters.AvailableCharacters[availableCharacters[i]].DisplayName,
                    availableCharacters[i]
                );
            }
            backButton.onClick.AddListener(OnBack);
        }
        
        private void OnBack()
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}