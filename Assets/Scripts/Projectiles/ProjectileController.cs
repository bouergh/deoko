using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    //these variables are set by instance which instantiates the projectile
    private Vector2 direction;
	[SerializeField] private float speed;
    private string origin;
	[SerializeField] private int damage;

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
            other.GetComponent<EnemyController>().TakeDamage(damage);
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
    public void Initialize(Vector2 direction, string origin)
    {
        this.direction = direction;
        this.origin = origin;
    }
}
