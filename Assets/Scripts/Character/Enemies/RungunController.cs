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


        
        /* Ce qu'on peut changer :
         * ajouter un cercle concentrique (ou un carré) intérieur pour arrêter de suivre si déjà collé,
         * ou s'arrêter à une distance de sécurité pour ceux qui tapent de loin
         * et ajouter un cercle concentrique extérieur pour la détection du joueur :
         * la zone d'aggro serait plus petite que la zone de suivi, c'est à dire que le
         * joueur doit s'éloigner encore plus pour semer l'ennemi qu'il n'avait dû s'approcher
         * pour que ce dernier commence à le suivre
         * */
        
    }
}
