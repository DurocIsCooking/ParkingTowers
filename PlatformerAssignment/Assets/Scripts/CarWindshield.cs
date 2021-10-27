using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
