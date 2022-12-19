using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JungExtension.UI;
public class ExtensionScrollSnapCellViewDemo : ExtensionScrollSnapCellView
{
    [SerializeField] private Text indexText;
    public override void SetData(int _index)
    {
        indexText.text = _index.ToString();
    }
}
