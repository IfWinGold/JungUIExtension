using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JungExtension.UI
{
    public enum MessageBoxState
    {
        Ok,
        OkCancel
    }
    public class MessageBox
    {
        public const float DEFAULT_WIDTH = 800f;
        public const float DEFAULT_HEIGHT = 800f;
        public const string DEFAULT_TITLE = "Message";
        public const Ease DEFAULT_EASE_IN = Ease.Linear;
        public const Ease DEFAULT_EASE_OUT = Ease.Linear;        
        public static void Show(Transform _parent,string _message,string _title,float _width,float _height,Ease _in,Ease _out, float _duration,Action _onExitButtonClick)
        {            
            Addressables.InstantiateAsync("MessageBox", _parent).Completed += (handle) =>
            {
                if(handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject messageObj = handle.Result;
                    RectTransform rect = messageObj.GetComponent<RectTransform>();
                    rect.sizeDelta = Vector2.zero;

                    Vector2 sizeTarget = new Vector2(_width, _height);

                    DOTween.To(() => rect.sizeDelta, x => rect.sizeDelta = x, sizeTarget, _duration).SetEase(_in);                    
                    

                    TextMeshProUGUI Title = messageObj.transform.Find("tmp_Title").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI Message = messageObj.transform.Find("tmp_Message").GetComponent<TextMeshProUGUI>();                    

                    Title.text = _title;
                    Message.text = _message;


                }
            };
        }
    }
}

