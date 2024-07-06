using System.Collections;
using System.Collections.Generic;
using qbfox;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    private static Manager _instance = null;
    public static Manager Instance{get{return _instance;}}

    // public DragBall Player;

    // public GameObject[] AllBallPrefabs;
    // public int[] Quantities = {8,4,2,1};
    // public float[] RecollectAmount = {1,0.5f,0.25f,0.125f};

    // [SerializeField]
    // private Transform _gizmoT;

    // [SerializeField]
    // private MeshRenderer _spawnBox;
    // private Bounds _bd;

    private qbfox.FSMSystem _fsm;

    // [SerializeField]
    // private ParticleSystem _restoreFxPrefab;
    // [SerializeField]
    // private ParticleSystem _shatterFxPrefab;
    // [SerializeField]
    // private ParticleSystem _absorbFxPrefab;
    // [SerializeField]
    // private ParticleSystem _collectFxPrefab;
    private YieldInstruction _restoreInterval;

    private AudioSource _as;

    public const string ST_M = "menu";
    public const string ST_T = "tutor";
    public const string ST_LV = "level";
    public const string ST_OV = "gameover";

    private int _currentEnergySet = 1;
    public int CurrentEnergySet{get{return _currentEnergySet;}}

    [SerializeField]
    private SfxSet[] _sfxSets;
    private Dictionary<string, AudioClip> _sfxDict = new Dictionary<string, AudioClip>();

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _fsm = new qbfox.FSMSystem(this);
        _fsm.AddState("menu", new Entry());
        _fsm.AddState("tutor", new InLevel());
        _fsm.AddState("level", new InLevel());
        _fsm.AddState("gameover", new GameOver());
        _fsm.ChangeState("menu");

        // init sfx
        foreach (SfxSet ss in _sfxSets)
        {
            _sfxDict.Add(ss.Name, ss.Clip);
        }

        // Player.Init();
        // _restoreInterval = new WaitForSeconds(0.05f);

        // _bd = new Bounds(_spawnBox.bounds.center, new Vector3(
        //     _spawnBox.bounds.size.x * _spawnBox.transform.localScale.x
        //     ,_spawnBox.bounds.size.y * _spawnBox.transform.localScale.y
        //     ,_spawnBox.bounds.size.z * _spawnBox.transform.localScale.z)
        // );
    }

    public void PlayAudio(string name, float pitch = 1.0f)
    {
        if(_as == null)
            _as = gameObject.GetComponent<AudioSource>();
        if(!_sfxDict.ContainsKey(name))
        {
            Debug.LogErrorFormat("{0} not exist", name);
            return;
        }
        AudioClip c = _sfxDict[name];
        _as.pitch = pitch;
        _as.PlayOneShot(c);
    }

    // public void PlayFallInHoleFxAndDestory(DragBall db)
    // {
    //     db.transform.DOScale(Vector3.zero, 0.8f)
    //         .OnComplete(()=>Destroy(db.gameObject));
    //     PlayAudio("fall");
    // }

    // public void PlayCollectFx(Vector3 p)
    // {
    //     PlayParticleFx(_collectFxPrefab, Color.white, p, 1, 2);
    //     PlayAudio("pickup");
    // }

    // public void PlayAbsorbFx(Color c, Vector3 p)
    // {
    //     PlayParticleFx(_absorbFxPrefab, c, p, 1, 4);
    //     PlayAudio("restore", 0.7f);
    // }

    // public void PlayRestoreFx(Color c, Vector3 p)
    // {
    //     PlayParticleFx(_restoreFxPrefab, c, p, 1, 4);
    //     PlayAudio("restore");
    //     // GameObject restore_fx = Instantiate(_restoreFxPrefab.gameObject, p, Quaternion.identity);
    //     // ParticleSystem ps = restore_fx.GetComponent<ParticleSystem>();
    //     // ParticleSystem.MainModule mm = ps.main;
    //     // mm.startColor = c;
    //     // StartCoroutine(PlayRestoreFx_Co(ps));
    // }

    // public void PlayShatterFx(Color c, Vector3 p)
    // {
    //     PlayParticleFx(_shatterFxPrefab, c, p, 15);
    //     PlayAudio("hit");
    // }

    void PlayParticleFx(ParticleSystem prefab, Color c, Vector3 p, int emitCount, int emitTimes = 1)
    {
        GameObject restore_fx = Instantiate(prefab.gameObject, p, Quaternion.identity);
        ParticleSystem ps = restore_fx.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        mm.startColor = c;
        StartCoroutine(PlayParticleFx_Co(ps, emitCount, emitTimes));
    }

    IEnumerator PlayParticleFx_Co(ParticleSystem ps,int emCount, int times)
    {
        int count = times;
        while(count > 0)
        {
            ps.Emit(emCount);
            count--;
            if(count > 0)
                yield return _restoreInterval;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(ps.gameObject);
    }

    private void Update() {
        _fsm.CurrentState.Run();
    }

    // public int GetIndexByQuantity(int val)
    // {
    //     int result = System.Array.IndexOf(Quantities, val);
    //     if(result < 0)
    //     {
    //         for(int i=0;i<Quantities.Length;i++)
    //         {
    //             if(val > Quantities[i])
    //             {
    //                 result = i;
    //                 break;
    //             }
    //         }
    //     }
    //     return result;
    // }

    // [ContextMenu("Reset")]
    // GameObject Reset()
    // {
    //     GameObject g = Instantiate(AllBallPrefabs[0], Vector3.zero, Quaternion.identity);
    //     DragBall db = g.GetComponent<DragBall>();
    //     db.SetGizmo(_gizmoT);
    //     db.Init();
    //     return g;
    // }

    // public void SetupPlayer(int lifeCharge)
    // {
    //     _currentEnergySet = lifeCharge;
	// 	GameObject flag = GameObject.FindGameObjectWithTag("startFlag");
	// 	Vector3 p = flag.transform.position;
	// 	GameObject player = Reset();
	// 	player.transform.position = new Vector3(p.x, p.y, 0);
    //     DragBall db = player.GetComponent<DragBall>();
    //     db.SetTrailCenter(player.transform.position);
    //     db.OnSplittedAction = OnSplitHandler;
    //     OnSplitHandler(db, false);
    // }

//     void OnSplitHandler(DragBall db, bool dead)
//     {
//         InLevel lv = _fsm.GetState(ST_LV) as InLevel;
//         if(dead)
//         {
//             db.OnSplittedAction = null;
//             lv.AddPlayerPiece(-1);
//         }
//         else
//         {
//             db.OnSplittedAction = OnSplitHandler;
//             lv.AddPlayerPiece(1);
//         }
//     }
}

