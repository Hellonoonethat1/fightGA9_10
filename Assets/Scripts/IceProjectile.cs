using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<AudioSource>().PlayOneShot(clips[1]);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<playercon>().takeIceDamage(damage, owner);
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + 100);
        Destroy(gameObject);
    }

}
