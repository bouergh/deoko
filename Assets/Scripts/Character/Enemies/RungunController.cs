using UnityEngine;
using System.Collections;

/* This one chases player and shoots at
 * him if close enough, without stopping
 * will stop chasing player if too far.
 * */

public class RungunController : EnemyController {
    

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.closeToPlayer;
    }

    void FixedUpdate() 
    {
        BaseBehaviour();
    }
}
