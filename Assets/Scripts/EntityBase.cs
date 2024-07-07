using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityBase : MonoBehaviour, IEntity
{
    [System.Flags]
    public enum TransitionType
    {
        ModelScale = 1 << 1,
    }

    [System.Serializable]
    public class TransitionData
    {
        public TransitionType type = TransitionType.ModelScale;

        public GameObject model;

        public float initialScale = 0f;

        public float scale = 1f;

        public float duration = 1f;
    }

    [SerializeField]
    public EntityConfigSO Setup;

    [SerializeField]
    private List<TransitionData> transitionDatas;

    int _currLevel = 0;

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
            // Debug.Log(string.Format("entity {0} lvup: {1} -> {2}", GetKey(), GetLevel() - 1, GetLevel()));
            UIMain.Instance.ShowDebug(string.Format("entity {0} lvup: {1} -> {2}", GetKey(), GetLevel() - 1, GetLevel()));
            return true;
        }
        return false;
    }

    public async UniTask PlayLevelUpAnimation(CancellationToken cancellationToken)
    {
        if (Setup.Key == "D")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                var originScale = child.localScale;
                child.localScale = Vector3.zero;
                await child.DOScale(originScale, 1f).ToUniTask(cancellationToken: cancellationToken);
            }
            return;
        }

        if (transitionDatas == null || transitionDatas.Count <= _currLevel - 1)
            return;

        TransitionData data = transitionDatas[_currLevel - 1];

        if (data?.model == null)
        {
            UIMain.Instance.ShowDebug("model not set");
            Debug.LogError("model not set", gameObject);
            return;
        }

        data.model.SetActive(true);
        if (data.type.HasFlag(TransitionType.ModelScale))
        {
            data.model.transform.localScale = Vector3.one * data.initialScale;
            await data.model.transform.DOScale(data.scale, data.duration).ToUniTask(cancellationToken: cancellationToken);

            UIMain.Instance.ShowDebug("level up animation done");
            Debug.Log("level up animation done", data.model);
        }
    }
}

public interface IEntity
{
    public void DoThingsWhenSpawn();
    public string GetKey();
    public int GetLevel();
    public bool LevelUp(Dictionary<string, IEntity> currMember);      // return true if increased
    public UniTask PlayLevelUpAnimation(CancellationToken cancellationToken);
}
