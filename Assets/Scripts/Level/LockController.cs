using UnityEngine;
using System.Collections;

public class LockController : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>().ChangeKey(-1))
            {
                Destroy(gameObject);
            }
        }
    }
}
