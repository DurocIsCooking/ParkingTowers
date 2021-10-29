using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFlappingWings : MonoBehaviour
{
    [SerializeField] private Animator _wingsAnimator;
    public void StopFlapping()
    {
        _wingsAnimator.SetBool("useWings", false);
    }
}
