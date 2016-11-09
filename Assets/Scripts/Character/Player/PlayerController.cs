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
		if (!IsDead())
        {
            Move(); // Movement routine
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")        //catches if enemy is close enough to stop and make damage
        {
            other.GetComponentInParent<EnemyController>().TouchPlayer(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
			other.GetComponentInParent<EnemyController>().TouchPlayer(false);
        }
    }

	virtual public void TakeDamage(int damageTaken)
	{
		if (!IsDead()) {
			life = Mathf.Max (life - damageTaken, 0);

			if (IsDead()) { //si ce dégât vient de nous tuer
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //on arrête de bouger
				if (anim != null)
					anim.SetTrigger ("death"); //animation de la mort
			}
		}
	}

	private bool IsDead()
	{
		return life <= 0;
	}

	private void Move()
	{
		Vector2 direction = Vector2.zero;
		if (anim != null)
			anim.SetFloat("walkSpeed", Vector2.Distance(Vector2.zero, direction));

		GetComponent<Rigidbody2D> ().velocity = direction * speed;
		if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0)) {
			facingRight = !facingRight;
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
	}
}

/*Remarque et idées futures :
 * Il faudrait bouger le joueur grâce à la physique du jeu sinon ça fout la merde ? oupa ? cf Isaac
 * */