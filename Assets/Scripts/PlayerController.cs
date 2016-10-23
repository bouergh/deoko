using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject shot;
    public float fireRate;
    private float fireTimer;

	// Use this for initialization
	void Start () {
        fireTimer = fireRate;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        fireTimer -= Time.fixedDeltaTime;

        // Movement routine
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        // Shooting routine
        float shotHorizontal = Input.GetAxis("HorizontalShot");
        float shotVertical = Input.GetAxis("VerticalShot");
        Vector2 direction = new Vector2(shotHorizontal, shotVertical);
        direction.Normalize();
        if(direction != Vector2.zero && fireTimer <= 0)
        {
            GameObject shotFired = (GameObject)Instantiate(shot, transform.position, transform.rotation);
            shotFired.GetComponent<Mover>().direction = direction;
            fireTimer = fireRate;
        }
        
    }
}
