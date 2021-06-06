using System.Collections;
using System.Collections.Generic;
using UnitsAndFormation;
using UnityEngine;
using DG.Tweening;

public class ResourceStack : UnitInteractable
{
    public ResourceInformation _resourceInformation;
    public float _yeetHeight;

    [HideInInspector] public int _hp = 10800;
    public int _resourceObjectsCount = 0;
    public int _amountOfResources = 0;

    protected override void Awake()
    {
        if (_targetTransform == null)
            _targetTransform = this.gameObject.transform;

        base.Awake();
        _type = InteractableType.ResourceStack;
    }

    protected override void Start()
    {
        base.Start();
        SetOnGround();
        base.OnPlacement();
        transform.DOScale(1f, 1f).SetEase(Ease.OutElastic);
    }

    private void FixedUpdate()
    {
        _hp -= 1;
        if (_hp <= 0)
            Destroy(this.gameObject);

        if (_amountOfResources <= 0 && _resourceObjectsCount <= 0)
        {
            TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.DEMOLISH, 3);
            CutsceneManager._instance?.StartCutscene(1);

            Destroy(this.gameObject);
        }
    }

    public override bool Action(int strenght, Unit unit)
    {
        YeetResource(unit);
        if (_amountOfResources <= 0)
            return false;
        else
            return true;
    }

    private void YeetResource(Unit unit)
    {
        GameObject resource = Instantiate(_resourceInformation._resourcePrefab, 
            this.transform.position + new Vector3(0, _yeetHeight, 0), 
            Quaternion.identity, 
            null);

        ResourceObject resourceObject = resource.GetComponent<ResourceObject>();
        Rigidbody resourceRigidBody = resource.GetComponent<Rigidbody>();
        resourceObject.harvester = unit;
        resourceObject._resourceInformation = _resourceInformation;
        resourceObject._amount = _resourceInformation._resourceAmountPerInstantiatedGameObject;
        resourceObject.SpawnFromResource();
        resourceObject._stack = this;
        _resourceObjectsCount += 1;
        _amountOfResources -= resourceObject._amount;

        //Give playfull physics to resource
        resourceRigidBody.AddForce(new Vector3(Random.Range(-150f, 150f), 0, Random.Range(-150f, 150f)) + (Vector3.up * 500));
        resourceRigidBody.AddTorque(new Vector3(Random.Range(-110f, 110f), Random.Range(-110f, 110f), Random.Range(-110f, 110f)));
    }

    private void SetOnGround()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position + new Vector3(0, 5, 0), transform.TransformDirection(Vector3.down), out ray, Mathf.Infinity, _layerMask))
        {
            Vector3 position = ray.point;
            this.transform.position = position;
        }
    }
}
