using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerConnection : NetworkBehaviour
{
    private Text timeOutText;
    [SerializeField]
    private int turnTime;
    private float elapsedTime = 0;
    private AbstractPlayerController controller;
    private MobilemaxCamera MobilemaxCamera;

    public GameObject HealthCanvas;

    void Start()
    {
        HealthCanvas.SetActive(!isLocalPlayer);
        if (!isLocalPlayer) return;
        MobilemaxCamera = Camera.main.GetComponent<MobilemaxCamera>();
        controller = gameObject.GetComponent<AbstractPlayerController>();
        Debug.Log("i'm server: " + isServer);
        controller.myTurn = isServer;
        MobilemaxCamera.target = transform;
        timeOutText = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        timeOutText.color = controller.myTurn ? Color.blue : Color.red;
    }

    void Update()
    {
        if (!isLocalPlayer || NumberOfPlayers() < 2) return;

        elapsedTime += Time.deltaTime;
        int timeRemaining = turnTime - (int)elapsedTime;
        if(timeRemaining <= 0) {
            controller.myTurn = !controller.myTurn;
            MobilemaxCamera.target = transform;
            timeRemaining = turnTime;
            elapsedTime = 0;
            timeOutText.color = controller.myTurn ? Color.blue : Color.red;
        }
        timeOutText.text = "" + timeRemaining;
    }

    int NumberOfPlayers()
    {
        return GameObject.FindObjectsOfType<PlayerConnection>().Length;
    }

    /////////////COMMANDS
    
}
