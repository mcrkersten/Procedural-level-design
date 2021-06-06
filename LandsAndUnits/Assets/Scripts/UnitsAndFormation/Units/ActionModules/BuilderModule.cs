using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
public class BuilderModule : BehaviourModule
{
    /// <summary>
    /// When this UnitInteractable is scheduled for destruction, execute DestroyAction
    /// </summary>
    public override void OnDestruction()
    {
        DestroyAction();
    }

    /// <summary>
    /// When a builder contacts this Construction, it checks if it needs any resources, if not. Building can begin.
    /// Else if the unit has freeWill it can go collect the needed resources for this building.
    /// </summary>
    protected override void OnConstruction()
    {
        base.OnConstruction();
        FillConstructionCost((Construction)_unitBrain._targetInformation._interactable);
        if (((Construction)_unitBrain._targetInformation._interactable)._isFilled)
        {
            StartCoroutine(ModuleSpecificInteractionIEnumerator(_cooldownTime, _animationTime));
        }
        else
        {
            if (_unitBrain._memory._hasFreeWill)
            {
                foreach (StockpileInformation stockpile in ((Construction)_unitBrain._targetInformation._interactable)._stockpiles)
                {
                    if (stockpile._currentStockAmount < stockpile._max)
                    {
                        CollectResource(stockpile._resourceType, InteractableType.Storage, _unitBrain._targetInformation._interactable);
                        return;
                    }
                }
            }
            else
            {
                Debug.Log("Unit does not have enough resources for construction");
            }
        }
    }

    /// <summary>
    /// Enumerator that loops the ModuleInteraction with animation
    /// </summary>
    /// <param name="reloadTime">Time it takes to loop back to animation-start</param>
    /// <param name="animationTime">time for animation to reach 'hit-position'</param>
    /// <returns></returns>
    public override IEnumerator ModuleSpecificInteractionIEnumerator(float reloadTime, float animationTime)
    {
        float elapsedTime = 0;
        _animationController.TriggerAttack();

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ModuleSpecificInteraction();
        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_unitBrain._targetInformation != null && !((Construction)_unitBrain._targetInformation._interactable)._isBuild)
            StartCoroutine(ModuleSpecificInteractionIEnumerator(reloadTime, animationTime));
    }

    private void FillConstructionCost(Building interactable)
    {
        interactable.DepositInToStorage(_unitBrain._unitBody);
    }

    private void DestroyAction()
    {
        _animationController.TriggerAttack();
        if (_unitBrain._targetInformation._interactable.DestroyInteractableAction())
            FollowupAction();
    }
}
