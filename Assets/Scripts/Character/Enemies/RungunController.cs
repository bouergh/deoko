using UnityEngine;
using System.Collections;

public class RungunController : EnemyController {

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.closeToPlayer;
    }

    void FixedUpdate()
    {
        MoveEnemy();
        if (!shooting)
            StartCoroutine(Shoot(Aim()));
    }
}
