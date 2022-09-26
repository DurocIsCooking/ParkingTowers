using UnityEngine;

// Simple script called by the animator to stop the flapping animation.
public class StopFlappingWings : MonoBehaviour
{
    [SerializeField] private Animator _wingsAnimator;
    public void StopFlapping()
    {
        _wingsAnimator.SetBool("useWings", false);
    }
}
