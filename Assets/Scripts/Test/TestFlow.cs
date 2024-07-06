using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestFlow : MonoBehaviour
{
    [SerializeField]
    EntityQueueWatcher _watcher;

    List<IEntity> _waitlist = new List<IEntity>();

    Dictionary<KeyCode, IEntity> _map = new Dictionary<KeyCode, IEntity>();

	[SerializeField]
    EntityBase[] _entityForTest;

    // List<EntityBase> _entitys = new List<EntityBase>();

    private void Start() {
        Debug.Assert(_entityForTest.Length > 0);

        _map.Add(KeyCode.Alpha1, CreateInstance(_entityForTest[0]));
        _map.Add(KeyCode.Alpha2, CreateInstance(_entityForTest[1]));
        _map.Add(KeyCode.Alpha3, CreateInstance(_entityForTest[2]));
        _map.Add(KeyCode.Alpha4, CreateInstance(_entityForTest[3]));
        _map.Add(KeyCode.Alpha5, CreateInstance(_entityForTest[4]));
    }

    EntityBase CreateInstance(EntityBase prefab)
    {
        EntityBase eb =Instantiate(prefab.gameObject).GetComponent<EntityBase>(); 
        _waitlist.Add(eb);
        return eb;
    }

    private void OnGUI() {
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            KeyCode k = Event.current.keyCode;
            if(_map.ContainsKey(k))
            {
                if(_waitlist.Contains(_map[k]))
                {
                    _watcher.NewMemeber(_map[k]);
                    _waitlist.Remove(_map[k]);
                }
            }
        }

    }

    private void Update() {
    }
}
