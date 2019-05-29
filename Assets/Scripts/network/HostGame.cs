using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 2;
    private NetworkManager networkManager;
    [SerializeField]
    private Text roomText;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void createRoom()
    {
        Debug.Log("creating room");
        string roomName = roomText.text;
        if(roomName != "" && roomName != null) {
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("Match created: " + success);
        networkManager.StartHost(matchInfo);
    }
}
