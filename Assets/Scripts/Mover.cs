using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    //these variables are set by instance which instantiates the projectile
    public Vector2 direction;
    public float speed;
    public string origin;   //make it some enum ?

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) // à modifier dans le cas où le projectile est envoyé par un ennemi ? ou faire autre script ?
    {
        if (other.tag == "Enemy" && origin != "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);    //replace by "TakeDamage"
        }

        if (other.tag == "Player" && origin != "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(10);
            Destroy(this.gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
