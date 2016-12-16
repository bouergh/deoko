using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : Character {

	//viseur et projectiles
	[SerializeField] private GameObject sightsPrefab;
	private GameObject projectilePrefab;

	//Elements d'UI
	private RectTransform lifeBarUIMaskRect;
	private Vector2 lifeBarWidth;
	private int totalLife;
	private Text PowerUpsUICount;
	private Text KeysUICount;

    //shooting
	private GameObject sights;
	private bool shooting = false;
	[SerializeField] private float fireRate;

    //slashing
    private bool slashing = false;
    [SerializeField]  private float slashRate;

	//systeme de clés
    private int keyNum = 0;

    //systeme de power-ups
    private int powNum = 0;
    [SerializeField] private static int maxPowNum = 100;    //max number of power-ups you can stack
    [SerializeField] private int healFactor = 10;    //life you heal when picking up a power-up (yellow shuriken on ground)
    [SerializeField] private GameObject shuriken1;
    [SerializeField] private GameObject shuriken2;
    [SerializeField] private GameObject shuriken3;
    private bool canShurikenStorm = false;
    private bool canSuperNinja = false;

    //shuriken storm
    private bool shurikenStorming = false;
    [SerializeField] private float shurikenStormCooldown;

    //super ninja
    private bool superNinjing = false;
    [SerializeField] private float superNinjaCooldown;

    protected override void Start() {
		base.Start ();

        projectilePrefab = shuriken1;

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
			if (Input.GetButton ("Shoot") && !shooting)
				StartCoroutine (ShootBullet ());

            // Katana routine
            if(Input.GetButton("Slash") && !slashing)
            {
                StartCoroutine(Katana());
            }
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

        //milestones are 0, 5, 10, 15, 20 for the moment
        if (num < 5)    //no power-ups
        {
            canShurikenStorm = false;
            canSuperNinja = false;
            projectilePrefab = shuriken1;
        }
        else if (num < 10)  
        {
            canShurikenStorm = true;    //confère une "attaque ultime" avec plus gros cooldown, permettant d'envoyer des shurikens partout
            canSuperNinja = false;
            projectilePrefab = shuriken1;
        }
        else if (num < 15)  
        {
            canShurikenStorm = true;
            canSuperNinja = false;
            projectilePrefab = shuriken2;   //changement de shuriken -> rouge
        }
        else if (num < 20)  
        {
            canShurikenStorm = true;
            canSuperNinja = true;   //confère un buff de vitesse et fireRate temporaire à activer comme deuxième attaque ultime
            projectilePrefab = shuriken2;
        }
        else  
        {
            canShurikenStorm = true;
            canSuperNinja = true;
            projectilePrefab = shuriken3;   //changement de shuriken -> or
        }
    }

    IEnumerator Katana()    //attaque de CaC
    {
        slashing = true;
        //do something !
        Debug.Log("coup de KATANA !");
        yield return new WaitForSeconds(slashRate);
        slashing = false;
    }

    IEnumerator ShurikenStorm()    //tourbillon de shuriken
    {
        shurikenStorming = true;
        //do something !
        Debug.Log("ShurikenSTOOOOOOORM !");
        yield return new WaitForSeconds(shurikenStormCooldown); //fonctionne actuellement avec cooldown, il faudrait peut être changer pour éviter les abus
        shurikenStorming = false;
    }

    IEnumerator SuperNinja()   //buff de déplacement et cadence de tir temporaire
    {
        superNinjing = true;
        //do something !
        Debug.Log("SupeeeeeeeerNINJA !");
        yield return new WaitForSeconds(superNinjaCooldown);    //fonctionne actuellement avec cooldown, il faudrait peut être changer pour éviter les abus
        superNinjing = false;
    }

//si on garde le déblocage d'attaques spéciales et le cooldown pour les lancer, il faudrait donner un intérêt au temps
//genre high score = temps
//ou alors les ennemis peuvent survivre à une attaque spéciale, et régénèrent leur vie si on sort de leur zone d'aggro
//il faudrait qu'on ait la pression des ennemis pour que le cooldown soit une bonne mécanique (mais on garde pour le futur car c'est une bonne mécanique)
}