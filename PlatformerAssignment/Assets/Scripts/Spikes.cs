using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    [SerializeField] private bool _isInCeiling;
    private bool _isFalling;

    private void Update()
    {
        if(_isInCeiling)
        {
            FireBoxCast();
        }
        if(_isFalling)
        {
            Fall();
        }

    }

    private void FireBoxCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector2.down, 30f);
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _isInCeiling = false;
                _isFalling = true;
            }
        }
        
    }

    private void Fall()
    {
        transform.position += Vector3.down * 0.05f;
    }

}
