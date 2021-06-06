using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnitsAndFormation;
using DG.Tweening;
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool _notLockedToType;
    [SerializeField]
    private InputState _tooltipType;
    private InputManager _inputManager;
    public string defaultContent;
    public string header;
    private UnitInteractable _interactable;

    private IEnumerator onPointerEnterCoroutine;
    private IEnumerator onMouseEnterCoroutine;

    public void Start()
    {
        _inputManager = InputManager.Instance;
        _interactable = GetComponent<UnitInteractable>();
        onPointerEnterCoroutine = OnPointerDelay(.5f);
        onMouseEnterCoroutine = OnMouseDelay(.5f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(onPointerEnterCoroutine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(onPointerEnterCoroutine);
        TooltipSystem.Hide();
    }

    public void OnMouseEnter()
    {
        if (_inputManager?._inputState == _tooltipType || _notLockedToType)
        {
            StartCoroutine(onMouseEnterCoroutine);
        }
    }

    private IEnumerator OnPointerDelay(float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        TooltipSystem.Show(defaultContent, header);
    }

    private IEnumerator OnMouseDelay(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        //if interactable and corresponding unit is selected
        if (_interactable != null && GroupManager.Instance._selectedGroup != null)
        {
            if (GroupManager.Instance._selectedGroup._type == _interactable._whoCanInteract || _interactable._type == InteractableType.Housing)
            {
                if(_interactable._type == InteractableType.Construction)
                {
                    TooltipSystem.Show("Click to build.", header);
                    yield break;
                }
                else if (_interactable._type == InteractableType.Resource)
                {
                    TooltipSystem.Show("Click to harvest.", header);
                    yield break;
                }
            }
        }
        TooltipSystem.Show(defaultContent, header);
    }


    public void OnMouseExit()
    {
        StopCoroutine(onMouseEnterCoroutine);
        TooltipSystem.Hide();
    }

    private void OnDisable()
    {
        if(GameManager._instance._currentScene != ScenesIndexes.MAIN_MENU)
        {
            if (onMouseEnterCoroutine != null)
                StopCoroutine(onMouseEnterCoroutine);
            if (onPointerEnterCoroutine != null)
                StopCoroutine(onPointerEnterCoroutine);
        }
        TooltipSystem.Hide();
    }
}
