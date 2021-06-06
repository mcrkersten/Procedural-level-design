using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitsAndFormation
{
    public class HarvestModule : BehaviourModule
    {
        public override IEnumerator ModuleSpecificInteractionIEnumerator(float reloadTime, float animationTime)
        {
            //If this resource is not in cooldown
            if (!((ResourceInteractable)_unitBrain._targetInformation._interactable)._inCooldown)
            {
                _unitBrain._memory._lastInteractedResource = (ResourceInteractable)_unitBrain._targetInformation._interactable;
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

                if (_unitBrain._targetInformation != null)
                    StartCoroutine(ModuleSpecificInteractionIEnumerator(reloadTime, animationTime));
            }
        }

        protected override void FollowupAction()
        {
            //Search for new resource to harvest
            StopAllCoroutines();
            ReturnToHarvesting();
        }
    }
}
