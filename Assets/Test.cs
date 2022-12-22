using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JungExtension.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private ExtensionToggleSwitch toggleSwitch;

    void Start()
    {
        toggleSwitch.Initialized(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
