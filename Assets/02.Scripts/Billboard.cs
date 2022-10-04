using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTr;
    private Transform tr;

    void Start()
    {
        camTr = Camera.main.transform; //Camera.main.GetComponent<Transform>();
        tr = transform; // GetComponent<Transform>();
    }

    void LateUpdate()
    {
        tr.LookAt(camTr);
    }
}
