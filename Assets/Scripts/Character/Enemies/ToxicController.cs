using UnityEngine;
using System.Collections;

public class ToxicController : EnemyController {

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.closeToPlayer;
    }


    void FixedUpdate()
    {
        if (triggerCounter > 0 && triggerCounter < 3)   //checks aggro zone, second member replace "Touch" with player
        {
            MoveEnemy();
        }
        else
        {
            StopMoving();   //else the enemy will continue in the same direction after losing aggro !
        }
        if (triggerCounter > 1)  //checks toxic damage zone
            {
                player.GetComponent<PlayerController>().TakeDamage(meleeDamage);
            }
        
    }
}
/* Remarque :
 * penser à faire une rétroaction visuelle pour la zone "toxique"
 * Problème :
 * Actuellement le perso ne semble commencer à suivre que quand le joueur entre dans la zone "2",
 * continue de le suivre "normalement" tant qu'il reste dans la zone 1,
 * puis continue pendant un moment dans la même direction si le joueur en sort... WTF ?!
 * */