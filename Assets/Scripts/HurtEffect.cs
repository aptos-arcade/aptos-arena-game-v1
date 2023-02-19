using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HurtEffect : MonoBehaviourPun
{

    public SpriteRenderer sprite;

    public enum EventCodes
    {
        ColorChange = 0
    }


    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        EventCodes eventCode = (EventCodes)photonEvent.Code;
        if(eventCode == EventCodes.ColorChange)
        {
            object[] data = photonEvent.CustomData as object[];
            if(data.Length == 4)
            {
                if ((int)data[0] == base.photonView.ViewID)
                {
                    sprite.color = new Color((float)data[1], (float)data[2], (float)data[3]);
                }
            }
        }
    }

    public void ResetToWhite()
    {
        ChangeColorToWhite();
    }

    IEnumerator ResetColorAfterTime()
    {
        yield return new WaitForSeconds(0.25f);
        ResetToWhite();
    }

    public void OnHit()
    {
        ChangeColorToRed();
        StartCoroutine(ResetColorAfterTime());
    }

    void ChangeColorToRed()
    {
        float r = 1f, g = 0f, b = 0f;

        object[] data = new object[] { base.photonView.ViewID, r, g, b };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.Reliability = true;

        PhotonNetwork.RaiseEvent((byte)EventCodes.ColorChange, data, options, sendOptions);
    }

    void ChangeColorToWhite()
    {
        float r = 1f, g = 1f, b = 1f;

        object[] data = new object[] { base.photonView.ViewID, r, g, b };

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.Reliability = true;

        PhotonNetwork.RaiseEvent((byte)EventCodes.ColorChange, data, options, sendOptions);
    }
}
