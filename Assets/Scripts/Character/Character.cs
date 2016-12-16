using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour{
    [SerializeField] protected int maxLife;
    protected int life;
	[SerializeField] protected float speed;
	private GameManager gameManager;

	private Animator anim;
	public enum animType {death = 0, shoot, walk};

	private bool facingRight = true;
	public bool FacingRight {
		get { return facingRight; }
		set { facingRight = value; }
	}


	virtual protected void Start() {
		anim = GetComponent<Animator>();
        life = maxLife;
		gameManager = GameObject.Find ("BoardManager").GetComponent<GameManager> ();
	}

	virtual public void TakeDamage(int damageTaken)
	{
		if (!IsDead()) {
			life = Mathf.Max (life - damageTaken, 0);

			if (IsDead()) { //si ce dégât vient de nous tuer
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //on arrête de bouger
				PlayAnim(animType.death);
                if(tag != "Player")
                {
                    Destroy(this.gameObject);
                }
			}
		}
	}

	protected bool IsDead()
	{
		return life <= 0;
	}

	protected void Move(Vector2 direction)
	{
		if (!gameManager.isPlayerDead ()) {
			float walkSpeed = ((facingRight && direction.x >= 0) || (!facingRight && direction.x <= 0)) ? direction.magnitude : direction.magnitude * -1;
			PlayAnim (animType.walk, walkSpeed);

			GetComponent<Rigidbody2D> ().velocity = direction * speed;
		} else {
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			EnemyController ec = GetComponent<EnemyController> ();
			if (ec != null) {
				Destroy (ec);
			}
		}
	}

	protected void Flip() {
		facingRight = !facingRight;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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
