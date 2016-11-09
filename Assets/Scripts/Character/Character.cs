using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour{
	[SerializeField] private int life;
	[SerializeField] private float speed;
	private bool facingRight = true;

	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
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

	private void Move(Vector2 direction)
	{
		if (anim != null)
			anim.SetFloat("walkSpeed", Vector2.Distance(Vector2.zero, direction));
		
		GetComponent<Rigidbody2D> ().velocity = direction * speed;
		if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0)) {
			facingRight = !facingRight;
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
	}
}
