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
        BaseBehaviour();
        //will do this instead of shooting since he has no projectile
        if (triggerCounter > 2)  //checks toxic damage zone
            {
                player.GetComponent<PlayerController>().TakeDamage(meleeDamage);
            }
        
    }
}