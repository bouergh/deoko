using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    //these variables are set by instance which instantiates the projectile
    public Vector2 direction;
    public float speed;
    public string origin;   //make it some enum ?
    public int damage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = direction * speed;   //projectile goes towards its initial direction, at same speed, for the moment
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && origin != "Enemy")
        {
            other.GetComponent<EnemyScript>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.tag == "Player" && origin != "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    //when a projectile is instantiated, this script is associated and we call this function to initialize projectile parameters
    public void Initialize(Vector2 direction, float speed, string origin, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.origin = origin;
        this.damage = damage;
    }
}
