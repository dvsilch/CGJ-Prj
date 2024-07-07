using System.Collections;
using System.Collections.Generic;
using ProjectTools;
using UnityEngine;

public class StandaloneAnim : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<string, GameObject> AnimationClipSet;

    SerializableDictionary<string, GameObject> _instances = new SerializableDictionary<string, GameObject>();

    public float Play(string key)
    {
        if(!AnimationClipSet.ContainsKey(key))
        {
            Debug.LogError("failed to fetch fx prefab with key" + key);
            return 0f;
        }
        GameObject fx_go = Instantiate(AnimationClipSet[key]);
        _instances.Add(key, fx_go);
        fx_go.GetComponent<Animation>().Play();
        float duration = fx_go.GetComponent<Animation>().clip.length;
        return duration;
    }

    public void Despawn(string key)
    {
        if(!_instances.ContainsKey(key))
        {
            Debug.LogError("no existing fx gameobject in scene with key " + key);
            return;
        }
        
        Destroy(_instances[key]);
    }
}
