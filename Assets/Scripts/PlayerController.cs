using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject shot;
    [SerializeField]
    private float fireRate;
    private float fireTimer;
    [SerializeField]
    private float projectSpeed;



    // Use this for initialization
    void Start () {
        fireTimer = fireRate;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        fireTimer -= Time.fixedDeltaTime;

        // Movement routine
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        // Shooting routine
        float shotHorizontal = Input.GetAxis("HorizontalShot");
        float shotVertical = Input.GetAxis("VerticalShot");
        Vector2 direction = new Vector2(shotHorizontal, shotVertical);
        direction.Normalize();
        if(direction != Vector2.zero && fireTimer <= 0)
        {
            GameObject shotFired = (GameObject)Instantiate(shot, transform.position, transform.rotation);
            shotFired.GetComponent<Mover>().direction = direction;
            shotFired.GetComponent<Mover>().speed = projectSpeed;
            shotFired.GetComponent<Mover>().origin = "Player";
            fireTimer = fireRate;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")        //catches if enemy is close enough to stop and make damage
        {
            other.GetComponentInParent<EnemyScript>().TouchPlayer(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyScript>().TouchPlayer(false);
        }
    }
}

/*Remarque et idées futures :
 * Il faudrait bouger le joueur grâce à la physique du jeu sinon ça fout la merde ? oupa ? cf Isaac
 * */