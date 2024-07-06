using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        if (Manager.Instance.FSM.CurrentState is InLevel2 inLevel)
        {
            inLevel.Restart();
        }
    }
}
