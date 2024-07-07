using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsGroup : MonoBehaviour
{
    [SerializeField]
    private List<EntityButton> buttons;
    public List<EntityButton> Buttons{get{return buttons;}}

    [SerializeField]
    private Image entityIcon;

    [SerializeField]
    private TMPro.TextMeshProUGUI entityName;

    [SerializeField]
    private TMPro.TextMeshProUGUI entityDescription;

    [SerializeField]
    private List<EntityConfigSO> clickedEntities = new List<EntityConfigSO>(5);

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons)
        {
            button.OnEntityPointerEnter += OnEntityPointerEnter;
            button.OnEntityPointerEnter += HoverBtnFx;
            button.OnEntityPointerExit += OnEntityPointerExit;
            button.OnEntityPointerExit += ExitBtnFx;
            button.OnEntityClick += PressedBtnFx;
        }
    }

    private void OnEntityPointerEnter(EntityConfigSO entityConfig, RectTransform rt)
    {
        entityName.text = entityConfig.EntityName;
        entityDescription.text = entityConfig.EntityDescription;
        entityIcon.sprite = entityConfig.EntityIcon;
    }

    private void OnEntityPointerExit(EntityConfigSO entityConfig, RectTransform rt)
    {
        entityName.text = "";
        entityDescription.text = "";
        entityIcon.sprite = null;
    }

    void HoverBtnFx(EntityConfigSO entityCfg, RectTransform rt){
        DOTween.Sequence()
            .Append(rt.DOScale(Vector2.one * 1.2f, 0.1f))
            .Append(rt.DOScale(Vector2.one, 0.1f));
    }

    void ExitBtnFx(EntityConfigSO entityCfg, RectTransform rt){
        rt.localScale = Vector2.one;
    }

    void PressedBtnFx(EntityConfigSO entityCfg, RectTransform rt){
        rt.DOPunchScale(Vector2.one * 1.2f, 0.2f, 3);
    }
}
