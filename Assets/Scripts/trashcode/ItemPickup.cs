using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private UICanvas uic;
   
    // Start is called before the first frame update
    void Start()
    {
        uic = GameObject.Find("StageCanvas/").GetComponent<UICanvas>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         int itemp = uic.getItemCount() + 1;
    //         uic.setItemCount(itemp);
    //         this.gameObject.SetActive(false);
    //     }
    // }

   
}
