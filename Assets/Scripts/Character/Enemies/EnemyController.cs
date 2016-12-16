using UnityEngine;
using System.Collections;

public class EnemyController : Character {

    //variables communes
    protected GameObject player;
    [SerializeField] private GameObject projectile;

    //variables changeant en fonction du type d'ennemi
    protected MoveType moveType;  //how the enemy moves
	[SerializeField] private float fireRate;
	protected bool shooting = false;
    protected int triggerCounter;   //counter to know in which collider player has actually entered
    [SerializeField] protected int meleeDamage;      //damage done in melee or via poison

    [SerializeField] LayerMask hitMask;

    protected bool follow;

    protected enum MoveType
    {
        still,
        horizontal,
        vertical,
        closeToPlayer,
        toPlayerPathfinding,
        awayFromPlayer
    }
    
	override protected void Start () {
        base.Start();
        triggerCounter = 0;
		player = GameObject.FindGameObjectWithTag("Player");
	}

    protected void MoveEnemy()
    {
        switch (moveType)
        {
            case MoveType.closeToPlayer:
                Move((player.transform.position - transform.position).normalized);
                break;

            case MoveType.toPlayerPathfinding:
                Debug.Log("I wish I had implemented an A* algorithm :'(");
                break;

            case MoveType.still:
                break;

            default:
                print("Wrong/Unimplemented movement type for " + this.name);
                break;
        }
    }
    protected void StopMoving()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // enemy shoots toward player at his maximum rate
    protected IEnumerator Shoot(Vector2 direction){
		shooting = true;
		GameObject shotFired = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        shotFired.GetComponent<ProjectileController>().Initialize(direction, gameObject);
        yield return new WaitForSeconds (fireRate);
		shooting = false;
    }

    // raycasts to check for visual on player or not, gives his direction
    protected Vector2 Aim()
    {
        Vector2 shootLine = player.transform.position - transform.position;
        Vector2 direction = shootLine.normalized;
        float distance = shootLine.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, hitMask);
        if(hit.collider)  //signifie qu'il y a un mur entre le joueur et l'ennemi qui veut tirer
        {
            direction = Vector2.zero;
        }// pas encore parfait comme les murs sont carrés et que le joueur a une box cheloue
        return direction;
    }
    
    //modify trigger counter so that we know in which collider player is at this moment
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !other.isTrigger)
        {
            triggerCounter++;
            Debug.Log(triggerCounter);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            triggerCounter--;
            Debug.Log(triggerCounter);
        }
    }

    /* gestion des zones d'aggro, du mouvement et des tirs :
     * bien configurer MoveType et les trigger collider pour que cette fonction 
     * s'applique à tous les types d'ennemis
     *  */
    protected void BaseBehaviour()  
    {
        if (Aggro())            //only gains aggro when inside the specific aggro zone (second from the exterior) and seeing the player (raycast)
        {
            follow = true;
        }

        if (triggerCounter < 1) //only loses aggro when out of the external zone
        {
            follow = false;
        }

        if(triggerCounter < 4 && follow)    //if has aggro and not too close, will move toward player
        {
            MoveEnemy();    //should replace this with a more intelligent pathfinding or chose for each enemy !
        }
        else
        {
            StopMoving();   //else the enemy will continue in the same direction after losing aggro !
        }

        if (triggerCounter > 2 && !shooting)   //checks shooting zone
        {
            Vector2 direction = Aim();
            if (direction != Vector2.zero)
            {
                StartCoroutine(Shoot(direction));
            }
        }
    }
    // aggro marche avec BaseBehaviour, bien configurer les zones !
    protected bool Aggro()  //gestion de l'aggro par raycast et zone d'aggro
    {
        return ((triggerCounter > 1) && Aim() != Vector2.zero);
    }

}
