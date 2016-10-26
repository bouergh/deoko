using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    //variables communes
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject shot;


    //variables changeant en fonction du type d'ennemi
    [SerializeField]
    private MoveType moveType;  //how the enemy moves
    [SerializeField]
    private float speed;        //the speed at which he moves
    [SerializeField]
    private bool closeEnoughToPlayer;  //the minimum distance he can approach the player if moving (depends on both colliders ?!)
    [SerializeField]
    private float fireRate;  //the minimum distance he can approach the player if moving (depends on both colliders ?!)
    private float fireTimer;
    [SerializeField]
    private float projectSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int life;


    private enum MoveType
    {
        still,
        horizontal,
        vertical,
        closeToPlayer,
        awayFromPlayer
    }
    
	void Awake () {
        player = GameObject.Find("Player");
        moveType = MoveType.closeToPlayer;
        fireTimer = fireRate;
	}
	
	void FixedUpdate () {
        Move();
        Shoot();
        fireTimer -= Time.fixedDeltaTime;
	}

    void Move()
    {
        switch (moveType)
        {
            case MoveType.closeToPlayer:
                if (!closeEnoughToPlayer)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
                }
                break;

            case MoveType.still:
                break;

            default:
                print("Wrong/Unimplemented movement type for " + this.name);
                break;
        }
    }

    // enemy shoots toward player at his maximum rate
    // need to implement raycast to make it realistic
    void Shoot(){
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (fireTimer <= 0)
        {
            GameObject shotFired = (GameObject)Instantiate(shot, transform.position, transform.rotation);
            shotFired.GetComponent<ProjectileScript>().Initialize(direction, projectSpeed, "Enemy", damage);
            fireTimer = fireRate;
        }
    }

    public void TouchPlayer(bool value)
    {
        closeEnoughToPlayer = value;    //sets variable to know if the player is close enough to take damage
    }

    public void TakeDamage(int damageTaken)
    {
        if (!IsDead())
        {
            life = Mathf.Max(life - damageTaken, 0);

            if (IsDead())
            { //si ce dégât vient de nous tuer
                Destroy(this.gameObject);
            }
        }
    }

    private bool IsDead()
    {
        return life <= 0;
    }

}

/* Remarques/idées :
 * L'ennemi ne touche pas le jouer grâce à la hitbox extérieure de ce dernier.
 * Par contre il peut collisionner avec les autres ennemis, ainsi qu'être bloqué par les murs.
 * Si on veut éviter cela de la même manière les ennemis risquent de ne plus bouger s'ils se rencontrent, ce qui est problématique si ça arrive avant le joueur.
 * Eventuellement faire un algo de pathfinding ? A* ! Cf Théophile.
 * */
