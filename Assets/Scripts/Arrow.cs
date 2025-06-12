using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //velocity에는 속도와  방향이 담김 
        transform.right = GetComponent<Rigidbody2D>().linearVelocity;
    }
}
