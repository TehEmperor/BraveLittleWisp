using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessgae : MonoBehaviour
{
    Camera maincam;
    private void Awake() {
        maincam = Camera.main;
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.forward = maincam.transform.forward;
    }
}
