using System;
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

    public string EntityName => entityName;

    public string EntityDescription => entityDescription;

	// props
    public int Level = 1;
    public int LevelSelfIncCap = 3;
    public string Key = "NotSet";

	[SerializeField]
    public Dictionary<string, int> LvUpCondition;

    public void Check(List<EntityConfigSO> list)
    {
        if (!list.Contains(this))
            return;
    }

    internal bool CanLvUp()
    {
        throw new NotImplementedException();
    }
}
