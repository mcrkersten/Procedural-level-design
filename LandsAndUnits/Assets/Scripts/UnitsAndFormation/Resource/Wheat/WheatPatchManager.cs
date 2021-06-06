using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatPatchManager : MonoBehaviour
{
    [SerializeField] private bool _randomNoise;
    [SerializeField] private GameObject _wheatPrefab;
    private List<WheatObject> _wheats = new List<WheatObject>();
    private List<WheatObject> _harvestedWheats = new List<WheatObject>();

    [Tooltip("Spacing between wheats")]
    [SerializeField] private Vector2 _spacing;
    [SerializeField] private Vector2 _size;

    [Header("Grow Scale")]
    [SerializeField] private float _growScale;

    private int tryRate = 0;

    public void ActivateWheatField()
    {
        for (float x = (_spacing.x/2) + .05f; x < _size.x - .05f; x += _spacing.x)
        {
            for (float y = (_spacing.y/2) + .05f; y < _size.y - .05f; y += _spacing.y)
            {
                Vector3 position;
                if (_randomNoise)
                    position = new Vector3(Random.Range(x - .05f, x + .05f), 0, Random.Range(y - .05f, y + .05f));
                else
                    position = new Vector3(x, 0, y);

                GameObject g = Instantiate(_wheatPrefab, Vector3.zero, Quaternion.identity, this.transform);
                g.transform.localPosition = position - new Vector3(.5f,0,.5f);
                WheatObject wheat = g.GetComponent<WheatObject>();
                wheat._growScale = _growScale;
                wheat.DeActivateWheatSteam();
                wheat.DeActivateWheatFlower();
                _wheats.Add(wheat);
            }
        }

        this.GetComponent<ResourceInteractable>().TriggerResourceCooldown();
    }

    public void WheatHarvest(int fixedHitsTillCooldown, int currentHitsTillCooldown)
    {
        tryRate = 0;
        float number = Mathf.Round(_wheats.Count / fixedHitsTillCooldown);
        number += _harvestedWheats.Count;

        while (_harvestedWheats.Count < number && tryRate < 50)
        {
            if (currentHitsTillCooldown == 1)
            {
                DeactivateRemainingWheats();
                break;
            }

            DeactivateWheats();
        }
        _harvestedWheats = new List<WheatObject>();
    }

    public void ReGrowWheat(float growAmount)
    {
        foreach (WheatObject wheatStem in _wheats)
        {
            wheatStem.Grow(growAmount);
        }
    }

    private void DeactivateRemainingWheats()
    {
        foreach (WheatObject item in _wheats)
        {
            if (!_harvestedWheats.Contains(item))
            {
                item.DeActivateWheatFlower();
                item.DeActivateWheatSteam();
            }
        }
    }

    private void DeactivateWheats()
    {
        int toPick = Random.Range(0, (int)_wheats.Count);
        if (!_harvestedWheats.Contains(_wheats[toPick]))
        {
            _harvestedWheats.Add(_wheats[toPick]);
            _wheats[toPick].DeActivateWheatFlower();
            _wheats[toPick].DeActivateWheatSteam();
            tryRate = 0;
        }
        tryRate++;
    }
}
