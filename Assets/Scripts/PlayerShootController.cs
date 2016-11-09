using UnityEngine;
using System.Collections;

public class PlayerShootController : MonoBehaviour {

	[SerializeField] private GameObject sightsPrefab;
	private GameObject sights;

	[SerializeField] private float fireRate;
	[SerializeField] private GameObject projectilePrefab;
	private bool shooting = false;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		sights = (GameObject)Instantiate (sightsPrefab, transform.position, Quaternion.identity);
		Debug.Log (sights);
	}
	
	// Update is called once per frame
	void Update () { 
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); // get the real mouse position
		sights.transform.position = new Vector2(transform.position.x, transform.position.y) + (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized * 2;
		sights.transform.localScale = transform.localScale;

		// Shooting routine
		if (Input.GetMouseButton(0) && !shooting)
			StartCoroutine(ShootBullet ());
	}

	IEnumerator ShootBullet()
	{
		shooting = true;

		GameObject shotFired = (GameObject)Instantiate (projectilePrefab, transform.position, transform.rotation);
		shotFired.GetComponent<ProjectileScript> ().Initialize (sights.transform.position - transform.position, "Player");

		//déclenchement de l'animation de tir
		anim.SetTrigger ("fire");

		yield return new WaitForSeconds (fireRate);
		shooting = false;
	}
}
