using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int life;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject shot;
    [SerializeField]
    private float fireRate;
    private float fireTimer;
    [SerializeField]
    private float projectSpeed;

    
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
        fireTimer = fireRate;
		life = 100;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!IsDead ()) {
			fireTimer = Mathf.Max (fireTimer - Time.fixedDeltaTime, 0f);

			// Movement routine
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
			GetComponent<Rigidbody2D> ().velocity = movement * speed;

			// Shooting routine
			if (Input.GetAxis ("HorizontalShot") != 0 || Input.GetAxis ("VerticalShot") != 0)
				ShotBullet ();
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")        //catches if enemy is close enough to stop and make damage
        {
            other.GetComponentInParent<EnemyScript>().TouchPlayer(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyScript>().TouchPlayer(false);
        }
    }

	private void ShotBullet()
	{
		float shotHorizontal = Input.GetAxis("HorizontalShot");
		float shotVertical = Input.GetAxis("VerticalShot");
		Vector2 direction = new Vector2(shotHorizontal, shotVertical);
		direction.Normalize();

		if (direction != Vector2.zero && fireTimer <= 0) {
			GameObject shotFired = (GameObject)Instantiate (shot, transform.position, transform.rotation);
			shotFired.GetComponent<Mover> ().direction = direction;
            shotFired.GetComponent<Mover>().speed = projectSpeed;
            shotFired.GetComponent<Mover>().origin = "Player";
            fireTimer = fireRate;

			//déclenchement de l'animation de tir
			anim.SetTrigger ("fire");
		}
	}

	public void TakeDamage(int damage)
	{
		if (!IsDead()) {
			life = Mathf.Max (life - damage, 0);

			if (IsDead()) { //si ce dégât vient de nous tuer
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //on arrête de bouger
				//animation de la mort
				anim.SetTrigger ("death");
			}
		}
	}

	private bool IsDead()
	{
		return life <= 0;
	}
}

/*Remarque et idées futures :
 * Il faudrait bouger le joueur grâce à la physique du jeu sinon ça fout la merde ? oupa ? cf Isaac
 * */