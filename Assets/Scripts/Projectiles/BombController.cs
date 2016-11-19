using UnityEngine;
using System.Collections;

public class BombController : ProjectileController {

    [SerializeField]
    private float exploTime;
    private bool tictac;
    private float time;
	private GameObject bombRadius;  //à enlever, remplacer par vraie animation
    private bool exploding;
    [SerializeField] private float exploRadius; //à pas confondre avec le truc à enlever, c'est le rayon d'explosion, utile pour Physics2D.OverlapCircle
    [SerializeField] private LayerMask playerLayer;

    // Use this for initialization
    void Start()
    {
        time = 0f;
        tictac = false;
        bombRadius = this.gameObject.transform.GetChild(0).gameObject;  //à enlever, remplacer par vraie animation
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
        if (other.tag == "Player" && origin.tag != "Player" && !tictac)
        {
            tictac = true;
            time = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (other.tag == "Wall" && !tictac)
        {
            tictac = true;
            time = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private IEnumerator Explode()
    {
        //detection du joueur dans la zone d'explosion et distribution des dégâts
        Collider2D hit = Physics2D.OverlapCircle(transform.position, exploRadius, playerLayer);
        if (hit != null && hit.tag == "Player") hit.gameObject.GetComponent<PlayerController>().TakeDamage(damage);

        //"animation"
        bombRadius.GetComponent<SpriteRenderer>().enabled = true;   //à enlever, tout comme "bombRadius" et le reste
        yield return new WaitForSeconds(0.5f);  //ça aussi sans doute, faudra juste gérer l'animation et détruire à la fin
        
        // destruction de l'objet
        Destroy(gameObject);
    }

    /* remarque :
     * il faudrait enlever la partie avec l'objet enfant,
     * puisque on fera la détection du joueur lors de l'explosion avec Physics2D.OverlapCircle,
     * et l'explosion (et donc visuel de la portée de la bombe) avec une animation du GameObject ORIGINAL
     *  */
}
