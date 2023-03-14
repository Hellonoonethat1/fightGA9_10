using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercon : MonoBehaviour
{

    [Header("Max Values")]
    public int maxHp;
    public int maxJumps;
    public float maxSpeed;
    public float curattacktim;
    public float lastDuration;
    public float slowTime;
    public float max_charge_dmg;



    [Header("Cur Values")]
    public int curHP;
    public int curJumps;
    public int score;
    public float CurMoveInput;
    public int DieCount;
    public float lastHit;
    public float lastHitIce;
    public bool isSlowed;
    public float charge_dmg;
    public float charge_rate;
    public bool ischarging;

    [Header("Atttacking Section")]
    public playercon curAttacker;
    public float attackDmg;
    public float attackSpeed;
    public float attackRate;
    public float IceAttackSpeed;
    public float lastAttackTime;
    public GameObject[] attackPrefabs;


    [Header("MODS")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Audio Clips")]
    //jump abd 0
    // land and 1
    // taunt_1 2
    //dead sound
    public AudioClip[] playerfx_list;

    [Header("Componets")]
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource audio;
    private Transform muzzle;
    public GAmermanager gAmermanager;
    public playercontainer playerUI;
    public SpriteRenderer sr;
    public GameObject deatheffect;
    
    // Start is called before the first frame update
    void Start()
    {

        curHP = maxHp;
        curJumps = maxJumps;
        score = 0;
        moveSpeed = maxSpeed;

    }
    public void setColor(Color color)
    {
        sr.color = color;
    }
    // Update is called once per frame
    void Update()
    {
        move();
     if (transform.position.y < -10)
            {
            die();

        }
     if (transform.position.y < -10)
        {
            die();

        }
     if (curAttacker)
        {
            if(Time.time - lastHit > curattacktim)
            {
                curAttacker = null;
            }
        }
     if (isSlowed)
        {
            if(Time.time - lastHitIce > slowTime)
            {
                isSlowed = false;
                moveSpeed = maxSpeed;
            }
        }
     if (ischarging)
        {
            charge_dmg += charge_rate;
            if(charge_dmg > max_charge_dmg)
            {
                charge_dmg = max_charge_dmg;
            }
        }
    }
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        muzzle = GetComponentInChildren<MU>().GetComponent<Transform>();
        gAmermanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GAmermanager>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D hit in collision.contacts)
        {
            if(hit.collider.CompareTag("Platforms"))
            {
                if(hit.point.y < transform.position.y)
                {
                    curJumps = maxJumps;
                }    
            }
            {
                if((hit.point.x > transform.position.x || hit.point.x < transform.position.x) && hit.point.y < transform.position.y)
                {
                    curJumps++;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void move()
    {
        if (ischarging)
        {
            CurMoveInput *= .5f;
        }
        print("is moving");
        rig.velocity = new Vector2(CurMoveInput * moveSpeed, rig.velocity.y);

        // player direction
        if(CurMoveInput != 0)
        {
            transform.localScale = new Vector3(CurMoveInput > 0 ? 1 : -1, 1, 1);
        }
    }
    private void jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, 0);
        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        audio.PlayOneShot(playerfx_list[2]);
    }
    
    public void die()
    {
        Destroy(Instantiate(deatheffect, transform.position, Quaternion.identity), 1f);
        print("PLAYER IS DEAD");
        //DEAD SOUND 
      
        if(curAttacker != null)
        {
            curAttacker.addScore();
        }
        else
        {
            score--;
           
            if (score < 0)
            {
                score = 0;
            }
            playerUI.updateScoreText(score);
        }
        respawn();

    }
    public void addScore()
    {
        score++;
        playerUI.updateScoreText(score); 
    }

    public void drop_out()
    {
        Destroy(playerUI.gameObject);
        Destroy(gameObject);
    }
    public void takeDamage(int ammount, playercon attacker)
    {
        curHP -= ammount;
        curAttacker = attacker;
        lastHit = Time.time;
    }
    public void takeDamage(float ammount, playercon attacker)
    {
        curHP -= (int)ammount;
        curAttacker = attacker;
        lastHit = Time.time;
        playerUI.updateHealthBar(curHP, maxHp);
        if (ischarging)
        {
            charge_dmg /= 2;
        }
    }
    

    public void takeIceDamage(float ammount, playercon attacker)
    {
        curHP -= (int)ammount;
        curAttacker = attacker;
        lastHit = Time.time;
        isSlowed = true;
        lastHit = Time.time;
        lastHitIce = Time.time;
        moveSpeed /= 2;
        if (ischarging)
        {
            charge_dmg /= 2;
        }
    }

    private void respawn()
    {
        curHP = maxHp;
        curJumps = maxJumps;
        curAttacker = null;
        rig.velocity = Vector2.zero;
        transform.position = gAmermanager.spawn_points[Random.Range(0, gAmermanager.spawn_points.Length)].position;
    }

    // input action map methods
    //move input
    public void onMoveInput(InputAction.CallbackContext context)
    {
        print("pressed move button");
        CurMoveInput = context.ReadValue<float>();
    }
    public void onJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (curJumps > 0)
            {
                curJumps--;
                jump();
            }
        }
    }
    public void onBlockInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed block button");
        }
    }
    public void onAttackStandardInput(InputAction.CallbackContext context)
    {
        print("WORK");
        if (context.phase == InputActionPhase.Performed&& Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            spawn_std_attack();
        }
        if(ischarging)
        {
            ischarging = false;
            charge_dmg = 0;
        }

    }

    public void spawn_std_attack()
    {
       
        GameObject Fireball = Instantiate(attackPrefabs[0], muzzle.position, Quaternion.identity);
        Fireball.GetComponent<Projectile>().onSpawn(attackDmg, attackSpeed, this, transform.localScale.x);
    }
    
    public void onAttackChargeInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed Charged Attack button");
            ischarging = true;
          
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //toggle off charge bool
            ischarging = false;
            //spawn attack
            spawn_charge_attack();
            //set dmg value to 0
            charge_dmg = 0;
           
        }
    }
    public void spawn_charge_attack()
    {

        GameObject FredtheDangerBall = Instantiate(attackPrefabs[2], muzzle.position, Quaternion.identity);
        FredtheDangerBall.GetComponent<Projectile>().onSpawn(charge_dmg, attackSpeed, this, transform.localScale.x);
    }

    public void setUI(playercontainer playerUI)

    {
        this.playerUI = playerUI;
    }





    public void onAttackIceInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (context.phase == InputActionPhase.Performed && Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                spawn_ice_attack();
            }


        }
    }
    public void spawn_ice_attack()
    {
        GameObject Iceball = Instantiate(attackPrefabs[1], muzzle.position, Quaternion.identity);
        Iceball.GetComponent<Projectile>().onSpawn(attackDmg, attackSpeed, this, transform.localScale.x);
    }
    public void onPauseInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed pause button");
        }
    }
    public void onTauntUnoInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed Taunt Nombre Uno button");
        }
    }
    public void onTauntDosInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("Taunt Nombre Dos button");































        }
    }
}
