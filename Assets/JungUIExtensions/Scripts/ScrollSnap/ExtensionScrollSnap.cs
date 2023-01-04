using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace JungExtension.UI
{   
    
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("JungExtensions/ExtensionScrollSnap")]
    public class ExtensionScrollSnap : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {                  
        public class ChangedEvent : UnityEvent<int> { }
        public class ChangeEndEvent : UnityEvent<int> { }
        
        private ChangedEvent m_OnchangedEvent = new ChangedEvent();
        public ChangedEvent OnChangedEvent { get { return m_OnchangedEvent; } set { m_OnchangedEvent = value; } }


        private ChangeEndEvent m_OnchangeEndEvent = new ChangeEndEvent();
        public ChangeEndEvent OnChangeEndEvent { get { return m_OnchangeEndEvent; } set { m_OnchangeEndEvent = value; } }



        [SerializeField] private ExtensionScrollSnapCellView m_prefab;

        [SerializeField] private int m_startIndex = 0;

        [SerializeField] private float m_spacing;

        [SerializeField] private float m_moveDuration;

        [SerializeField] private float m_stopVelocity;

        [SerializeField] private Ease m_moveEase;

        [SerializeField] private bool m_UseHardSwipe = false;

        [SerializeField] private bool m_UseDragged = true;


        private int m_currentIndex;

        private bool m_isMove = false;        
        private bool m_isDrag = false;        


        private ScrollRect m_scrollRect;                

        private List<ExtensionScrollSnapCellView> m_ViewList = new List<ExtensionScrollSnapCellView>();        

        private RectTransform m_snapRect;

        //private HorizontalLayoutGroup hLayout;





        private Vector2 m_dragDir = Vector2.zero;

        private Vector2 m_BeginDrag = Vector2.zero;

        private Vector2 m_EndDrag = Vector2.zero;


        public RectTransform Container { get { return m_scrollRect.content; } }         
        public int CurrentIndex { get { return m_currentIndex; } }
        public GameObject CurrentIndexObject { get { return m_ViewList[m_currentIndex].gameObject; } }
        public List<ExtensionScrollSnapCellView> ViewList { get { return m_ViewList; } }
        public float HalfWidth { get { return m_snapRect.sizeDelta.x / 2; } }                


        
        private void Awake()
        {            
            m_scrollRect = this.GetComponent<ScrollRect>();

            m_snapRect = this.GetComponent<RectTransform>();                        

            if (m_scrollRect.content == null)
                m_scrollRect.content = CreateContainer();               

            if(m_snapRect.sizeDelta.x == 0 || m_snapRect.sizeDelta.y == 0)
            {
                Debug.LogError("[ExtensionScrollSnap]Width 또는 Height가 0입니다. 만약 앵커가 stretch 상태라면 stretch를 해제해주세요. stretch 상태라면 width 또는 height를 알 수 없습니다.");
            }
        }        
        public void Initialized(int _count)
        {
            if (m_UseDragged)
                m_scrollRect.horizontal = true;
            else
                m_scrollRect.horizontal = false;

            m_scrollRect.vertical = false;
            if (m_UseHardSwipe) m_scrollRect.inertia = false;
            m_scrollRect.elasticity = 0.1f;
            m_scrollRect.decelerationRate = 0.135f;
            m_currentIndex = m_startIndex;
            CreateCellView(_count);
            m_scrollRect.horizontalNormalizedPosition = GetHorizontalNormalizedPosition(m_startIndex);
        }
        public void Initialized(int _count,int _startIndex)
        {            
            if (m_UseDragged)
                m_scrollRect.horizontal = true;
            else
                m_scrollRect.horizontal = false;

            m_scrollRect.vertical = false;
            if (m_UseHardSwipe) m_scrollRect.inertia = false;
            m_scrollRect.elasticity = 0.1f;
            m_scrollRect.decelerationRate = 0.135f;
            m_startIndex = _startIndex;
            m_currentIndex = m_startIndex;
            CreateCellView(_count);
            m_scrollRect.horizontalNormalizedPosition = GetHorizontalNormalizedPosition(m_startIndex);
        }



        private void CreateCellView(int _count)
        {
            List<float> posList_X = new List<float>();

            float posX = 0f;            
            for (int i = 0; i < _count; i++)
            {
                ExtensionScrollSnapCellView obj = Instantiate(m_prefab, Container);
                obj.SetData(i);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(m_snapRect.sizeDelta.x, m_snapRect.sizeDelta.y);                                     
                rect.anchorMin = new Vector2(0f, 0.5f);
                rect.anchorMax = new Vector2(0f, 0.5f);

                float pos = posX + HalfWidth;                
                rect.anchoredPosition = new Vector2(pos, rect.localPosition.y);
                
                posX += m_snapRect.sizeDelta.x+m_spacing;                
                
                posList_X.Add(m_snapRect.sizeDelta.x);
                m_ViewList.Add(obj);
            }            
            float containerWidth = (m_snapRect.sizeDelta.x * (_count - 1)) + (m_spacing * (_count-1));            
            Container.offsetMax = new Vector2(containerWidth, Container.offsetMax.y);            
        }



        private RectTransform CreateContainer()
        {
            GameObject container = new GameObject("Container");
            RectTransform containerRect = container.AddComponent<RectTransform>();
            container.transform.parent = this.transform;
            containerRect.anchorMin = new Vector2(0f, 0f);
            containerRect.anchorMax = new Vector2(1f, 1f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;
            return containerRect;
        }
        


        private int GetNearestPageIndex()
        {
            int current = m_currentIndex;
            float scrollPosition = m_scrollRect.horizontalNormalizedPosition;
            if (current == 0)
            {
                //비교대상 인덱스
                int next = m_currentIndex + 1;//1번 인덱스
                float currentPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(current));                
                float nextPosition =Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(next));
                                
                return currentPosition < nextPosition ? current : next;
            }
            else if(current == m_ViewList.Count-1) 
            {
                int prev = m_currentIndex - 1;
                float currentPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(current));
                float prevPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(prev));

                return currentPosition < prevPosition ? current : prev;
            }
            else 
            {
                int prev = m_currentIndex - 1;
                int next = m_currentIndex + 1;

                float prevPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(prev));
                float currentPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(current));
                float nextPosition = Mathf.Abs(scrollPosition - GetHorizontalNormalizedPosition(next));

                int[] minIndex = { prev, current, next };
                float[] minValue = { prevPosition, currentPosition, nextPosition };

                float f_min = minValue[0];
                int i_min = 0;
                for(int i =0; i<3;i++)
                {
                    if(f_min>minValue[i])
                    {
                        f_min = minValue[i];
                        i_min = i;
                    }
                }
                return minIndex[i_min];
            }

        }




        #region DragInterface
        float m_minClamp;
        float m_maxClamp;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_UseDragged) 
                return;

            m_isDrag = true;
            m_BeginDrag = eventData.position;
            //m_OnchangeStartEvent.Invoke();   
            if(m_UseHardSwipe)
            {
                float np = m_scrollRect.horizontalNormalizedPosition;
                m_minClamp = np - 0.05f;
                m_maxClamp = np + 0.05f;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!m_UseDragged)
                return;
            if (m_UseHardSwipe)
            {
                float value = Mathf.Clamp(m_scrollRect.horizontalNormalizedPosition, m_minClamp,m_maxClamp);
                m_scrollRect.horizontalNormalizedPosition = value;                
            }            
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!m_UseDragged)
                return;
            m_EndDrag = eventData.position;            
            m_dragDir = (m_EndDrag - m_BeginDrag).normalized;            
            if(m_UseHardSwipe)
            {
                float dragDis = Vector2.Distance(m_EndDrag, m_BeginDrag);                
                if (dragDis < 200)
                    CurrentView();
                else
                {
                    if (m_dragDir.x < -0.5f) //Next
                        NextView();
                    else if (m_dragDir.x > 0.5f)
                        PrevView();

                }
            }
            else
            {
                m_isMove = true;
            }

            m_BeginDrag = Vector2.zero;
            m_EndDrag = Vector2.zero;
            m_dragDir = Vector2.zero;            
            m_isDrag = false;
        }
        #endregion





        #region Move
        public void NextView()
        {
            if (m_currentIndex + 2 >m_ViewList.Count)
                return;
            OnChangedEvent.Invoke(m_currentIndex + 1);
            MovePage(m_currentIndex+1);            
        }



        public void PrevView()
        {
            if (0 > m_currentIndex - 1)
                return;
            OnChangedEvent.Invoke(m_currentIndex -1);
            MovePage(m_currentIndex-1);
        }



        public void CurrentView()
        {
            MovePage(m_currentIndex);
        }



        private void MovePage(int _page)
        {
            float value = GetHorizontalNormalizedPosition(_page);            
            DOTween.To(() => m_scrollRect.horizontalNormalizedPosition, x => m_scrollRect.horizontalNormalizedPosition = x, value, m_moveDuration).SetEase(m_moveEase).OnStart(()=> 
            {                
                m_scrollRect.velocity = Vector2.zero;
                m_currentIndex = _page;
            }).OnComplete(()=> 
            {
                m_OnchangeEndEvent.Invoke(m_currentIndex);
            });            
        }



        private float GetHorizontalNormalizedPosition(int _index)
        {
            float containerWidth = Container.sizeDelta.x;
            float pageX = m_ViewList[_index].GetComponent<RectTransform>().anchoredPosition.x;
            float targetX = pageX - HalfWidth;
            float normalValue = targetX / containerWidth;
            return normalValue;
        }
        #endregion
                
        private void UpdatePage()
        {
            float velX = Mathf.Abs(m_scrollRect.velocity.x);

            if(m_UseHardSwipe)
            {
                if (m_scrollRect.inertia)
                    m_scrollRect.inertia = false;
            }
            else
            {
                if (m_isMove && velX < m_stopVelocity)
                {
                    CurrentView();
                    m_isMove = false;
                }
                int current = m_currentIndex;

                int change = GetNearestPageIndex();
                if (current != change)
                {
                    m_currentIndex = change;
                    m_OnchangedEvent.Invoke(m_currentIndex);
                }
            }            
        }
        private void Update()
        {            
            UpdatePage();                      
        }        
    }
}

