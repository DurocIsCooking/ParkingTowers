using UnityEngine;
using DG.Tweening;

public class Spikes : MonoBehaviour
{

    [SerializeField] private bool _isInCeiling; // If true, these spikes are shaded pink on awake, and will fall when the player passes beneath.
    private bool _isFalling;
    private float _fallDurationMultiplier = 0.075f; // Smaller = faster

    [SerializeField] private GameObject _target; // A child object used to determine stopping point of fall, and range of raycast used to check for player.
    private float _raycastRange;

    // If this is a falling set of spikes, shade pink and set parameters
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

    // Raycast beneath for player. If the player is found, start falling.
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

    // Fire a boxcast beneath the spikes. If the player is found, return true.
    private bool CheckForPlayer()
    {
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

    // Using DoTween to control speed of fall and bounce at the end.
    private void Fall()
    {
        TweenParams fallParams = new TweenParams().SetEase(Ease.OutBounce);
        float duration = (transform.position.y - _target.transform.position.y) * _fallDurationMultiplier;
        transform.DOMoveY(_target.transform.position.y, duration).SetAs(fallParams);
        _isFalling = false;
    }

}
