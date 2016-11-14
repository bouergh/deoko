using UnityEngine;
using System.Collections;

/* This one shoots at player and doesn't move.
 * */

public class AimTurretController : EnemyController
{

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.still;
    }

    void FixedUpdate()
    {
        if (triggerCounter > 0 && !shooting)   //checks shooting zone
        {
            Vector2 direction = Aim();
            if (direction != Vector2.zero)
            {
                StartCoroutine(Shoot(direction));
            }
        }
    }
}
