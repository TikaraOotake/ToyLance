using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shield : MonoBehaviour
{
    private float initialShieldX;
    private bool isFacingRight = false;

    private void Awake()
    {
        initialShieldX = transform.localPosition.x;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
