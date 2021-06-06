using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitsAndFormation;
using DG.Tweening;
public class TalkCloudComponent : MonoBehaviour
{
    [SerializeField] GameObject _cloud;
    public SpriteRenderer _icon;
    public UnitWarningType _currentWarning { private set; get; }
    private UnitWarningType _tempWarning;

    private bool _isPermanentMode;
    private bool _overridden;
    private bool _inAction;

    public void EnableCloud(UnitWarningType warning, bool permanent = default)
    {
        if (!_inAction)
        {
            _inAction = true;
            if (_isPermanentMode && !_overridden)
            {
                _tempWarning = warning;
                _cloud.transform.DOKill();
                _cloud.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutElastic).OnComplete(OverrideCall);
                return;
            }

            if (WarningDatabase.GetPriorityWeight(warning) >= WarningDatabase.GetPriorityWeight(_currentWarning))
            {
                _cloud.transform.DOKill();
                _currentWarning = warning;
                _icon.sprite = WarningDatabase.GetIcon(warning);
                _cloud.SetActive(true);

                if (permanent)
                {
                    _isPermanentMode = true;
                    _cloud.transform.DOScale(new Vector3(.1f,.1f,.1f), 1f).SetEase(Ease.InOutElastic).OnComplete(InAction);
                    return;
                }

                _cloud.transform.DOScale(new Vector3(.1f, .1f, .1f), 1f).SetEase(Ease.InOutElastic);
                if (!permanent)
                {
                    DisableCloud();
                }
            }
        }
    }

    #region Override permanent
    private void OverrideCall()
    {
        _icon.sprite = WarningDatabase.GetIcon(_tempWarning);
        _cloud.SetActive(true);
        _cloud.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic);
        DisableAnimation(3f);
        _overridden = true;
    }

    private void DisableAnimation(float delay)
    {
        _cloud.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutElastic).SetDelay(delay).OnComplete(ReEnablePermanent);
    }

    private void ReEnablePermanent()
    {
        _inAction = false;
        EnableCloud(_currentWarning, true);
    }
    #endregion

    public void DisableCloud()
    {
        _cloud.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutElastic).SetDelay(3f).OnComplete(OnDisable);
    }

    private void InAction()
    {
        _inAction = false;
    }

    private void OnDisable()
    {
        _currentWarning = UnitWarningType.none;
        InAction();
        _cloud.SetActive(false);
    }
}
