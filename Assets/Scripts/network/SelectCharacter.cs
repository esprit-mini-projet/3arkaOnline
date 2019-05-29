using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public void selectHaj()
    {
        GameObject.FindGameObjectWithTag("characterSelector").GetComponent<CharacterSelection>().Character = Character.Haj;
    }

    public void selectKhaliga()
    {
        GameObject.FindGameObjectWithTag("characterSelector").GetComponent<CharacterSelection>().Character = Character.Khaliga;
    }
}
