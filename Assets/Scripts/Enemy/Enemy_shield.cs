using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shield : MonoBehaviour
{
    private float initialShieldX;
    private bool isFacingRight = false;

    private Enemy_move enemyMove;

    private bool isBroken = false;

    private void Awake()
    {
        initialShieldX = transform.localPosition.x;

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }
    }

    //èÇÇÃîΩì]èàóù
    public void UpdateShieldPosition(bool FacingRight)
    {
        if (FacingRight == isFacingRight)
        {
            return;
        }

        isFacingRight = FacingRight;

        float direction;

        if (FacingRight)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        }

        Vector3 pos = transform.localPosition;
        pos.x = initialShieldX * direction;
        transform.localPosition = pos;

        foreach (Transform child in transform)
        {
            var spear = child.GetComponent<SpearProjectile>();
            if (spear != null)
            {
                //ëÑÇ™èÇÇ…éhÇ≥Ç¡ÇΩéûÇÃîΩì]èàóù(Reversal process when a spear is stuck in a shield)
                spear.FlipInShield();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        if (collision.CompareTag("PlayerMeleeAttack")) 
        {
            var meleeHitbox=collision.GetComponent<MeleeHitbox>();
            if(meleeHitbox != null)
            {
                var player = meleeHitbox.GetComponentInParent<Player_move>();
                if (player != null && !player.isDownAttacking) 
                {
                    BreakShield();
                }
            }
        }
        if (collision.GetComponent<SpearProjectile>() != null)
        {
            return;
        }
    }

    private void BreakShield()
    {
        if (isBroken)
        {
            return;
        }

        isBroken = true;

        if (enemyMove != null)
        {
            enemyMove.PauseMovement(1f);
        }

        gameObject.SetActive(false);

        Invoke(nameof(RestoreShield), 6f);
    }

    private void RestoreShield()
    {
        isBroken = false;

        gameObject.SetActive(true);
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
