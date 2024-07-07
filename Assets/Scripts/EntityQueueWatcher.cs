using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectTools;
using UnityEngine;
//using UnityEngine.Rendering;
using UnityEngine.TestTools;

public class EntityQueueWatcher : MonoBehaviour
{
    // [SerializeField]
    // TurnConfigSO _turnCfg;

    [SerializeField, Tooltip("结算时判定条件的设定")]
    SerializableDictionary<int, WinCheck> _checkRules;

    [SerializeField, Tooltip("结算回合")]
    int TurnEnd = 8;

    Dictionary<string, IEntity> _currUserInput = new Dictionary<string, IEntity>();       // entity queue 
    // List<IEntity> _currMembers = new List<IEntity>();
    IEntity _last;

    public int _currTurn = 0;

    List<IEntity> _member_to_play_lvup = new List<IEntity>();

    public bool win = false;

    public string ResultText => _currUserInput.Values.Select(i => $"{(i as EntityBase).Setup.EntityName} Lv.{i.GetLevel()}").Aggregate((pre, next) => $"{pre} {next}");

    public async UniTask NewMemeber(IEntity entity, CancellationToken cancellationToken = default)
    {
        Debug.Assert(!_currUserInput.ContainsKey(entity.GetKey()), string.Format("memmber key {0} already exist", entity.GetKey()));

        // Debug.Log(string.Format("added entity {0}", entity.GetKey()));
        UIMain.Instance.ShowDebug(string.Format("added entity {0} -> lv1", entity.GetKey()));

        _currUserInput.Add(entity.GetKey(), entity);
        // _currMembers.Add(entity);
        _last = entity;

        // 已有成员成长，包括自增
        _member_to_play_lvup.Clear();
        CheckMember();

        await PlayMemberLvUp(cancellationToken).AttachExternalCancellation(cancellationToken);

        Debug.Assert(_currTurn < TurnEnd, "当前回合数不应高于最大回合数");
        _currTurn++;

        // 检查当前回合是否有pass check
        if (_checkRules.ContainsKey(_currTurn))
        {
            // 回合判定
            bool passed = _checkRules[_currTurn].Check(_currUserInput);
            string key_to_play = passed
                 ? _checkRules[_currTurn].AnimationTriggerPassed
                  : _checkRules[_currTurn].AnimationTriggerFailed;

            if(!string.IsNullOrEmpty(key_to_play))
            {
                // 提前结束？
                await Manager.Instance.DoPlayAnim(key_to_play, ()=>{
                    Manager.Instance.AnimCtrl.Despawn(key_to_play);
                });
            }

			// 判定失败惩罚
            if(!passed)
            {
                foreach(KeyValuePair<string, int> pair in _checkRules[_currTurn].DoApplyWhenFailed)
                {
                    await ApplyPunishment(pair.Key, pair.Value);
                }
            }

            // 最终回合
            if (_currTurn == TurnEnd)
            {
                // 最终回合需要的表现和跳转
                // await Manager.Instance.DoPlaySuccess();
                UIMain.Instance.ShowGameOver(ResultText);
                return;
            }
        }

        if (_currTurn == TurnEnd)
        {
            // 最终回合需要的表现和跳转
            // await Manager.Instance.DoPlaySuccess();
            //Manager.Instance.FSM.ChangeState("game over");
            UIMain.Instance.ShowGameOver(ResultText);

        }
    }

    private Sequence ApplyPunishment(string key, int value)
    {
        int origin_lv = _currUserInput[key].GetLevel();
        return _currUserInput[key].UpdateForce(value, true);
    }

    private async UniTask PlayMemberLvUp(CancellationToken cancellationToken = default)
    {
        foreach (IEntity e in _member_to_play_lvup)
        {
            // other fx?
            Debug.Log("play other fx lvup related? 1 sec");
            await UniTask.WhenAll(UniTask.WaitForSeconds(1.0f, cancellationToken: cancellationToken), PlayLvUpShow(e, cancellationToken))
                .AttachExternalCancellation(cancellationToken);
        }
    }

    public void Reset()
    {
        _currUserInput.Clear();
        // _currMembers.Clear();
        _last = null;
        UIMain.Instance.ShowDebug("Restarted");
        _currTurn = 0;
    }

    // member level increase if possible
    // member level increase beyond limit
    void CheckMember()
    {
        // IEntity lastOne = _last;
        // IEntity prevOneBefore = _currUserInput[_currUserInput.Count - 2];

        // switch(DoRelationCheck(prevOneBefore, lastOne))
        // {
        //     // for example
        //     case "Chip2Industry":
        //         throw new NotImplementedException();
        //         break;
        //     default:
        //         throw new Exception("invalid");
        // }

        foreach (IEntity e in _currUserInput.Values)
        {
            //if(e == _last)
            //    continue;
            bool lvUpSuccess = e.LevelUp(_currUserInput);
            if (lvUpSuccess)
            {
                _member_to_play_lvup.Add(e);
            }
        }

    }

    async UniTask PlayLvUpShow(IEntity e, CancellationToken cancellationToken)
    {
        // Debug.Log("play entity level up show, 1 sec");
        // Debug.Log(string.Format("{0} lvUp: {1} -> {2}", e.GetKey(), e.GetLevel() - 1, e.GetLevel()));
        UIMain.Instance.ShowDebug(string.Format("{0} lvUp: {1} -> {2}", e.GetKey(), e.GetLevel() - 1, e.GetLevel()));
        await e.PlayLevelUpAnimation(cancellationToken).AttachExternalCancellation(cancellationToken);
    }

    //     string DoRelationCheck(IEntity left, IEntity right)
    //     {
    //         return string.Format("{0}2{1}", left.GetKey() , right.GetKey());
    //     }
}

[System.Serializable]
public class WinCheck
{
    public SerializableDictionary<string, int> Condition = new SerializableDictionary<string, int>(){
        {"A", 4},
        {"B", 4},
        {"C", 4},
        {"D", 4},
        {"E", 4},
    };

    public bool Check(Dictionary<string, IEntity> userInput)
    {
        foreach(KeyValuePair<string ,int> pair in Condition)
        {
            if(!userInput.ContainsKey(pair.Key))
                return false;
            IEntity e = userInput[pair.Key];
            if(e.GetLevel() < Condition[e.GetKey()])
                return false;
        }
        return true;
    }

    public string AnimationTriggerPassed = "";
    public string AnimationTriggerFailed = "";

    public SerializableDictionary<string, int> DoApplyWhenFailed;
}