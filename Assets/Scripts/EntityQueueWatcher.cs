using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.TestTools;

public class EntityQueueWatcher : MonoBehaviour
{
    List<IEntity> _currUserInput;       // entity queue 
    IEntity _last;

    [SerializeField]
    TurnConfigSO _turnCfg;

	[SerializeField]
    WinCheck _checkRule;

    int _currTurn = 0;

    public void NewMemeber(IEntity entity)
    {
        _currUserInput.Add(entity);
        _last = entity;

        CheckMember();
        CheckTurn();
    }

    void CheckTurn()
    {
        foreach(IEntity e in _currUserInput)
        {

        }
    }

	// member level increase if possible
    // member level increase beyond limit
    void CheckMember(){
        IEntity lastOne = _last;
        IEntity prevOneBefore = _currUserInput[_currUserInput.Count - 2];

        switch(DoRelationCheck(prevOneBefore, lastOne))
        {
            // for example
            case "Chip2Industry":
                throw new NotImplementedException();
                break;
            default:
                throw new Exception("invalid");
        }
    }

    string DoRelationCheck(IEntity left, IEntity right)
    {
        return string.Format("{0}2{1}", left.GetKey() , right.GetKey());
    }
}

[System.Serializable]
public class WinCheck
{
    public static Dictionary<string, int> Condition = new Dictionary<string, int>(){
        {"A", 4},
        {"B", 4},
        {"C", 4},
        {"D", 4},
        {"E", 4},
    };

    public static bool Check(List<IEntity> entities)
    {
        foreach(IEntity e in entities)
        {
            Debug.Assert(Condition.ContainsKey(e.GetKey()), string.Format("entity key {0} not included in winning condition set"));
            if(e.GetLevel() < Condition[e.GetKey()])
                return false;
        }
        return true;
    }
}