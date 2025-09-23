using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject CameraArea;//�J�����G���A

    [SerializeField]
    private float ChaseRate = 1.0f;

    [SerializeField]
    private float AdjustCameraPosX = 0.0f;
    [SerializeField]
    private int CameraWayX = 0;

    [SerializeField]
    private Vector2 CameraGazePos;//�J�����̒������W

    //�U���Ɋւ��ϐ�
    private Vector2 ShakeRot_Vec;//�U���x�N�g��
    private float ShakeRot_Length;//���a
    [SerializeField]
    private float ShakeRot_Speed = 1.0f;//��]���x
    [SerializeField]
    private float ShakeRot_LowValue = 1.0f;//������
    private float ShakeRot_Angle = 0.0f;//�p�x

    private Vector2 cameraShakeOffset = Vector2.zero;

    private void Awake()
    {
        

        //�J����Manager�ɃJ�������Z�b�g
        CameraManager.SetCamera(this.gameObject);

    }
    void Start()
    {
        if (player == null) player = GameManager_01.GetPlayer();//�v���C���[���擾
        if (player == null) Debug.Log("�v���C���[���ݒ肳��Ă��܂���");

        //�J�����̏������W���v���C���[�̈ʒu�ɐݒ�
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }

        //�������W���Z�b�g
        CameraGazePos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            CameraWayX = 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CameraWayX = -1;
        }
    }


    public void SetShakeCamera()
    {
        const float Length = 0.2f;
        const float Speed = 10.0f;
        const float LowValue = 10.0f;
        SetShakeCamera(Length, Speed, LowValue);
    }
    public void SetShakeCamera(float _Length, float _Speed, float _LowValue)
    {
        ShakeRot_Length = _Length;   //���a
        ShakeRot_Speed = _Speed;     //��]���x
        ShakeRot_LowValue = _LowValue;   //������
        ShakeRot_Angle = Random.Range(0f, 360f);    //�p�x�̓����_���ɃX�^�[�g
    }
    //�J�����U���̍X�V����
    private void ShakeCamera_Update()
    {
        //�U�����I�����Ă���Ή������Ȃ�
        if (ShakeRot_Length <= 0.01f)
        {
            ShakeRot_Vec = Vector2.zero;
            return;
        }

        //�p�x�����Z���ĉ�]�i���t���[���j
        ShakeRot_Angle += ShakeRot_Speed * Time.unscaledDeltaTime * 360f; //1�b��1��]
        if (ShakeRot_Angle >= 360f)
        {
            ShakeRot_Angle -= 360f;
        }

        //�~�^���x�N�g�����v�Z
        float rad = ShakeRot_Angle * Mathf.Deg2Rad;
        ShakeRot_Vec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * ShakeRot_Length;

        //����������
        ShakeRot_Length = Mathf.Lerp(ShakeRot_Length, 0f, ShakeRot_LowValue * Time.unscaledDeltaTime);

        //�J�����̈ʒu�ɐU����������
        cameraShakeOffset = (Vector3)ShakeRot_Vec;
    }

    public void StartShake(float length, float speed, float lowValue)
    {
        ShakeRot_Length = length;   //���a
        ShakeRot_Speed = speed;     //��]���x
        ShakeRot_LowValue = lowValue;   //������
        ShakeRot_Angle = Random.Range(0f, 360f);    //�p�x�̓����_���ɃX�^�[�g
    }

    void FixedUpdate()
    {
        ShakeCamera_Update();

        //�ڕW���W���Z�b�g
        Vector2 TargetPos =  GetTargetPos();

        //�����_�̍��W�v�Z
        Vector2 pos = CameraGazePos;
        float rate = 1.0f - Mathf.Pow(1.0f - ChaseRate, Time.deltaTime * 60f); // �b��ChaseRate�ɂȂ�悤��
        pos.x += (TargetPos.x - pos.x) * rate;
        pos.y += (TargetPos.y - pos.y) * rate;
        CameraGazePos = pos;

        //Nan�`�F�b�N
        if(!(CameraGazePos.x + CameraGazePos.y >= 0.0f) &&
           !(CameraGazePos.x + CameraGazePos.y <= 0.0f))
        {
            return;//Nan�Ǝv���鐔�l�����������ߏI��
        }

        //�J�����̍��W���Z�b�g
        float CameraZ = transform.position.z;//z����񂪏����Ă��܂����ߕێ�
        transform.position = CameraGazePos + cameraShakeOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, CameraZ);//Z���W��߂�
    }

    private Vector2 GetTargetPos()
    {
        Vector2 TargetPos = new Vector2(0.0f, 0.0f);

        //�ڕW���W���Z�b�g
        if (CameraArea)//�J�����G���A
        {
            CameraScrollArea _cameraScrollArea = CameraArea.GetComponent<CameraScrollArea>();
            if (_cameraScrollArea)
            {
                //�J�����X�N���[���G���A
                TargetPos = _cameraScrollArea.GetCameraPos();
            }
            else
            {
                //�J�������b�N�G���A
                TargetPos = CameraArea.transform.position;
                if (player)
                {
                    const float AttractRate = 0.1f;//�ꊄ�قǃv���C���[�Ɋ񂹂�
                    Vector3 tempVec = player.transform.position - CameraArea.transform.position;
                    TargetPos += new Vector2(tempVec.x, 0.0f) * AttractRate;
                }
            }
        }
        else
        if (player)//�ʏ펞
        {
            TargetPos = player.transform.position;
            TargetPos.x += AdjustCameraPosX * CameraWayX;
        }

        return TargetPos;
    }

    public void SetCameraArea(GameObject _area)
    {
        CameraArea = _area;
    }
    public GameObject GetCameraArea()
    {
        return CameraArea;
    }
    public void SetCameraGazePos(Vector2 _pos)
    {
        CameraGazePos = _pos;
        Vector3 cameraPos = _pos;
        cameraPos.z = transform.position.z;
        transform.position = cameraPos;
    }

    public void ResetCameraPos()
    {
        CameraGazePos = GetTargetPos();

        //Nan�`�F�b�N
        if (!(CameraGazePos.x + CameraGazePos.y >= 0.0f) &&
           !(CameraGazePos.x + CameraGazePos.y <= 0.0f))
        {
            return;//Nan�Ǝv���鐔�l�����������ߏI��
        }

        //�J�����̍��W���Z�b�g
        float CameraZ = transform.position.z;//z����񂪏����Ă��܂����ߕێ�
        transform.position = CameraGazePos + cameraShakeOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, CameraZ);//Z���W��߂�
    }
}
