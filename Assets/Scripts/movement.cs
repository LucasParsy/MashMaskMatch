using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Transform t;

    void Start()
    {
       t = GetComponent<Transform>();        
    }

    private void FixedUpdate() {
        t.Rotate(Vector3.one);
    }

}
