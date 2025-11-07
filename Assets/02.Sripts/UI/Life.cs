using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.LifeUI();
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.LifeUI();
    }
}
