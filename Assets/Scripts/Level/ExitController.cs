using UnityEngine;
using System.Collections;


public class ExitController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
			GameObject.Find ("BoardManager").GetComponent<GameManager> ().LoadNextLevel ();
        }
    }
}
