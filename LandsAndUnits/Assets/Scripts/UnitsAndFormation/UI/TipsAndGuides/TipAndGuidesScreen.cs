using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipAndGuidesScreen : MonoBehaviour
{
    [SerializeField] private GameObject _guidePanel;
    [SerializeField] private TextMeshProUGUI _guideTitle;
    [SerializeField] private TextMeshProUGUI _guideText;
    [SerializeField] private Button _closeMenuButton;

    public delegate void TriggerMenuHighlight(MenuHighlight toHighlight);
    public static event TriggerMenuHighlight OnTriggerMenuHighlight;

    public void Awake()
    {
        _closeMenuButton.onClick.AddListener(CloseMenu);
    }

    public void ActivateGuidePoint(GuidePoint data)
    {
        _guideTitle.text = data._title;
        _guideText.text = data._text;

        _guidePanel.SetActive(true);
        _guidePanel.transform.DOLocalMoveX(600f,1f).SetEase(Ease.OutBack);
        OnTriggerMenuHighlight?.Invoke(data._toHighlight);
    }

    public void CloseMenu()
    {
        OnTriggerMenuHighlight?.Invoke(MenuHighlight.NONE);
        _guidePanel.transform.DOLocalMoveX(1300f, 1f).OnComplete(DeactivatePanel).SetEase(Ease.InBack);
    }

    private void DeactivatePanel()
    {
        _guidePanel.SetActive(false);
    }
}
