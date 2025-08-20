using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Hp;

    public Player_move player;

    // ü�� ����
    public void HpDown()
    {
        if (Hp > 1)
            Hp--;
        else
        {
            player.OnDie();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HpDown();
            if (Hp > 0)
            {
                PlayerRePosition();
            }
        }
    }

    // ����ġ
    void PlayerRePosition()
    {
        player.transform.position = new Vector3(-5, 0, 0);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
