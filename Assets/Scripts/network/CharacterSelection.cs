using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Character
{
    Haj, Khaliga
}

public class CharacterSelection : MonoBehaviour
{
    public Character Character { get; set; }
    
    private static CharacterSelection _instance;

    public static CharacterSelection Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
