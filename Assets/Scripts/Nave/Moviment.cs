using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.velocity = new Vector3(h, v, 0) * 5;
    }
}
