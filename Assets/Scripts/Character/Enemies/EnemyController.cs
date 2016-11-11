using UnityEngine;
using System.Collections;

public class EnemyController : Character {

    //variables communes
    private GameObject player;
    [SerializeField] private GameObject projectile;

    //variables changeant en fonction du type d'ennemi
    protected MoveType moveType;  //how the enemy moves
    //[SerializeField] private float speed;        //the speed at which he moves
    private bool closeEnoughToPlayer;  			 //the minimum distance he can approach the player if moving (depends on both colliders ?!)
    //[SerializeField] private int life;
	[SerializeField] private float fireRate;
	protected bool shooting = false;


    protected enum MoveType
    {
        still,
        horizontal,
        vertical,
        closeToPlayer,
        awayFromPlayer
    }
    
	override protected void Start () {
        base.Start();
        player = GameObject.Find("Player");
	}

    protected void MoveEnemy()
    {
        switch (moveType)
        {
            case MoveType.closeToPlayer:
                if (!closeEnoughToPlayer)
                {
                    //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
                    Move((player.transform.position - transform.position).normalized);
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
    protected IEnumerator Shoot(Vector2 direction){
		shooting = true;
		GameObject shotFired = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        shotFired.GetComponent<ProjectileController>().Initialize(direction, "Enemy");

		yield return new WaitForSeconds (fireRate);
		shooting = false;
    }

    protected Vector2 Aim()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        return direction;
    }

    public void TouchPlayer(bool value)
    {
        closeEnoughToPlayer = value;    //sets variable to know if the player is close enough to take damage
    }

}
