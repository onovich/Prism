using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class FrameSampleEntry : MonoBehaviour {

        VFXFrameCore vfxCore;
        bool isInit = false;

        [SerializeField] RoleEntity role;
        [SerializeField] PathEntity path;
        [SerializeField] Transform preSpawnRoot;
        [SerializeField] FrameNavigationPanel navigationPanel;

        [SerializeField] Sprite[] frames;
        [SerializeField] bool isLoop;
        [SerializeField] bool isFlipX;
        [SerializeField] string sortingLayerName;
        [SerializeField] int sortingOrder;
        float frameInterval;

        int preSpawnVFXID;

        void Awake() {
            PLog.Log = Debug.Log;
            PLog.Error = Debug.LogError;
            PLog.Warning = Debug.LogWarning;

            frameInterval = 1f / 12f;

            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            vfxCore = new VFXFrameCore("VFX_Frame", vfxRoot);

            Action main = async () => {
                await vfxCore.LoadAssets();
                Init();
                isInit = true;
            };
            main.Invoke();
        }

        void Init() {
            preSpawnVFXID = vfxCore.TryPreSpawnVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, preSpawnRoot.position, isFlipX, sortingLayerName, sortingOrder);

            path.Ctor();
            path.InitMoveState();

            navigationPanel.Ctor();
            navigationPanel.AddToWorldHandle = OnAddToWorld;
            navigationPanel.AddToTargetHandle = OnAddToTarget;
            navigationPanel.PlayManualyHandle = OnPlayManualy;
            navigationPanel.StopManualyHandle = OnStopManualy;
        }

        void OnAddToWorld() {
            vfxCore.TrySpawnAndPlayVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, role.Pos, isFlipX, sortingLayerName, sortingOrder);
        }

        void OnAddToTarget() {
            vfxCore.TrySpawnAndPlayVFX_ToTarget("VFX_02", frames, isLoop, frameInterval, role.Transform, Vector3.zero, isFlipX, sortingLayerName, sortingOrder);
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

            if (!isInit) {
                return;
            }
            var dt = Time.deltaTime;

            var pointer = path.TickPointerMove(dt);
            role.Tick(dt, pointer);
            vfxCore.Tick(dt);

        }

    }

}