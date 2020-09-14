using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        var lookAtVec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,transform.position.z));
        transform.LookAt(lookAtVec,Vector3.forward);
    }
}
