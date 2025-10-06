using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shieldFlipper : MonoBehaviour
{
    private bool isFacingRight = false;     //�E�������Ă��邩

    //���̔��]����
    public void UpdateShieldPosition(bool FacingRight)
    {
        if (FacingRight == isFacingRight)
        {
            return;
        }

        isFacingRight = FacingRight;

        float yRotation;

        //�E�������Ă���Ȃ�
        if (FacingRight)
        {
            //0��
            yRotation = 0f;
        }
        else
        {
            //180��
            yRotation = 180f;
        }

        //���݂̉�]���擾
        Vector3 currentRotation = transform.localEulerAngles;
        //��]���X�V
        currentRotation.y = yRotation;
        //��]��K�p
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
