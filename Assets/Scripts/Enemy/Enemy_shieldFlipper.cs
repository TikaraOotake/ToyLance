using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shieldFlipper : MonoBehaviour
{
    private bool isFacingRight = false;

    //èÇÇÃîΩì]èàóù
    public void UpdateShieldPosition(bool FacingRight)
    {
        if (FacingRight == isFacingRight)
        {
            return;
        }

        isFacingRight = FacingRight;

        float yRotation;

        if (FacingRight)
        {
            yRotation = 0f;
        }
        else
        {
            yRotation = 180f;
        }

        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y = yRotation;
        transform.localEulerAngles = currentRotation;
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
