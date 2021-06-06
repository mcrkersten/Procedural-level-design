using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;

public class ResourceEffect : MonoBehaviour
{
    private ResourceInteractable _resource;
    private List<ParticleSystem> _particles = new List<ParticleSystem>();
    private WheatPatchManager _wheatPatchManager;

    public void Initiate()
    {
        _resource = this.GetComponent<ResourceInteractable>();
        _particles = new List<ParticleSystem>(this.GetComponentsInChildren<ParticleSystem>());
        switch (_resource._resourceInformation._resourceType)
        {
            case ResourceType.Wheat:
                _wheatPatchManager = this.GetComponent<WheatPatchManager>();
                _wheatPatchManager.ActivateWheatField();
                break;
            default:
                break;
        }
    }

    public void ResourceHarvestEffectTrigger(int tillCooldown)
    {
        switch (_resource._resourceInformation._resourceType)
        {
            case ResourceType.Wood:
                WoodEffect();
                break;
            case ResourceType.Stone:
                StoneEffect();
                break;
            case ResourceType.Wheat:
                WheatEffect(tillCooldown);
                break;
            default:
                break;
        }
    }

    public void CooldownEffect(float timeTillCooldownOver)
    {
        switch (_resource._resourceInformation._resourceType)
        {
            case ResourceType.Wood:
                break;
            case ResourceType.Stone:
                break;
            case ResourceType.Wheat:
                GrowWheat(timeTillCooldownOver);
                break;
            default:
                break;
        }
    }

    private void WoodEffect()
    {
        ParticleSystems();
    }

    private void StoneEffect()
    {
        ParticleSystems();
    }

    #region Wheat
    private void WheatEffect(int tillCooldown)
    {
        _wheatPatchManager.WheatHarvest(_resource._resourceInformation._harvestTillCooldown, tillCooldown);
    }

    private void GrowWheat(float timeTillCooldownOver)
    {
        float percentage = 1 - (timeTillCooldownOver / _resource._resourceInformation._cooldownTime);
        _wheatPatchManager.ReGrowWheat(percentage);
    }
    #endregion

    private void ParticleSystems()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Emit(Random.Range(5, 10));
        }
    }
}
