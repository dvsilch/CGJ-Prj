using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityButton : MonoBehaviour
{
    private static int clickCount = 0; // TODO: get clicked index from the manager

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

    public event Action<EntityConfigSO> OnEntityPointerEnter;

    public event Action<EntityConfigSO> OnEntityPointerExit;

    public event Action<EntityConfigSO> OnEntityClick;

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
        if (true) // TODO: Add condition
        {
            clickCount++;
            suffix.text = clickCount.ToString();
            suffixContainer.SetActive(true);
            OnEntityClick?.Invoke(entityConfig);
        }
    }

    public void Restart()
    {
        clickCount = 0;
        suffix.text = "";
        suffixContainer.SetActive(false);
    }

    private void OnPointerEnter(BaseEventData eventData)
    {
        OnEntityPointerEnter?.Invoke(entityConfig);
    }

    private void OnPointerExit(BaseEventData eventData)
    {
        OnEntityPointerExit?.Invoke(entityConfig);
    }
}
