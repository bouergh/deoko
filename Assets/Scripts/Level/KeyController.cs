using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ChangeKey(1);
            Destroy(gameObject);
        }
    }
}
