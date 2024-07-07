using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    static UIMain _instance = null;
    public static UIMain Instance{get{return _instance;}}

    [SerializeField]
    ButtonsGroup _entities;
    public ButtonsGroup Entities{get{return _entities;}}

	[SerializeField]
    ResetButton _resetBtn;
    public ResetButton ResetBtn{get{return _resetBtn;}}

    public TextMeshProUGUI DebugText;

    public GameObject gameoverModal;

    public TMPro.TextMeshProUGUI gameoverText;

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        DebugText.text = "";
#if UNITY_EDITOR
        DebugText.gameObject.SetActive(true);
#else
        DebugText.gameObject.SetActive(false);
#endif
    }

    public void ShowDebug(string s)
    {
        DebugText.text += s + "\n";
    }

    public void ShowGameOver(string text)
    {
        gameoverModal.SetActive(true);
        gameoverText.text = text;
    }

    public void HideGameOver()
    {
        gameoverModal.SetActive(false);
    }
}
