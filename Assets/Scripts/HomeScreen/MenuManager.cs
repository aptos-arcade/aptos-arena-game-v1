using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject ConnectingScreen, UserNameScreen, ConnectScreen;

    [SerializeField]
    GameObject CreateUserNameButton;

    [SerializeField]
    TMP_InputField UserNameInput, CreateRoomInput;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby");
        ConnectingScreen.SetActive(false);
        UserNameScreen.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #region UIMethods

    public void OnClickCreateNameButton()
    {
        PhotonNetwork.NickName = UserNameInput.text;
        UserNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
    }

    public void OnNameFieldChange()
    {
        if(UserNameInput.text.Length >= 2)
        {
            CreateUserNameButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            CreateUserNameButton.GetComponent<Button>().interactable = false;
        }
    }

    public void OnClickCreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomInput.text, new RoomOptions { MaxPlayers = 4 }, null);
    }

    #endregion
}
