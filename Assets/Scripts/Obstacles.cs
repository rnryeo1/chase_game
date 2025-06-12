using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject itemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        colInit();
    }

  
    void colInit()
    {
        for (int i = 0; i < Random.Range(0, 5); i++)
        {
            GameObject go = Instantiate(itemPrefab, new Vector3(transform.position.x, transform.position.y + i,
                transform.position.z), transform.rotation) as GameObject;

            Transform t = go.transform;
            t.localScale = new Vector3(0.1f, 0.1f, 0);
            t.SetParent(transform);
        }
    }
}
