using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockArea : MonoBehaviour
{
    private GameObject Camera;
    private GameObject Player;
    void Start()
    {
        Camera = CameraManager.GetCamera();

        //“–‚½‚è”»’èŠm”F—p‚ÌƒŒƒ“ƒ_ƒ‰[“§–¾‰»
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        if (_sr)
        {
            _sr.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    private void Update()
    {
        if (Camera != null)
        {
            CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
            if (_camera_cs)
            {
                if (Player)
                {
                    //_camera_cs.SetCameraArea(this.gameObject);
                }
                else
                {
                    if (_camera_cs.GetCameraArea() == this.gameObject)
                    {
                        //_camera_cs.SetCameraArea(null);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Camera != null)
            {
                CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
                if (_camera_cs)
                {
                    //’N‚à“o˜^‚³‚ê‚Ä‚¢‚È‚©‚Á‚½‚ç©g‚ğ“o˜^
                    if (_camera_cs.GetCameraArea() == null)
                    {
                        _camera_cs.SetCameraArea(this.gameObject);
                    }
                }
            }
                
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            //Player = null;
            if (Camera != null)
            {
                CameraControl _camera_cs = Camera.GetComponent<CameraControl>();
                if (_camera_cs)
                {
                    //©g‚ª“o˜^‚³‚ê‚Ä‚¢‚½ê‡‰ğœ
                    if (_camera_cs.GetCameraArea() == this.gameObject)
                    {
                        _camera_cs.SetCameraArea(null);
                    }
                }
            }
        }
    }
}
