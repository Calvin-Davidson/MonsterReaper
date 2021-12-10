using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectClient : MonoBehaviour
{
    public void Connect()
    {
        NetworkClient.Instance.Connect();
    }
}
