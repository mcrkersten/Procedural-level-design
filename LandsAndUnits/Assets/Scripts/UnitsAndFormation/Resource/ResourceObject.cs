using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using DG.Tweening;
public class ResourceObject : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [HideInInspector] public Unit harvester; //if null, anyone can pickup
    [HideInInspector] public ResourceInformation _resourceInformation;
    [HideInInspector] public int _amount;

    [HideInInspector] public ResourceStack _stack;
    private int _lifetime = 150;
    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _lifetime--;
        if(_lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnFromResource()
    {
        StartCoroutine(PerformAction(1f));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Unit"))
        {
            if(collision.transform == harvester.transform)
            {

                Unit unit = collision.transform.GetComponent<Unit>();

                unit._unitBrain.VerifyStorageOfUnit(_resourceInformation._resourceType);
                int overdraft = unit._storagCompartment.DepositResource(_amount, _resourceInformation._resourceType);
                if (overdraft != 0)
                {
                    unit._storagCompartment.OnFullStorage(_resourceInformation);

                    if(_stack != null)
                        _stack._amountOfResources += overdraft;
                }

                if (_stack != null)
                    _stack._resourceObjectsCount -= 1;
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator PerformAction(float timer)
    {
        float elapsedTime = 0;
        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SpawnFromResourceAction();
    }

    private void SpawnFromResourceAction()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.DOMove(harvester.transform.position, 1f).SetEase(Ease.InOutBack);
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f),1f).SetEase(Ease.InOutBack);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
