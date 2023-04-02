namespace ModuleConstants
{
    public class GameCapabilityModule
    {
        public const string ModuleAddress = "0xe3eaddfcc4d7436d26fef92ee39685ef176e3513dc736d116129ce055c07afac";
        public const string ModuleName = "game_capabilities";
        public const string GameCapabilityStoreStruct = "GameCapabilityStore";

        public static string GameCapabilityStore()
        {
            return ModuleAddress + "::" + ModuleName + "::" + GameCapabilityStoreStruct;
        }
    }
}