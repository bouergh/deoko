using UnityEngine;
using System.Collections;

public class LaserController : ProjectileController {

    //no speed, the laser is instant ! (for the moment)
    LineRenderer line;
    private Vector2 position; //till we use parent !
    [SerializeField]
    LayerMask hitMask;  //les murs à détecter pour savoir où s'arrête le laser


    // Use this for initialization
    void Start()
    {
        position = origin.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, hitMask);
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.SetPosition(0, position);
        line.SetPosition(1, hit.point);
        line.sortingLayerName = "Projectiles";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //just for testing purposes, will keep only at laser destruction in the end !
        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        if (hit.transform.tag == "Player")
        {
            hit.transform.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    void OnDestroy()
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        if(hit && hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
