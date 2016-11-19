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
        if (triggerCounter > 0 && triggerCounter < 3 && !shooting)   //checks aggro zone, second member replace "Touch" with player
        {
            MoveEnemy();
        }
        else
        {
            StopMoving();   //else the enemy will continue in the same direction after losing aggro !
        }
        if (triggerCounter > 1 && !shooting)   //checks shooting zone
        {
            StopMoving();
            Vector2 direction = Aim();
            if(direction != Vector2.zero)
            {
                StartCoroutine(Laser(direction));
            }
        }
    }
}
