mergeInto(LibraryManager.library, {
  WalletScreenLoad: function () {
    dispatchReactUnityEvent("WalletScreenLoad");
  },
  RankedCharacterSelectScreenLoad: function () {
    dispatchReactUnityEvent("RankedCharacterSelectScreenLoad");
  },
});