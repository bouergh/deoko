using UnityEngine;
using System.Collections;

/* Celui-ci lance des bombes IEM
 * qui ralentissent le joueur et l'empeche d'attaquer
 * pendant un certain temps. les bombes partent de lui,
 * vont assez vite mais s'arrêtent une fois sur la position du 
 * joueur et mettent un certain temps à exploser
 * */

public class Tazman : EnemyController {

    override protected void Start()
    {
        base.Start();
        moveType = MoveType.closeToPlayer;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
