using Aptos.Accounts;

namespace Characters
{
    public enum CharactersEnum {PontemPirates, AptosMonkeys}
    
    public class Character
    {
        public string DisplayName { get; }
        
        public string PrefabName { get; }
        
        public AccountAddress CreatorAddress { get; }
        public string CollectionName { get; }
        public string TokenName { get; }
        
        public Character(
            string displayName, 
            string prefabName,
            AccountAddress creatorAddress,
            string collectionName,
            string tokenName
        )
        {
            DisplayName = displayName;
            PrefabName = prefabName;
            CreatorAddress = creatorAddress;
            CollectionName = collectionName;
            TokenName = tokenName;
        }
    }
}