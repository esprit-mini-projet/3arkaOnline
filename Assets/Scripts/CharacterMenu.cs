using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    public float Speed = 10;

    public Animator animator;

    public bool rotate = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate)
        {
            transform.Rotate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
        }
    }
}
