﻿using UnityEngine;
using System.Collections;

public class PlayerController : Character {

	//viseur et projectiles
	[SerializeField] private GameObject sightsPrefab;
	[SerializeField] private GameObject projectilePrefab;

	//barre de vie
	private RectTransform lifeBarUIMaskRect;
	private Vector2 lifeBarWidth;
	private int totalLife;

	private GameObject sights;
	private bool shooting = false;
	[SerializeField] private float fireRate;


	protected override void Start() {
		base.Start ();
		sights = (GameObject)Instantiate (sightsPrefab, transform.position, Quaternion.identity);
		sights.transform.parent = transform;

		lifeBarUIMaskRect = GameObject.Find("UI canvas/LifeBar/mask").GetComponent<RectTransform>();
		lifeBarWidth = lifeBarUIMaskRect.sizeDelta;
		totalLife = getLife ();
	}

	public override void TakeDamage(int damageTaken) {
		base.TakeDamage (damageTaken);
		//updates the life bar in the UI
		lifeBarUIMaskRect.sizeDelta = new Vector2((getLife() * lifeBarWidth.x) / totalLife, lifeBarWidth.y);
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

		if ((FacingRight && sights.transform.position.x < transform.position.x) || (!FacingRight && sights.transform.position.x > transform.position.x)) {
			base.Flip ();
		}
	}

	IEnumerator ShootBullet()
	{
		shooting = true;
		GameObject shotFired = (GameObject)Instantiate (projectilePrefab, transform.position, transform.rotation);
		shotFired.GetComponent<ProjectileController> ().Initialize (sights.transform.position - transform.position, gameObject);    //répercussion de la modification des projectiles


		//déclenchement de l'animation de tir
		base.PlayAnim(animType.shoot);

		yield return new WaitForSeconds (fireRate);
		shooting = false;
	}
}

/*Remarque et idées futures :
 * Il faudrait bouger le joueur grâce à la physique du jeu sinon ça fout la merde ? oupa ? cf Isaac
 * */