using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

    //these variables are set by instance which instantiates the projectile
    private Vector2 direction;
    private string origin;
    [SerializeField]
    private int damage;
    //no speed, the laser is instant ! (for the moment)
    LineRenderer line;
    private Vector2 position; //till we use parent !
    [SerializeField]
    LayerMask hitMask;  //les murs à détecter pour savoir où s'arrête le laser


    // Use this for initialization
    void Start()
    {
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
    

    //when a projectile is instantiated, this script is associated and we call this function to initialize projectile parameters
    public void Initialize(Vector2 direction, string origin, Vector2 position)
    {
        this.direction = direction;
        this.origin = origin;
        this.position = position;
    }

    void OnDestroy()
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        if(hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
