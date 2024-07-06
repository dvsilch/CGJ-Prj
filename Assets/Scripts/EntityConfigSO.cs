using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityConfigSO", menuName = "Entity Config")]
public class EntityConfigSO : ScriptableObject
{
    [SerializeField]
    private string entityName;

    [SerializeField]
    [TextArea]
    private string entityDescription;

    [SerializeField]
    private Sprite entityIcon;

    public string EntityName => entityName;

    public string EntityDescription => entityDescription;

    public Sprite EntityIcon => entityIcon;

    public void Check(List<EntityConfigSO> list)
    {
        if (!list.Contains(this))
            return;
    }
}
