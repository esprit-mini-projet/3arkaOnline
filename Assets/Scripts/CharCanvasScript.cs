using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCanvasScript : MonoBehaviour
{
    public int ZAngle = 0;
    public int YAngle = 0;
    public int XAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var _direction = (Camera.main.transform.position - transform.position).normalized * -1 ;
        /*_direction.z = 180;
        _direction.x = -180;
        */
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}