public class Entry : FSMState
{
    public override void Activate()
    {
        base.Activate();
        // UIMain.Instance.Init();
        // UIMain.Instance.ShowMainMenu();
        // UIMain.Instance.EntryClickBoard.onClick.AddListener(()=>TryChangeState());
        MarkNextState(Manager.ST_LV);
        (m_Controller.GetState(NextStateId) as InLevel).Restart();
    }

    public override void Deactivate()
    {
        // UIMain.Instance.EntryClickBoard.onClick.RemoveAllListeners();
    }
}

public class Tutor : FSMState
{
}

public class InLevel : FSMState
{
    private int _currLevelIdx = 1;
    private Scene _currScene;

    // private Star[] _allStars;
    private int _playerPieces = 0;
    private int _starCount = 0;

    private int _energyCount = 10;

    private bool _levelLoaded = false;
    private bool _setCharging = false;

    private bool _recollecting = false;

    public void Restart()
    {
        _currLevelIdx = 1;
        _energyCount = 10;
        _playerPieces = 0;
        _starCount = 0;
    }

    public override void Activate()
    {
        base.Activate();

        _levelLoaded = false;
        _setCharging = false;

        // charge
        // UIMain.Instance.ChargeBtn.onClick.AddListener(()=>
        // {
        //     int used = UIMain.Instance.GetChargeNum();
        //     Manager.Instance.SetupPlayer(used);
        //     UIMain.Instance.ToggleChargePanel(false);
        //     _energyCount-=used;
        //     Manager.Instance.PlayAudio("charged");
        //     // _playerPieces++;
        // });

        // load level
        // AsyncOperation ao = LoadLevel(_currLevelIdx);
        // ao.completed += (a)=>
        // {
        //     // Manager.Instance.Player.Init();
        //     _allStars = Object.FindObjectsOfType<Star>();
        //     _starCount = _allStars.Length;
        //     foreach(Star s in _allStars)
        //     {
        //         s.OnStarCollected = CollectStar;
        //     }

        //     // UI
        //     UIMain.Instance.ShowHUD();
        //     _currScene = SceneManager.GetSceneByBuildIndex(_currLevelIdx);

        //     _levelLoaded = true;
        // };
    }

