using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public Vector2 direction;
    public float speed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
