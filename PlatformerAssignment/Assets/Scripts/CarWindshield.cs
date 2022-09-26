using UnityEngine;

// This script is placed on the windshield object, a child of the car enemy. It simply makes the car explode when the windshield collides with anything.
public class CarWindshield : MonoBehaviour
{
    CarEnemy car;

    private void Awake()
    {
        car = transform.GetComponentInParent<CarEnemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        car.Die();
    }

}
