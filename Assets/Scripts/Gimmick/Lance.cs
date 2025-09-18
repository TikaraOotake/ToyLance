using System.Collections;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private bool isTouched = false;

    private SEManager _seManager;

    // Start is called before the first frame update
    void Start()
    {
        _seManager = Camera.main.GetComponent<SEManager>();
        if (_seManager == null) Debug.Log("SE‚ÌŽæ“¾‚ÉŽ¸”s");
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
            _seManager.PlaySE("get");
            player.SetLance(isTouched);
            player.SetLanceNum(999);
            Destroy(this.gameObject);
        }
    }
}
