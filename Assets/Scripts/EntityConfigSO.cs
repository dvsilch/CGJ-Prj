using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectTools;
using Sirenix.Serialization;

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

	// props
    public int LevelSelfIncCap = 3;
    public string Key = "NotSet";

	[SerializeField]
    public SerializableDictionary<string, LvUpCondition> LvUpCondition;

    public Sprite EntityIcon => entityIcon;

    public void Check(List<EntityConfigSO> list)
    {
        if (!list.Contains(this))
            return;
    }
}


[System.Serializable]
public class LvUpCondition
{
    public int CheckEntityLevel;    // 检查对应目标实体的级别是否满足要求
    public int BuffVal;         // 能增长几级
}