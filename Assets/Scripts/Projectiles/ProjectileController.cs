using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
    protected Vector2 direction;
	[SerializeField] protected float speed;
    protected string origin;
	[SerializeField] protected int damage;


    //when a projectile is instantiated, this script is associated and we call this function to initialize projectile parameters
    public void Initialize(Vector2 direction, string origin)
    {
        this.direction = direction;
        this.origin = origin;
    }
}
