using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spikes : MonoBehaviour
{

    [SerializeField] private bool _isInCeiling;
    private bool _isFalling;
    private float _fallDurationMultiplier = 0.075f; // Smaller = faster

    [SerializeField] private GameObject _target; // Used to determine raycast range and stopping point of fall
    private float _raycastRange;

    private void Awake()
    {
        if(_isInCeiling)
        {
            _raycastRange = Mathf.Abs(_target.transform.localPosition.y * transform.localScale.y);
            foreach(SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = new Color(1, 0.6839622f, 0.6839622f);
            }
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
        // I don't understand why I had to factor in the scale of the parent object (transform.localscale.y) both here and in the awake function
        // It seems to me this should only be required once
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.5f, _raycastRange * transform.localScale.y), 0, Vector2.down, 0f);
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
        TweenParams fallParams = new TweenParams().SetEase(Ease.OutBounce);
        float duration = (transform.position.y - _target.transform.position.y) * _fallDurationMultiplier;
        transform.DOMoveY(_target.transform.position.y, duration).SetAs(fallParams);
        _isFalling = false;
    }

}
