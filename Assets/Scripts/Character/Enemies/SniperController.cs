using UnityEngine;
using System.Collections;

/* This one chases player and shoots at
 * him if close enough, stopping each time he shoots.
 * Will stop chasing player if too far.
 * Should fire instant projectile, and drawing a lazor beforehand.
 * */

public class SniperController : EnemyController {

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.closeToPlayer;
    }

    void FixedUpdate() 
    {
        //sa spécificité est qu'il arrête de bouger quand il tire !
        BaseBehaviour();
    }

    protected override void BaseBehaviour()
    {
        if (Aggro())            //only gains aggro when inside the specific aggro zone (second from the exterior) and seeing the player (raycast)
        {
            follow = true;
        }

        if (triggerCounter < 1) //only loses aggro when out of the external zone
        {
            follow = false;
        }

        //won't move if shooting... spécificité du perso !
        if (triggerCounter < 4 && follow && !shooting)    //if has aggro and not too close, will move toward player
        {
            MoveEnemy();    //should replace this with a more intelligent pathfinding or chose for each enemy !
        }
        else
        {
            StopMoving();   //else the enemy will continue in the same direction after losing aggro !
        }

        if (triggerCounter > 2 && !shooting)   //checks shooting zone
        {
            StopMoving(); //(spécificité du sniper !)
            Vector2 direction = Aim();
            if (direction != Vector2.zero)
            {
                StartCoroutine(Shoot(direction));
            }
        }
    }
}
