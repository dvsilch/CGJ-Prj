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
    private RectTransform tip;

    [SerializeField]
    private TMPro.TextMeshProUGUI tipText;


    // Start is called before the first frame update
    void Start()
    {
        SetTipText("123iy8iah sodfjialn sdglnkaojrdoasjuhot gujoufopdijoiqjiwoghrnaoljholja");

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
    }

    private void OnPointerEnter(BaseEventData eventData)
    {
        Debug.Log("Pointer Enter");
        tip.gameObject.SetActive(true);
    }

    private void OnPointerExit(BaseEventData eventData)
    {
        Debug.Log("Pointer Exit");
        tip.gameObject.SetActive(false);
    }

    private void SetTipText(string text)
    {
        tipText.text = text;
        tip.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tip);
        tip.anchoredPosition = new Vector2(tip.anchoredPosition.x, tip.rect.height);
        tip.gameObject.SetActive(false);
    }
}
