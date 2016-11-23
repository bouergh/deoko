using UnityEngine;
using System.Collections;

public class LaserController : ProjectileController {

    //no speed, the laser is instant ! (for the moment)
    LineRenderer line;
    private Vector2 position; //till we use parent !
    [SerializeField]
    LayerMask hitMask;  //les murs à détecter pour savoir où s'arrête le laser

    //things for the animation of the lazer (for the moment)
    [SerializeField]
    private float maxWidth = 0.30f;
    [SerializeField]
    private float minWidth = 0.10f;
    [SerializeField]
    private Color aimColor = new Color(0, 0, 0, 0.1f);
    [SerializeField]
    private Color shootColor = new Color(1, 1, 1, 1);
    [SerializeField]
    private float animTime = 1.5f;


    private float timer;

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

        StartCoroutine(Count());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnDestroy()
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        if(hit && hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    void Shoot()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(position, direction);
        int i = 1;
        bool wall = false;
        while (!wall && i <= hit.Length)
        {
            if (hit[i].collider.transform.tag == "Wall")
            {
                wall = true;
            }
            else if (hit[i].collider.transform.tag == "Player")
            {
                hit[i].collider.transform.GetComponent<PlayerController>().TakeDamage(damage);
            }
            i++;
        }
        line.SetColors(shootColor, shootColor);
    }

    IEnumerator Count()
    {
        float timeStep = 0.1f;
        line.SetColors(aimColor, aimColor);
        while (timer < animTime)
        {
            float width = Mathf.Lerp(minWidth, maxWidth, timer / animTime);
            line.SetWidth(width, width);
            timer += timeStep;
            yield return new WaitForSeconds(timeStep);
        }
        Shoot();
        yield return new WaitForSeconds(2 * timeStep);
        Destroy(gameObject);
    }
}
