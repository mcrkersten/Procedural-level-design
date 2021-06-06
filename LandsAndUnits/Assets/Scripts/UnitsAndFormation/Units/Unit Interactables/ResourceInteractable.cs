 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ResourceEffect))]
public class ResourceInteractable : UnitInteractable
{
    public int _resourceAmountPerInstantiatedGameObject { private set; get; }
    private int _instantiatedGameObjectsPerHarvest;
    public ResourceInformation _resourceInformation;
    public float _yeetHeight;

    private ResourceEffect _effect;
    private int _hitPointsToHarvest;
    private int _harvestsTillCooldown;
    public bool _inCooldown { private set; get; }

    public float _yVectorOffset = 0;

    protected override void Awake()
    {
        if(_targetTransform == null)
            _targetTransform = this.gameObject.transform;

        base.Awake();
        _type = InteractableType.Resource;
        _effect = GetComponent<ResourceEffect>();
    }

    protected override void Start()
    {
        base.Start();
        _hitPointsToHarvest = _resourceInformation._hitsToHarvest;
        _harvestsTillCooldown = _resourceInformation._harvestTillCooldown;
        _instantiatedGameObjectsPerHarvest = _resourceInformation._instantiatedGameObjectsPerHarvest;
        _resourceAmountPerInstantiatedGameObject = _resourceInformation._resourceAmountPerInstantiatedGameObject;
        this.GetComponent<ResourceEffect>().Initiate();
        base.OnPlacement();
    }

    public void SetResourceInformationAndStart(ResourceInformation scriptable)
    {
        _resourceInformation = scriptable;
        Start();
    }

    public void TriggerResourceCooldown()
    {
        StartCooldown();
    }

    public override bool Action(int unitStrenght, Unit unit)
    {
        if (!_inCooldown)
        {
            HitEffect();
            _hitPointsToHarvest -= unitStrenght;
            if (_hitPointsToHarvest <= 0)
            {
                ThrowResourceObject(unit, _instantiatedGameObjectsPerHarvest);
                _hitPointsToHarvest = _resourceInformation._hitsToHarvest;
                _harvestsTillCooldown -= 1;

                if (_harvestsTillCooldown <= 0)
                {
                    StartCooldown();
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void ThrowResourceObject(Unit unit, int amountToInstantiate)
    {
        //Yeet the resources
        for (int i = 0; i < amountToInstantiate; i++)
            YeetResource(unit);
    }

    private void YeetResource(Unit unit)
    {
        //Instantiate resource
        GameObject resource = Instantiate(_resourceInformation._resourcePrefab, 
            this.transform.position + new Vector3(0, _yeetHeight, 0), 
            Quaternion.identity, 
            null);

        ResourceObject o = resource.GetComponent<ResourceObject>();
        Rigidbody resourceRigidBody = resource.GetComponent<Rigidbody>();
        o.harvester = unit;
        o._resourceInformation = _resourceInformation;
        o._amount = _resourceInformation._resourceAmountPerInstantiatedGameObject;
        o.SpawnFromResource();

        //Give playfull physics to resource
        resourceRigidBody.AddForce(new Vector3(Random.Range(-150f, 150f), 0, Random.Range(-150f, 150f)) + (Vector3.up * 500));
        resourceRigidBody.AddTorque(new Vector3(Random.Range(-110f, 110f), Random.Range(-110f, 110f), Random.Range(-110f, 110f)));
    }

    private void StartCooldown()
    {
        _inCooldown = true;
        StartCoroutine(CoolDownEffect(_resourceInformation._cooldownTime));
        _harvestsTillCooldown = _resourceInformation._harvestTillCooldown;
    }

    private void EndCooldown()
    {
        _inCooldown = false;
    }

    private void HitEffect()
    {
        if (_animator != null)
            _animator.SetTrigger("Hit");
        if (_effect != null)
            _effect.ResourceHarvestEffectTrigger(_harvestsTillCooldown);
    }

    public override void OnMouseEnter()
    {
        if (InputManager.Instance?._inputState == InputState.MENU_HOVER)
            return;

        if (GameManager._instance._gameState != GameState.PAUSED)
        {
            base.OnMouseEnter();
            if (_outlineState != OutlineState.Selected)
            {
                SetOutlineState(OutlineState.AboutToBeSelected);
                SendInteractableDelegate(this, OutlineState.AboutToBeSelected);
            }
            SendCursorDelegate(CursorType.Harvest);
        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        if (UnitSelection.Instance != null)
            UnitSelection.Instance._hoveredUnitInteractable = null;
    }

    public IEnumerator CoolDownEffect(float cooldownTime)
    {
        float elapsedTime = 0;
        while(elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            float timeTillCooldownOver = cooldownTime - elapsedTime;
            _effect.CooldownEffect(timeTillCooldownOver);
            yield return null;
        }
        EndCooldown();
    }
}
