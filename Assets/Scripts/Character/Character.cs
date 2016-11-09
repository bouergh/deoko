using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour{
	[SerializeField] private int life;
	[SerializeField] private float speed;
	private bool facingRight = true;

	private Animator anim;
	public enum animType {death = 0, shoot, walk};

	protected void Start() {
		anim = GetComponent<Animator>();
	}

	virtual public void TakeDamage(int damageTaken)
	{
		if (!IsDead()) {
			life = Mathf.Max (life - damageTaken, 0);

			if (IsDead()) { //si ce dégât vient de nous tuer
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //on arrête de bouger
				PlayAnim(animType.death);
			}
		}
	}

	protected bool IsDead()
	{
		return life <= 0;
	}

	protected void Move(Vector2 direction)
	{
		GetComponent<Rigidbody2D> ().velocity = direction * speed;
		if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0)) {
			facingRight = !facingRight;
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
		PlayAnim(animType.walk, Vector2.Distance(Vector2.zero, direction));
	}

	protected void PlayAnim(animType animationType, float parameter = 0f) {
		if (anim != null) {
			switch (animationType) {
			case animType.death:
				anim.SetTrigger ("death");
				break;
			case animType.shoot:
				//on devra peut-être mettre cette animation dans une interface "IShoot" pour rester cohérent (car tous les "characters" ne tirent pas)
				//Je l'ai mis ici pour simplifier dans un premier temps : le player en a besoin
				anim.SetTrigger ("fire");
				break;
			case animType.walk:
				anim.SetFloat ("walkSpeed", parameter);
				break;
			}
		}
	}
}
