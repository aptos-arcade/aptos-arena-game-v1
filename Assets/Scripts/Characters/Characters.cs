using System.Collections.Generic;
using Aptos.Accounts;

namespace Characters
{
    public static class Characters
    {
        public static Dictionary<CharactersEnum, Character> AvailableCharacters = new(){
            {CharactersEnum.PontemPirates, new Character(
                "Pontem Pirate", 
                "Pontem Pirate",
                AccountAddress.FromHex("0xc46dd298b89d38314b486b2182a6163c4c955dce3509bf30751c307f5ecc2f36"), 
                "Pontem Space Pirates", 
                "Space Pirate #993"
                )
            },
            {CharactersEnum.AptosMonkeys, new Character(
                    "Aptos Monkey", 
                    "Aptos Monkey",
                    AccountAddress.FromHex("0xf932dcb9835e681b21d2f411ef99f4f5e577e6ac299eebee2272a39fb348f702"), 
                    "Aptos Monkeys", 
                    "AptosMonkeys #4037"
                )
            }
        };
    }
}