using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(destorycoroutine());
    }


    IEnumerator destorycoroutine()
    {
        yield return new WaitForSeconds(0.11f);//5초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
    }


}
