using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour
{
    public static BackgroundControl Instance;
    [Header("滚动速度")][Range(1,100)]public float _speed=1f;
    // Start is called before the first frame update
    void Awake()
    {
        Instance=this;
    }
}
