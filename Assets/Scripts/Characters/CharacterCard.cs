using System;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace Characters
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        
        [SerializeField] private TMP_Text characterName;
        
        [SerializeField] private Button selectButton;

        public void InitializeCharacterCard(Sprite charSprite, string charName, CharactersEnum charEnum)
        {
            characterImage.sprite = charSprite;
            characterName.text = charName;
            selectButton.onClick.AddListener(() => SelectCharacter(charEnum));
        }
        
        public void SelectCharacter(CharactersEnum character)
        {
            var playerProperties = new Hashtable() { { "Character", character } };
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            PhotonNetwork.LoadLevel(5);
        }
    }
}