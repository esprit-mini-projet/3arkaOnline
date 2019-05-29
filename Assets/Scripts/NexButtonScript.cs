using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NexButtonScript : MonoBehaviour
{
    [SerializeField]
    private MenuScript menuScript;

    public void NextClicked()
    {
        if(menuScript.selectedCharacter != null)
            SceneManager.LoadScene(1);
    }
}
