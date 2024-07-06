using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour, IEntity
{
    [SerializeField]
    EntityConfigSO Setup;

    int _currLevel = 1;

    bool CanLevelUp(Dictionary<string, IEntity> currMembers)
    {
        if(_currLevel < Setup.LevelSelfIncCap)
            return true;
        bool conditionSatisfied = true;
        bool foundKey = false;
        foreach(KeyValuePair<string, IEntity> m in currMembers)
        {
            // 若当前场景内升级条件满足此实体的升级条件，则true
            if(Setup.LvUpCondition.ContainsKey(m.Key))
            {
                foundKey |= true;
                bool condition_match = Setup.LvUpCondition[m.Key].CheckEntityLevel >= m.Value.GetLevel();
                int lv_cap_in_this_situation = Setup.LevelSelfIncCap + Setup.LvUpCondition[m.Key].BuffVal;
                bool reach_cap = _currLevel >= lv_cap_in_this_situation;
                conditionSatisfied &= condition_match && !reach_cap;
                // debug only
                // if(Setup.Key == "A")
                // {
                //     if(m.Value.GetLevel() >= 4)
                //         Debug.Log("A");
                // }
            }
        }

        return foundKey && conditionSatisfied;

        // throw new System.NotImplementedException();
    }

    public void DoThingsWhenSpawn()
    {
        throw new System.NotImplementedException();
    }

    public string GetKey()
    {
        Debug.Assert(Setup.Key != "NotSet", "entity key not set");
        return Setup.Key;
    }

    public int GetLevel()
    {
        return _currLevel;
    }

    public bool LevelUp(Dictionary<string, IEntity> currMember)
    {
        if(CanLevelUp(currMember))
        {
            _currLevel++;
            // do levelup show
            Debug.Log(string.Format("entity {0} lvup: {1} -> {2}", GetKey(), GetLevel() - 1, GetLevel()));
            return true;
        }
        return false;
    }
}

public interface IEntity
{
    public void DoThingsWhenSpawn();
    public string GetKey();
    public int GetLevel();
    public bool LevelUp(Dictionary<string, IEntity> currMember);      // return true if increased
}
