using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenonKit.Prism.Sample {

    public class ParticelNavigationPanel : MonoBehaviour {

        [SerializeField] Button btn_addToWorld;
        [SerializeField] Button btn_addToTarget;
        [SerializeField] Button btn_playManualy;
        [SerializeField] Button btn_stopManualy;

        public Action AddToWorldHandle;
        public Action AddToTargetHandle;
        public Action PlayManualyHandle;
        public Action StopManualyHandle;

        public void Ctor() {
            btn_addToWorld.onClick.AddListener(() => {
                AddToWorldHandle?.Invoke();
            });
            btn_addToTarget.onClick.AddListener(() => {
                AddToTargetHandle?.Invoke();
            });
            btn_playManualy.onClick.AddListener(() => {
                PlayManualyHandle?.Invoke();
            });
            btn_stopManualy.onClick.AddListener(() => {
                StopManualyHandle?.Invoke();
            });
        }

        public void TearDown() {
            btn_addToWorld.onClick.RemoveAllListeners();
            btn_addToTarget.onClick.RemoveAllListeners();
            btn_playManualy.onClick.RemoveAllListeners();
            btn_stopManualy.onClick.RemoveAllListeners();
            AddToWorldHandle = null;
            AddToTargetHandle = null;
            PlayManualyHandle = null;
            StopManualyHandle = null;
            GameObject.Destroy(gameObject);
        }


    }

}