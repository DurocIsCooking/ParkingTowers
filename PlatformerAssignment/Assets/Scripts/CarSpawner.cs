using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    // Spawn cars periodically if player is within the activation range
    [SerializeField] private Vector2 _activationRange;
    [SerializeField] private float _spawnFrequency;
    [SerializeField] private float _spawnTimer = 0;

    [SerializeField] private GameObject _car;
    [SerializeField] private bool _carFacingRight;
    [SerializeField] private float _carAcceleration;
    [SerializeField] private float _maxCarVelocity;

    private void Update()
    {
        if(Physics2D.OverlapBox(transform.position, _activationRange, 0, LayerMask.GetMask("Player")) != null)
        {
            _spawnTimer += Time.deltaTime;
        }
        if(_spawnTimer >= _spawnFrequency)
        {
            SpawnCar();
        }
    }

    private void SpawnCar()
    {
        // Face car in direction of player
        if(Physics2D.OverlapBox(transform.position, _activationRange, 0, LayerMask.GetMask("Player")) != null)
        {
            GameObject player = Physics2D.OverlapBox(transform.position, _activationRange, 0, LayerMask.GetMask("Player")).gameObject;
            if (player.transform.position.x < transform.position.x)
            {
                _carFacingRight = false;
            }
            else
            {
                _carFacingRight = true;
            }
        }
        // Instantiate car and set values
        GameObject car = Instantiate(_car, transform.position, Quaternion.identity);
        car.GetComponent<CarEnemy>().IsFacingRight = _carFacingRight;
        _spawnTimer = 0;
        car.GetComponent<CarEnemy>().SetSpeed(_carAcceleration, _maxCarVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_activationRange.x, _activationRange.y, 1));
    }
}
