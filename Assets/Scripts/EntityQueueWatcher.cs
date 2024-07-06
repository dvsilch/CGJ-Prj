using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityQueueWatcher : MonoBehaviour
{
    List<IEntity> _currUserInput;       // entity queue 
    IEntity _last;

    [SerializeField]
    TurnSet _turnSet;

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
