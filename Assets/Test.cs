using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JungExtension.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ExtensionScrollSnap snap = this.GetComponent<ExtensionScrollSnap>();
        snap.Initialized(14);
        snap.OnChangedEvent.AddListener((int n) =>
        {
            Debug.Log($"> changed {n}");
        });
        snap.OnChangeEndEvent.AddListener((int n) => 
        {
            Debug.Log($"> chang End {n}");
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
