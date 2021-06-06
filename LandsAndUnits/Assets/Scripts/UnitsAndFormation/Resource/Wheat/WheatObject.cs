using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatObject : MonoBehaviour
{
    [SerializeField] private GameObject _flower;
    [SerializeField] private GameObject _stem;

    [HideInInspector] public float _growScale;

    public void DeActivateWheatFlower()
    {
        _flower.SetActive(false);
    }
    private void ActivateWheatFlower()
    {
        _flower.SetActive(true);
    }

    public void DeActivateWheatSteam()
    {
        _stem.SetActive(false);
    }
    private void ActivateWheatSteam()
    {
        _stem.SetActive(true);
    }

    public void Grow(float amount)
    {
        float scaleAxis = Mathf.Lerp(0, _growScale, amount);
        Vector3 scale = new Vector3(scaleAxis, scaleAxis, scaleAxis);
        this.transform.localScale = scale;
        _flower.transform.localScale = scale;
        ActivateWheatSteam();
        ActivateWheatFlower();
    }
}
