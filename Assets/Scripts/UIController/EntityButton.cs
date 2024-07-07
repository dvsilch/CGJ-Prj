using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityButton : MonoBehaviour
{
    [SerializeField]
    private EventTrigger eventTrigger;

    [SerializeField]
    private Button button;

    [SerializeField]
    private EntityConfigSO entityConfig;

    [SerializeField]
    private Image entityIcon;

    [SerializeField]
    private TMPro.TextMeshProUGUI suffix;

    [SerializeField]
    private GameObject suffixContainer;

    public bool IsClicked { get; private set; } = false;

    public event Action<EntityConfigSO, RectTransform> OnEntityPointerEnter;

    public event Action<EntityConfigSO, RectTransform> OnEntityPointerExit;

    public event Action<EntityConfigSO, RectTransform> OnEntityClick;

    public bool isReigistered;

    // Start is called before the first frame update
    void Start()
    {
        eventTrigger.triggers = new List<EventTrigger.Entry>(2);
        EventTrigger.Entry pointerEnterEntry = new()
        {
            eventID = EventTriggerType.PointerEnter,
        };
        pointerEnterEntry.callback.AddListener(OnPointerEnter);
        eventTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new()
        {
            eventID = EventTriggerType.PointerExit,
        };
        pointerExitEntry.callback.AddListener(OnPointerExit);
        eventTrigger.triggers.Add(pointerExitEntry);

        button.onClick.AddListener(OnClick);

        entityIcon.sprite = entityConfig.EntityIcon;

        Restart();
    }

    private void OnClick()
    {
        if (Manager.Instance.FSM.CurrentState is InLevel2 level && level.LvUpCompleted)
        {
            IsClicked = true;
            suffix.text = (Manager.Instance.Watcher._currTurn + 1).ToString();
            suffixContainer.SetActive(true);
            OnEntityClick?.Invoke(entityConfig, button.GetComponent<RectTransform>());
            button.interactable = false;
        }
    }

    public void SetButtonInteractable(bool interactable)
    {
        button.interactable = !IsClicked && interactable;
    }

    public void Restart()
    {
        suffix.text = "";
        suffixContainer.SetActive(false);
        button.interactable = true;
        IsClicked = false;
    }

    private void OnPointerEnter(BaseEventData eventData)
    {
        OnEntityPointerEnter?.Invoke(entityConfig, button.GetComponent<RectTransform>());
    }

    private void OnPointerExit(BaseEventData eventData)
    {
        OnEntityPointerExit?.Invoke(entityConfig, button.GetComponent<RectTransform>());
    }
}