    public void AddPlayerPiece(int val)
    {
        // first time unlock charging
        if(_playerPieces == 0 && val == 1)
            _setCharging = false;
        this._playerPieces+=val;
        Debug.LogFormat("player piece {0}, {1} left", val, _playerPieces);
    }

    public void CollectStar()
    {
        _starCount--;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if(_currScene.path != null)
            SceneManager.UnloadSceneAsync(_currScene);
        // UIMain.Instance.ChargeBtn.onClick.RemoveAllListeners();
        // DragBall[] all = Object.FindObjectsOfType<DragBall>();
        // for(int i=0;i<all.Length;i++)
        // {
        //     all[i].OnSplittedAction = null;
        //     GameObject.Destroy(all[i].gameObject);
        // }
        _playerPieces = 0;
        _recollecting = false;
    }

    public override void Run()
    {
        if(!_levelLoaded)
            return;

        if(_recollecting)
            return;

        if(Application.isEditor && Input.GetKeyUp(KeyCode.RightBracket))
        {
            LevelPass();
            return;
        }

        // next level
        if(_playerPieces > 0 && _starCount <= 0)
        {
            LevelPass();
            return;
        }

        // UIMain.Instance.UpdateHUDStat(_energyCount, _starCount);

        if(!_setCharging && _playerPieces == 0)
        {
            // game over
            if(_energyCount == 0)
            {
                MarkNextState(Manager.ST_OV);
                TryChangeState();
                return;
            }

            // next turn
            // UIMain.Instance.SetEnergyLimit(_energyCount);
            // UIMain.Instance.ToggleChargePanel(true);
            _setCharging = true;
        }
    }

    public bool AllLevelComplete()
    {
        int lv_count = SceneManager.sceneCountInBuildSettings - 1;
        return _currLevelIdx > lv_count;
    }

    void LevelPass()
    {
        Manager.Instance.StartCoroutine(LevelPass_Co());
    }

    IEnumerator LevelPass_Co()
    {
        _currLevelIdx++;
        yield return RecollectEnergy_Co();
        yield return new WaitForSeconds(1.0f);
        if(AllLevelComplete())
        {
            // all win
            MarkNextState(Manager.ST_OV);
            (m_Controller.GetState(this.NextStateId) as GameOver).SetWin();
        }
        else
        {
            // next level
            MarkNextState(Manager.ST_LV);
        }
        TryChangeState();
    }

    IEnumerator RecollectEnergy_Co()
    {
        _recollecting = true;

        // float total = 0.0f;
        // DragBall[] balls = Object.FindObjectsOfType<DragBall>();
        // foreach (DragBall b in balls)
        //     b.DisableRigid();
        // foreach (DragBall b in balls)
        // {
        //     float bv = Manager.Instance.RecollectAmount[b.CollisionCount];
        //     int m = Manager.Instance.CurrentEnergySet;
        //     total += bv*m;
        //     UIMain.Instance.ShowCollectEnergy(b.transform.position, bv, m);
        //     yield return new WaitForSeconds(1.0f);
        // }

        // int val = Mathf.RoundToInt(total);
        // Debug.LogFormat("recollected {0} energy", val);
        // UIMain.Instance.ShowRecollect(val);
        // _energyCount += val;
        yield return new WaitForSeconds(1.0f);
    }

    public void LevelFail()
    {
        MarkNextState(Manager.ST_OV);
        TryChangeState();
    }

    AsyncOperation LoadLevel(int idx)
    {
        Scene s = SceneManager.GetSceneByBuildIndex(idx);
        Debug.LogFormat("load leve {0}:{1}",idx, s.name);
        return SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);
    }
}

public class GameOver : FSMState
{
    private bool _isWin = false;
    public void SetWin()
    {
        _isWin = true;
    }

    public override void Activate()
    {
        base.Activate();
        // UIMain.Instance.ShowGameOver(_isWin);
        // UIMain.Instance.OverClickBoard.onClick.AddListener(()=>TryChangeState());
        MarkNextState(Manager.ST_M);
    }

    public override void Deactivate()
    {
        // UIMain.Instance.OverClickBoard.onClick.RemoveAllListeners();
        _isWin = false;
    }
}

[System.Serializable]
public class SfxSet
{
    public AudioClip Clip;
    public string Name;
}