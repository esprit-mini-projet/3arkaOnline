using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInit : NetworkBehaviour
{
    [SerializeField]
    private GameObject HajPrefab;
    [SerializeField]
    private GameObject KhaligaPrefab;

    void Start()
    {
        Character character = GameObject.FindGameObjectWithTag("characterSelector").GetComponent<CharacterSelection>().Character;
        GameObject go;
        if(character == Character.Haj) {
            go = Instantiate(HajPrefab);
        }else {
            go = Instantiate(KhaligaPrefab);
        }
        go.transform.position = transform.position;
        var c = go.GetComponent<AbstractPlayerController>();
        c.Hud = GameObject.FindGameObjectWithTag("hud").GetComponent<HUD>();
        c.ACooldownIndicator = GameObject.FindGameObjectWithTag("ai");
        c.BCooldownIndicator = GameObject.FindGameObjectWithTag("bi");
        c.YCooldownIndicator = GameObject.FindGameObjectWithTag("yi");
        NetworkServer.ReplacePlayerForConnection(connectionToClient, go, 0);
        Destroy(this);
    }
}
