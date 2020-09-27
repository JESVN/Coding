using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private float _max=8.52f;
    private float _min=-8.52f;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.down * Time.deltaTime * BackgroundControl.Instance._speed;
        transform.localPosition= transform.localPosition.y<=_min?new Vector3(transform.localPosition.x,_max+(transform.localPosition.y-_min),transform.localPosition.z):new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
    }
}
