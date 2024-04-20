using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class FrameSampleEntry : MonoBehaviour {

        VFXFrameCore vfxCore;

        [Header("Role And Path")]
        [SerializeField] RoleEntity role;
        [SerializeField] PathEntity path;
        [SerializeField] Transform preSpawnRoot;

        [Header("Navigation UI")]
        [SerializeField] FrameNavigationPanel navigationPanel;

        [Header("VFX Info")]
        [SerializeField] Sprite[] frames;
        [SerializeField] bool isLoop;
        [SerializeField] bool isFlipX;
        [SerializeField] string sortingLayerName;
        [SerializeField] int sortingOrder;
        [SerializeField] float delayEndSec;

        [Header("Easing Out Info")]
        [SerializeField] MortiseFrame.Swing.EasingMode easingOutMode;
        [SerializeField] MortiseFrame.Swing.EasingType easingOutType;
        [SerializeField] float easingOutDuration;

        float frameInterval;
        int preSpawnVFXID;

        void Awake() {
            PLog.Log = Debug.Log;
            PLog.Error = Debug.LogError;
            PLog.Warning = Debug.LogWarning;

            frameInterval = 1f / 12f;

            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            vfxCore = new VFXFrameCore("VFX_Frame", vfxRoot);

            Init();
        }

        void Init() {
            preSpawnVFXID = vfxCore.TryPreSpawnVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, preSpawnRoot.position, isFlipX, sortingLayerName, sortingOrder);
            vfxCore.SetDelayEndSec(preSpawnVFXID, delayEndSec);
            vfxCore.SetFadingOut(preSpawnVFXID, easingOutDuration, easingOutType, easingOutMode);

            path.Ctor();
            path.InitMoveState();

            navigationPanel.Ctor();
            navigationPanel.AddToWorldHandle = OnAddToWorld;
            navigationPanel.AddToTargetHandle = OnAddToTarget;
            navigationPanel.PlayManualyHandle = OnPlayManualy;
            navigationPanel.StopManualyHandle = OnStopManualy;
        }

        void OnAddToWorld() {
            var id = vfxCore.TrySpawnAndPlayVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, role.Pos, isFlipX, sortingLayerName, sortingOrder);
            vfxCore.SetDelayEndSec(id, delayEndSec);
            vfxCore.SetFadingOut(id, easingOutDuration, easingOutType, easingOutMode);
        }

        void OnAddToTarget() {
            var id = vfxCore.TrySpawnAndPlayVFX_ToTarget("VFX_02", frames, isLoop, frameInterval, role.Transform, Vector3.zero, isFlipX, sortingLayerName, sortingOrder);
            vfxCore.SetDelayEndSec(id, delayEndSec);
            vfxCore.SetFadingOut(id, easingOutDuration, easingOutType, easingOutMode);
        }

        void OnPlayManualy() {
            vfxCore.TryRePlayManualy(preSpawnVFXID);
        }

        void OnStopManualy() {
            vfxCore.TryStopManualy(preSpawnVFXID);
        }

        void OnDestroy() {
            vfxCore.TearDown();
        }

        void Update() {

            var dt = Time.deltaTime;

            var pointer = path.TickPointerMove(dt);
            role.Tick(dt, pointer);
            vfxCore.Tick(dt);

        }

    }

}