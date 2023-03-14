using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip[] clips;
    public float damage;
    public float lifeTime;
    public playercon owner;
    public float speed;
    public Rigidbody2D rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        audio.PlayOneShot(clips[0]);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audio.PlayOneShot(clips[1]);
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<playercon>().takeDamage(damage,owner);
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + 100);
        Destroy(gameObject);
    }

    public void onSpawn(float damage, float speed, playercon owner,float dir)
    {
        setDamage(damage);
        setOwner(owner);
        setSpeed(speed);
        rig.velocity = new Vector2(dir * speed, 0);
    }
    public void setOwner(playercon owner)
    {
        this.owner = owner;
    }
    public void setDamage(float damage)
    {
        this.damage = damage;
    }
    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed; 
    }

   
}
