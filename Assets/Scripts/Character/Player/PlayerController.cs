using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : Character {

	//viseur et projectiles
	[SerializeField] private GameObject sightsPrefab;
	[SerializeField] private GameObject projectilePrefab;

	//Elements d'UI
	private RectTransform lifeBarUIMaskRect;
	private Vector2 lifeBarWidth;
	private int totalLife;
	private Text PowerUpsUICount;
	private Text KeysUICount;

	private GameObject sights;
	private bool shooting = false;
	[SerializeField] private float fireRate;

	//systeme de clés
    private int keyNum = 0;

    //systeme de power-ups
    private int powNum = 0;
    [SerializeField] private static int maxPowNum = 100;    //max number of power-ups you can stack
    [SerializeField] private int healFactor = 10;    //life you heal when picking up a power-up (yellow shuriken)


	protected override void Start() {
		base.Start ();

		//initialisation des objets du UI
		try {
			InitUIObjects ();
		}catch(NullReferenceException e){}

		//création de l'objet de visée
		sights = (GameObject)Instantiate (sightsPrefab, transform.position, Quaternion.identity);
		sights.transform.parent = transform;
	}

	private void InitUIObjects() {
		//barre de vie
		lifeBarUIMaskRect = GameObject.Find ("UI canvas/LifeBar/mask").GetComponent<RectTransform> ();
		lifeBarWidth = lifeBarUIMaskRect.sizeDelta;
		totalLife = getLife ();

		//powerups count
		PowerUpsUICount = GameObject.Find ("UI canvas/Powerups/Text").GetComponent<Text>();

		//powerups count
		KeysUICount = GameObject.Find ("UI canvas/Keys/Text").GetComponent<Text>();
	}

	public override void TakeDamage(int damageTaken) {
		base.TakeDamage (damageTaken);
        UpdateUI();
		if (IsDead ()) {
			GameObject.Find ("BoardManager").GetComponent<GameManager> ().ShowDeathScreen ();
		}
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
		if (!base.IsDead ()) {
			// Sight indicator update
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); // get the real mouse position
			sights.transform.position = new Vector2 (transform.position.x, transform.position.y) + (mousePosition - new Vector2 (transform.position.x, transform.position.y)).normalized * 2;
			sights.transform.localScale = transform.localScale;

			if ((FacingRight && sights.transform.position.x < transform.position.x) || (!FacingRight && sights.transform.position.x > transform.position.x)) {
				base.Flip ();
			}
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

    private void UpdateUI()
    {
        //updates the life bar
        lifeBarUIMaskRect.sizeDelta = new Vector2((getLife() * lifeBarWidth.x) / totalLife, lifeBarWidth.y);
		//updates the powerups count
		PowerUpsUICount.text = "x " + powNum.ToString();
		//updates the keys count
		KeysUICount.text = "x " + keyNum.ToString();
    }

    public bool ChangeKey(int num)
    {
        bool keysLeft = (keyNum > 0);   //to know if we can open a door
        keyNum = Mathf.Max(0, keyNum + num); //adds num to keyNum, can't go to less than 0
        return keysLeft;                // if true, we'll have reduced number of keys and opened a door
    }

    public void ChangePowerup(int num)
    {
        powNum += num;                      //
        Mathf.Clamp(powNum, 0, maxPowNum);  //nombre de power-ups compris entre le max défini et 0
        ApplyPowerup(powNum);               //applique la modification des stats/pouvoirs du joueur selon le nombre de power-ups collectés
    }

    private void ApplyPowerup(int num)
    {
        //prendre un power-up soigne
        life += healFactor;
        UpdateUI();

        switch (num)    //ici il faut changer quelques caractéristiques du perso quand son nombre de power-ups atteint des nombres particuliers
        {
            case 0:         //no power-ups
                break;

            case 10:
                //donne une nouvelle attaque de corps-à-corps p.ex
                break;

            case 20:
                //change l'attaque au shuriken, genre en lance 3 ou quoi
                break;

            case 30:    //ou plutôt maxPowNum*3/10 -> non !
                //confère une "attaque ultime" avec plus gros cooldown, permettant d'envoyer des shurikens partout p.ex
                break;

            //etc etc

            case 100: //ou plutôt maxPowNum ?? embêtant car un switch-case ne peut prendre que des constantes !
                //toute puissance du perso car max de powerUp
                break;

            default:
                break;
        }
    }


}