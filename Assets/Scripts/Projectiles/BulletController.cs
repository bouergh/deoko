using UnityEngine;
using System.Collections;

public class BulletController : ProjectileController{
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = direction * speed;   //projectile goes towards its initial direction, at same speed, for the moment
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Enemy" && origin.tag != "Enemy")  //enemy has trigger collider for range detection so we only check non-trigger colliders
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.tag == "Player" && origin.tag != "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
