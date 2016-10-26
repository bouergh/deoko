using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject shot;
    public float fireRate;
	public int life;
    private float fireTimer;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

        fireTimer = fireRate;
		life = 100;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isDead ()) {
			fireTimer = Mathf.Max (fireTimer - Time.fixedDeltaTime, 0f);

			// Movement routine
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
			GetComponent<Rigidbody2D> ().velocity = movement * speed;

			// Shooting routine
			if (Input.GetAxis ("HorizontalShot") != 0 || Input.GetAxis ("VerticalShot") != 0)
				shotBullet ();
		}
    }

	private void shotBullet()
	{
		float shotHorizontal = Input.GetAxis("HorizontalShot");
		float shotVertical = Input.GetAxis("VerticalShot");
		Vector2 direction = new Vector2(shotHorizontal, shotVertical);
		direction.Normalize();

		if (direction != Vector2.zero && fireTimer <= 0) {
			GameObject shotFired = (GameObject)Instantiate (shot, transform.position, transform.rotation);
			shotFired.GetComponent<Mover> ().direction = direction;
			fireTimer = fireRate;

			//déclenchement de l'animation de tir
			anim.SetTrigger ("fire");
		}
	}

	private void takeDamage(int damage)
	{
		if (!isDead()) {
			life = Mathf.Max (life - damage, 0);

			if (isDead()) { //si ce dégât vient de nous tuer
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //on arrête de bouger
				//animation de la mort
				anim.SetTrigger ("death");
			}
		}
	}

	private bool isDead()
	{
		return life <= 0;
	}
}
