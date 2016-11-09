using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField] private int life;
    [SerializeField] private float speed;
	private bool facingRight = true;
    
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!IsDead ()) {
			// Movement routine
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
			anim.SetFloat("walkSpeed", Vector2.Distance(Vector2.zero, movement));
			GetComponent<Rigidbody2D> ().velocity = movement * speed;
			if ((facingRight && moveHorizontal < 0) || (!facingRight && moveHorizontal > 0)) {
				facingRight = !facingRight;
				transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
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