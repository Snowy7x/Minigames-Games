using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLook : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    
    void Update()
    {
        if (Camera.main == null) return;
        // THIS IS INVERSED: transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
