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
            button.OnEntityPointerExit += OnEntityPointerExit;
        }
    }

    private void OnEntityPointerEnter(EntityConfigSO entityConfig)
    {
        entityName.text = entityConfig.EntityName;
        entityDescription.text = entityConfig.EntityDescription;
        entityIcon.sprite = entityConfig.EntityIcon;
    }

    private void OnEntityPointerExit(EntityConfigSO entityConfig)
    {
        entityName.text = "";
        entityDescription.text = "";
        entityIcon.sprite = null;
    }
}
