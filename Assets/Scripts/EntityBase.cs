using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour, IEntity
{
    [SerializeField]
    EntitySet Setup;

    int _currLevel = 1;

    public void DoThingsWhenSpawn()
    {
        throw new System.NotImplementedException();
    }

    public string GetKey()
    {
        throw new System.NotImplementedException();
    }

    public int GetLevel()
    {
        return _currLevel;
    }

    public bool LevelUp(bool ignoreLimit)
    {
        int before = _currLevel;
        _currLevel++;
        if(!ignoreLimit)
            _currLevel = Mathf.Min(Setup.LevelSelfIncCap);
        return before < _currLevel;
    }
}

public interface IEntity
{
    public void DoThingsWhenSpawn();
    public string GetKey();
    public int GetLevel();
    public bool LevelUp(bool ignoreLimit);      // return true if increased
}
