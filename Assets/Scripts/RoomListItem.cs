using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour
{

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;
    [SerializeField]
    private Text joinPrompt;

    private MatchInfoSnapshot match;

    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        match = _match;
        joinRoomCallback = _joinRoomCallback;

        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
        joinPrompt.text = match.currentSize == 1 ? "(click to join)" : "(full)";
    }

    public void JoinRoom()
    {
        if(match.currentSize == 1) {
            joinRoomCallback.Invoke(match);
        }
    }

}