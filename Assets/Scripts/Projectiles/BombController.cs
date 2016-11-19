using UnityEngine;
using System.Collections;

public class BombController : ProjectileController {

    [SerializeField]
    private float exploTime;
    private bool tictac;
    private float time;
	private GameObject bombRadius;
    private bool exploding;

    // Use this for initialization
    void Start()
    {
        time = 0f;
        tictac = false;
        bombRadius = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if(!tictac)
        {
            GetComponent<Rigidbody2D>().velocity = direction * speed;   //projectile goes towards its initial direction, at same speed, for the moment
        }
        else
        {
            if (time > exploTime && !exploding)
            {
                exploding = true;
                StartCoroutine(Explode());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && origin != "Player")
        {
            tictac = true;
            time = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (other.tag == "Wall")
        {
            tictac = true;
            time = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    //when a projectile is instantiated, this script is associated and we call this function to initialize projectile parameters
    public void Initialize(Vector2 direction, string origin)
    {
        this.direction = direction;
        this.origin = origin;
    }

    private IEnumerator Explode()
    {
        bombRadius.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
