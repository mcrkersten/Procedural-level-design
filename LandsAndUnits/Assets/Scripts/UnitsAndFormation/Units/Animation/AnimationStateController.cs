using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private int _animationHash;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _animationHash = Animator.StringToHash("Velocity");
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Walk animaiton
        _animator.SetFloat(_animationHash, _rigidbody.velocity.magnitude / GetComponent<Unit>()._movementSpeed);
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public void TriggerRangedAttack()
    {
        _animator.SetTrigger("RangedAttack");
    }

    public void TriggerResourceThrow()
    {
        _animator.SetTrigger("Collect");
    }

    public void TriggerDamage()
    {
        _animator.SetTrigger("Damage");
    }

    public void TriggerResponse(Response response)
    {
        _animator.SetInteger("ResponseIndex", (int)response);
        _animator.SetTrigger("ResponseTrigger");
    }
}

public enum Response
{
    Frustration = 0,
    Angry,
    Happy,
    Sad,
    Eat
}
