using UnityEngine;
using System.Collections;

public class PlayerController : Character {

	[SerializeField] private GameObject sightsPrefab;
	[SerializeField] private GameObject projectilePrefab;
	private GameObject sights;
	private bool shooting = false;
	[SerializeField] private float fireRate;

    private int keyNum = 0;
    private int powNum = 0;
    [SerializeField] private int maxPowNum = 10;    //max number of power-ups you can stack


	protected override void Start() {
		base.Start ();
		sights = (GameObject)Instantiate (sightsPrefab, transform.position, Quaternion.identity);
		sights.transform.parent = transform;
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

    public bool ChangeKey(int num)
    {
        bool keysLeft = (keyNum > 0);   //to know if we can open a door
        keyNum = Mathf.Max(0, keyNum + num); //adds num to keyNum, can't go to less than 0
        return keysLeft;                // if true, we'll have reduced number of keys and opened a door
    }

    public void ChangePowerup(int num)
    {
        powNum += num;
        Mathf.Clamp(powNum, 0, maxPowNum);
    }
}