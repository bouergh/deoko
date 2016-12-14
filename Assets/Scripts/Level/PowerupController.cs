using UnityEngine;
using System.Collections;

public class PowerupController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ChangePowerup(1);
            Destroy(gameObject);
        }
    }
}
