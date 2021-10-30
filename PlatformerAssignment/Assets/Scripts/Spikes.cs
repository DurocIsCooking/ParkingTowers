using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    [SerializeField] private bool _isInCeiling;
    private bool _isFalling;

    [SerializeField] private GameObject _target; // Used to determine raycast range and stopping point of fall
    private float raycastRange;

    private void Awake()
    {
        if(_isInCeiling)
        {
            raycastRange = Mathf.Abs(_target.transform.localPosition.y * transform.localScale.y);
        }
        
    }

    private void Update()
    {
        if(_isInCeiling && CheckForPlayer())
        {
            _isInCeiling = false;
            _isFalling = true;
        }
        if(_isFalling)
        {
            Fall();
        }

    }

    private bool CheckForPlayer()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.5f, raycastRange * transform.localScale.y), 0, Vector2.down, 0f);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") && hit.transform.position.y < transform.position.y)
            {
                // Deparent target
                _target.transform.parent = null;
                return true;
            }
        }
        return false;
    }

    private void Fall()
    {
        transform.position += Vector3.down * 0.05f;
        // Stop at target
        if(transform.position.y <= _target.transform.position.y)
        {
            _isFalling = false;
        }
    }

}
