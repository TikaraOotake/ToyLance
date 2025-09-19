using System.Collections;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private bool isTouched = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            player.SetLanceNum(999);
            Destroy(this.gameObject);
        }
    }
}
