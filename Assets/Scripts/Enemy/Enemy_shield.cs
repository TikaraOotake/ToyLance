using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackTypeEnums;

public class Enemy_shield : MonoBehaviour
{
    private float initialShieldX;
    //private bool isFacingRight = false;

    private Enemy_move enemyMove;

    public bool isBroken = false;

    Animator anim;

    [SerializeField] private GameObject Enemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        initialShieldX = transform.localPosition.x;

        if (enemyMove == null)
        {
            enemyMove = GetComponentInParent<Enemy_move>();
        }
    }

    //èÇÇÃîΩì]èàóù
    //public void UpdateShieldPosition(bool FacingRight)
    //{
    //    if (FacingRight == isFacingRight)
    //    {
    //        return;
    //    }

    //    isFacingRight = FacingRight;

    //    //float direction;

    //    //if (FacingRight)
    //    //{
    //    //    direction = 1f;
    //    //}
    //    //else
    //    //{
    //    //    direction = -11f;
    //    //}

    //    //Vector3 pos = transform.localPosition;
    //    //pos.x = initialShieldX * direction;
    //    //transform.localPosition = pos;

    //    float yRotation;

    //    if (FacingRight)
    //    {
    //        yRotation = 0f;
    //    }
    //    else
    //    {
    //        yRotation = 180f;
    //    }

    //    Vector3 currentRotation = transform.localEulerAngles;
    //    currentRotation.y = yRotation;
    //    transform.localEulerAngles = currentRotation;

    //    //foreach (Transform child in transform)
    //    //{
    //    //    var spear = child.GetComponent<SpearProjectile>();
    //    //    Debug.Log(spear);
    //    //    if (spear != null)
    //    //    {
    //    //        //ëÑÇ™èÇÇ…éhÇ≥Ç¡ÇΩéûÇÃîΩì]èàóù
    //    //        spear.FlipInShield();
    //    //    }
    //    //}
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) 
        {
            return;
        }

        SpearAttack spear = collision.GetComponent<SpearAttack>();
        if (spear != null) 
        {
            AttackType attackType= spear.GetAttackType();
            if (attackType == AttackType.Trust) 
            {
                GameObject Player = GameManager_01.GetPlayer();
                float PlayerPosX = 0.0f;
                float EnemyPosX = 0.0f;
                float ShieldPosX = 0.0f;

                if (Player != null && Enemy != null)
                {
                    PlayerPosX = Player.transform.position.x;
                    EnemyPosX = Enemy.transform.position.x;
                    ShieldPosX = transform.position.x;
                }

                float minX = Mathf.Min(PlayerPosX, ShieldPosX);
                float maxX = Mathf.Max(PlayerPosX, ShieldPosX);

                bool isEnemyBetween = EnemyPosX > minX && EnemyPosX < maxX;

                if (!isEnemyBetween)
                {
                    //èÇÇÃîjâÛèàóù
                    BreakShield();
                }
            }
        }

        //if (collision.CompareTag("PlayerMeleeAttack")) 
        //{
        //    var meleeHitbox = collision.GetComponent<MeleeHitbox>();
        //    if (meleeHitbox != null) 
        //    {
        //        var player = meleeHitbox.GetComponentInParent<Player_move>();
        //        if (player != null && !player.isDownAttacking)
        //        {
        //            BreakShield();
        //        }
        //    }
        //}

        if (collision.GetComponent<SpearProjectile>() != null)
        {
            return;
        }
    }

    //èÇÇÃîjâÛèàóù
    private void BreakShield()
    {
        if (isBroken)
        {
            return;
        }

        isBroken = true;

        if (enemyMove != null)
        {
            //í‚é~èàóù
            enemyMove.PauseMovement(1f);
        }

        gameObject.SetActive(false);

        Invoke(nameof(RestoreShield), 6f);
    }

    //èÇÇÃçƒê∂èàóù
    private void RestoreShield()
    {
        isBroken = false;

        gameObject.SetActive(true);
    }

    //èÇÇÃÉAÉjÉÅÅ[ÉVÉáÉìÇÃçƒê∂èàóù
    public void SetWalkSpeed(int speed)
    {
        if (anim != null)
        {
            anim.SetInteger("WalkSpeed", speed);
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
