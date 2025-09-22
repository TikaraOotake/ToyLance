using System.Collections;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private bool isTouched = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTouched)
        {
            isTouched = true;
        }

        Player_01_Control player = collision.gameObject.GetComponent<Player_01_Control>();
        if (player != null) 
        {
            //Žæ“¾‰¹
            SEManager.instance.PlaySE("get");
            player.SetLance(isTouched);
            player.SetGetItem();
            player.SetLanceNum(999);
            Destroy(this.gameObject);
        }
    }
}
