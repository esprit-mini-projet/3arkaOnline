using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControlScript : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            Vector2 delta = Input.GetTouch(0).deltaPosition;
            transform.Translate(-delta.x * speed, 0, -delta.y * speed);
        }
    }
}
