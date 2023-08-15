using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Line line;
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            var pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Camera.main.nearClipPlane));
            //line.LineSetUp(this.transform.position, pos); //Comment out this line to just check throwing mechanism since it is causing issues. 
        }

    }
}
