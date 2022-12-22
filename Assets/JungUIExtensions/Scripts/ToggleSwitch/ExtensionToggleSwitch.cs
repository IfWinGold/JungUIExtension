using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace JungExtension.UI
{
    [AddComponentMenu("JungExtensions/ExtensionToggleSwitch")]
    public class ExtensionToggleSwitch : MonoBehaviour,IPointerClickHandler
    {
        public class ChangedEvent : UnityEvent<bool> { }
        public ChangedEvent m_OnChangedEvent = new ChangedEvent();
        /// <summary>
        //
        /// </summary>
        public ChangedEvent OnChangeEvent { get { return m_OnChangedEvent; } set { m_OnChangedEvent = value; } }


        public enum SettingState
        {
            DefaultSetting,
            ChangeSprite
        }

        public SettingState m_SettingState;
        public bool m_UseChangeColor;
        



        [SerializeField] private Sprite m_backGroundOffSprite;
        public Sprite BackGroundOffSprite { get { return m_backGroundOffSprite; } set { m_backGroundOffSprite = value; } }

        [SerializeField] private Sprite m_backGroundOnSprite;
        public Sprite BackGroundOnSprite { get { return m_backGroundOnSprite; } set { m_backGroundOnSprite = value; } }



        [SerializeField] private Sprite m_handleOffSprite;
        public Sprite HandleOffSprite { get { return m_handleOffSprite; } set { m_handleOffSprite = value; } }

        [SerializeField] private Sprite m_handleOnSprite;
        public Sprite HandleOnSprite { get { return m_handleOnSprite; } set { m_handleOnSprite = value; } }



        [SerializeField] private Color m_handleOffColor;
        public Color HandleOffColor { get { return m_handleOffColor; } set { m_handleOffColor = value; } }

        [SerializeField] private Color m_handleOnColor;
        public Color HandleOnColor { get { return m_handleOnColor; } set { m_handleOnColor = value; } }

        [SerializeField] private Color m_backGroundOffColor;
        public Color BackGroundOffColor { get { return m_backGroundOffColor; } set { m_backGroundOffColor = value; } }

        [SerializeField] private Color m_backGroundOnColor;
        public Color BackGroundOnColor { get { return m_backGroundOnColor; } set { m_backGroundOnColor = value; } }



        [SerializeField] private Ease m_moveEase;
        public Ease MoveEase { get { return m_moveEase; } set { m_moveEase = value; } }



        private bool m_isOn = false;
        public bool IsOn { get { return m_isOn; } set { m_isOn = value; } }

        private RectTransform m_BGRect;

        private RectTransform m_BGImgRect;

        private Image m_BGImg;




        private RectTransform m_switchRect;

        private RectTransform m_handleRect;

        private Image m_handleImg;


        public RectTransform BackGround { get { return m_BGRect; } }

        private Sprite m_defaultBGSprite;
        private Sprite m_defaultHandleSprite;
        private static Color m_defaultHandleColor = new Color(0,0,1);
        private static Color m_defaultBGColor = new Color(1, 1, 1);






        public void Initialized(bool _defaultValue)
        {
            if (!HasChild())
            {
                CreateChilds();
            }                
            else
            {
                m_BGRect = this.transform.Find("BG").GetComponent<RectTransform>();
                m_BGImgRect = m_BGRect.transform.Find("BG_Image").GetComponent<RectTransform>();
                m_BGImg = m_BGImgRect.gameObject.GetComponent<Image>();

                m_switchRect = this.transform.Find("Switch").GetComponent<RectTransform>();
                m_handleRect = m_switchRect.transform.Find("Handle").GetComponent<RectTransform>();
                m_handleImg = m_handleRect.GetComponent<Image>();
            }

            SetDefaultHandle(_defaultValue);
            switch (m_SettingState)
            {
                case SettingState.DefaultSetting:
                    {
                        SetChangeDefaultSetting();
                    }
                    break;
                case SettingState.ChangeSprite:
                    {
                        SetChangeSpriteSetting();
                    }
                    break;
            }            
        }        


        public void CreateChilds()
        {
            CreateBG();
            CreateSwitch();
        }


        private void CreateBG()
        {
            GameObject BG = new GameObject("BG");            
            m_BGRect = BG.AddComponent<RectTransform>();                        
            BG.transform.parent = this.transform;
            m_BGRect.anchorMin = new Vector2(0f, 0f);
            m_BGRect.anchorMax = new Vector2(1f, 1f);
            m_BGRect.offsetMin = new Vector2(0f, 0f);
            m_BGRect.offsetMax = new Vector2(0f, 0f);

            GameObject BG_ImageObj = new GameObject("BG_Image");                   
            m_BGImgRect = BG_ImageObj.AddComponent<RectTransform>();
            BG_ImageObj.transform.parent = BG.transform;
            m_BGImg = BG_ImageObj.AddComponent<Image>();
            m_BGImgRect.anchorMin = new Vector2(0f, 0f);
            m_BGImgRect.anchorMax = new Vector2(1f, 1f);
            m_BGImgRect.offsetMin = new Vector2(0f, 0f);
            m_BGImgRect.offsetMax = new Vector2(0f, 0f);


            switch(m_SettingState)
            {
                case SettingState.DefaultSetting:
                    {
                        SetDefaultBackGround();
                    }
                    break;
                case SettingState.ChangeSprite:
                    {
                        m_BGImg.sprite = m_backGroundOffSprite;
                    }
                    break;
            }
        }


        private void CreateSwitch()
        {
            GameObject Switch = new GameObject("Switch");
            m_switchRect = Switch.AddComponent<RectTransform>();
            Switch.transform.parent = this.transform;
            m_switchRect.anchorMin = new Vector2(0f, 0f);
            m_switchRect.anchorMax = new Vector2(1f, 1f);
            m_switchRect.offsetMin = new Vector2(5f, 5f);
            m_switchRect.offsetMax = new Vector2(-5f, -5f);

            GameObject SwitchCircle = new GameObject("Handle");
            m_handleRect = SwitchCircle.AddComponent<RectTransform>();
            m_handleImg = SwitchCircle.AddComponent<Image>();
            SwitchCircle.transform.parent = Switch.transform;
            m_handleRect.pivot = new Vector2(0f, 0.5f);
            m_handleRect.anchorMin = new Vector2(0f, 0f);
            m_handleRect.anchorMax = new Vector2(0f, 1f);
            m_handleRect.offsetMin = new Vector2(m_handleRect.offsetMin.x, 0f);
            m_handleRect.offsetMax = new Vector2(m_handleRect.offsetMax.x, 0f);
            m_handleRect.anchoredPosition = new Vector2(0f, 0f);
            m_handleRect.sizeDelta = new Vector2(m_handleRect.rect.height, m_handleRect.sizeDelta.y);




            switch (m_SettingState)
            {
                case SettingState.DefaultSetting:
                    {
                        SetDefaultHandle();
                    }
                    break;
                case SettingState.ChangeSprite:
                    {
                        m_handleImg.sprite = m_handleOffSprite;
                    }
                    break;
            }
        }


        


        private void SetDefaultHandle()
        {
            if(m_defaultHandleSprite == null)
            {
                m_defaultHandleSprite = Resources.Load<Sprite>("Default/Circle");
            }
            m_handleImg.sprite = m_defaultHandleSprite;
            if(m_UseChangeColor)
            {
                if (IsOn)
                    m_handleImg.color = m_handleOnColor;
                else
                    m_handleImg.color = m_handleOffColor;
            }
            else
            {
                m_handleImg.color = m_defaultHandleColor;
            }
        }


        private void SetDefaultBackGround()
        {
            if (m_defaultBGSprite == null)
            {
                m_defaultBGSprite = Resources.Load<Sprite>("Default/Btn");
            }
            m_BGImg.sprite = m_defaultBGSprite;
            if(m_UseChangeColor)
            {
                if (IsOn)
                    m_BGImg.color = m_backGroundOnColor;
                else
                    m_BGImg.color = m_backGroundOffColor;
            }
            else
            {
                m_BGImg.color = m_defaultBGColor;
            }            
        }


        public void SetChangeDefaultSetting()
        {
            SetDefaultHandle();
            SetDefaultBackGround();
        }


        public void SetChangeSpriteSetting()
        {
            if (IsOn)
            {
                m_handleImg.sprite = m_handleOnSprite;
                m_BGImg.sprite = m_backGroundOnSprite;

                if (m_UseChangeColor)
                {
                    m_handleImg.color = m_handleOnColor;
                    m_BGImg.color = m_backGroundOnColor;
                }
            }
            else
            {
                m_handleImg.sprite = m_handleOffSprite;
                m_BGImg.sprite = m_backGroundOffSprite;

                if (m_UseChangeColor)
                {
                    m_handleImg.color = m_handleOffColor;
                    m_BGImg.color = m_backGroundOffColor;
                }
            }
        }


        public void SetActiveHandle(bool _active)
        {
            if (_active)
            {
                m_isOn = true;
                SetActiveHandleOn();                                
            }                
            else
            {
                m_isOn = false;
                SetActiveHandleOff();                
            }
            UpdateSetting();
        }


        private void UpdateSetting()
        {
            switch(m_SettingState)
            {
                case SettingState.DefaultSetting:
                    SetChangeDefaultSetting();
                    break;
                case SettingState.ChangeSprite:
                    SetChangeSpriteSetting();
                    break;
            }
        }

        private void SetActiveHandleOn()
        {
            //m_handleRect.do
            float switchWidth = m_switchRect.rect.width;
            float handleWidth = m_handleRect.rect.width;
            float handleY = m_handleRect.anchoredPosition.y;
            Vector2 target = new Vector2(switchWidth - handleWidth, handleY);
            m_handleRect.DOAnchorPos(target, 0.5f);
        }


        private void SetActiveHandleOff()
        {            
            float handleY = m_handleRect.anchoredPosition.y;
            Vector2 target = new Vector2(0, handleY);
            m_handleRect.DOAnchorPos(target, 0.5f);
        }


        private void SetDefaultHandle(bool _active)
        {
            if(_active)
            {
                float switchWidth = m_switchRect.rect.width;
                float handleWidth = m_handleRect.rect.width;
                float handleY = m_handleRect.anchoredPosition.y;
                Vector2 target = new Vector2(switchWidth - handleWidth, handleY);
                m_handleRect.DOAnchorPos(target, 0f);                
            }
            else
            {
                float handleY = m_handleRect.anchoredPosition.y;
                Vector2 target = new Vector2(0, handleY);
                m_handleRect.DOAnchorPos(target, 0f);
            }
            m_isOn = _active;
        }





        public void OnClickDebugButton()
        {

        }

        /// <summary>
        /// BG,Switch 자식 오브젝트를 가지고 있는가?
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "BG" || transform.GetChild(i).name == "Switch")
                    return true;
            }
            return false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(IsOn)
            {
                m_isOn = false;                
            }
            else
            {
                m_isOn = true;
            }
            SetActiveHandle(m_isOn);
            m_OnChangedEvent.Invoke(m_isOn);
        }
    }
}
