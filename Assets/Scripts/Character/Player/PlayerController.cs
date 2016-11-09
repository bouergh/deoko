using UnityEngine;
using System.Collections;

public class PlayerController : Character {

	[SerializeField] private GameObject sightsPrefab;
	[SerializeField] private GameObject projectilePrefab;
	private GameObject sights;
	private bool shooting = false;
	[SerializeField] private float fireRate;

	void Start() {
		base.Start ();
		sights = (GameObject)Instantiate (sightsPrefab, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!base.IsDead ()) {
			// move routine
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector2 direction = new Vector2 (moveHorizontal, moveVertical);
			base.Move (direction);

			// Shoot routine
			if (Input.GetMouseButton (0) && !shooting)
				StartCoroutine (ShootBullet ());
		}
    }

	void Update () { 
		// Sight indicator update
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); // get the real mouse position
		sights.transform.position = new Vector2(transform.position.x, transform.position.y) + (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized * 2;
		sights.transform.localScale = transform.localScale;
	}

	IEnumerator ShootBullet()
	{
		shooting = true;

		GameObject shotFired = (GameObject)Instantiate (projectilePrefab, transform.position, transform.rotation);
		shotFired.GetComponent<ProjectileController> ().Initialize (sights.transform.position - transform.position, "Player");

		//déclenchement de l'animation de tir
		base.PlayAnim(animType.shoot);

		yield return new WaitForSeconds (fireRate);
		shooting = false;
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
}

/*Remarque et idées futures :
 * Il faudrait bouger le joueur grâce à la physique du jeu sinon ça fout la merde ? oupa ? cf Isaac
 * */