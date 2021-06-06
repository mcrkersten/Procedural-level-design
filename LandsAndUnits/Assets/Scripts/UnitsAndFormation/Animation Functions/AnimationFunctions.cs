using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void TriggerAnimation(string anim)
    {
        if(_animator != null)
        {
            _animator.SetTrigger(anim);
        }
    }
    
    public void DestroyAnimator()
    {
        Destroy(_animator);
    }
}
