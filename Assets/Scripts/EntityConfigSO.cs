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
}
