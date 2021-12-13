using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobby : MonoBehaviour
{
    public void Join()
    {
        FindObjectOfType<NetworkSendHandler>().SendJoinLobby();
    }
}
