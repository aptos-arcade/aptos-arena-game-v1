using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    //private bool _playerOnPlatform;

    //private PlayerScript player;

    //private void Update()
    //{
    //    if(_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
    //    {
    //        player.PlayerComponents.FootCollider.enabled = false;
    //        StartCoroutine(EnableCollider());
    //    }
    //}

    //IEnumerator EnableCollider()
    //{
    //    yield return new WaitForSeconds(0.25f);
    //    player.PlayerComponents.FootCollider.enabled = true;
    //}

    //private void SetPlayerOnPlatform(Collision2D collision, bool value)
    //{
    //    var player = collision.gameObject.GetComponent<PlayerScript>();
    //    if(player != null)
    //    {
    //        this.player = player;
    //        _playerOnPlatform = true;
    //    }
    //}

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    SetPlayerOnPlatform(collision, true);
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    SetPlayerOnPlatform(collision, false);
    //}
}
