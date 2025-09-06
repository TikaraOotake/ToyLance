using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    [Header("¿Ãµø º≥¡§")]
    public float moveSpeed = 3f;      
    public float leftBoundary = -9f;  
    public float rightBoundary = 9f;  

    [Header("¡°«¡(ª–ª–) º≥¡§")]
    public float hopHeight = 0.3f; 
    public float hopFrequency = 5f; 

    private int direction = 1; 
    private SpriteRenderer spriteRenderer;
    private float startPosY;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosY = transform.position.y;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        float newY = startPosY + Mathf.Sin(Time.time * hopFrequency) * hopHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (transform.position.x > rightBoundary && direction == 1)
        {
            direction = -1;
            spriteRenderer.flipX = true; 
        }
        else if (transform.position.x < leftBoundary && direction == -1)
        {
            direction = 1;
            spriteRenderer.flipX = false; 
        }
    }
}
