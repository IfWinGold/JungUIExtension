using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JungExtension.UI;


namespace JungExtension.TestProj
{
    public enum TestState
    {
        scrollSnap,
        toggleSwitch
    }
    public class Test : MonoBehaviour
    {
        public TestState state;
        [SerializeField] private ExtensionScrollSnap scrollsnap;
        [SerializeField] private ExtensionToggleSwitch toggleSwitch;

        void Start()
        {
            switch (state)
            {
                case TestState.scrollSnap:
                    {
                        if (scrollsnap.gameObject.activeSelf == false)
                            scrollsnap.gameObject.SetActive(true);
                        scrollsnap.Initialized(20);
                    }
                    break;
                case TestState.toggleSwitch:
                    {
                        if (toggleSwitch.gameObject.activeSelf == false)
                            toggleSwitch.gameObject.SetActive(true);
                        toggleSwitch.Initialized(true);
                    }
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                scrollsnap.NextView();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                scrollsnap.NextView();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                scrollsnap.PrevView();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
                scrollsnap.PrevView();
        }
    }
}


