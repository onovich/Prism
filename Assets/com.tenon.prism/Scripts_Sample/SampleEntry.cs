using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class SampleEntry : MonoBehaviour {

        VFXCore vfxCore;
        bool isInit = false;

        [SerializeField] RoleEntity role;
        [SerializeField] PathEntity path;
        [SerializeField] Transform preSpawnRoot;
        [SerializeField] NavigationPanel navigationPanel;

        int preSpawnVFXID;

        void Awake() {
            PLog.Log = Debug.Log;
            PLog.Error = Debug.LogError;
            PLog.Warning = Debug.LogWarning;

            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            vfxCore = new VFXCore("VFX", vfxRoot);

            Action main = async () => {
                await vfxCore.LoadAssets();
                Init();
                isInit = true;
            };
            main.Invoke();
        }

        void Init() {
            preSpawnVFXID = vfxCore.TryPreSpawnVFX_ToWorldPos("VFX_01", 3f, preSpawnRoot.position);

            path.Ctor();
            path.InitMoveState();

            navigationPanel.Ctor();
            navigationPanel.AddToWorldHandle = OnAddToWorld;
            navigationPanel.AddToTargetHandle = OnAddToTarget;
            navigationPanel.PlayManualyHandle = OnPlayManualy;
            navigationPanel.StopManualyHandle = OnStopManualy;
        }

        void OnAddToWorld() {
            vfxCore.TrySpawnAndPlayVFX_ToWorldPos("VFX_01", 3f, role.Pos);
        }

        void OnAddToTarget() {
            vfxCore.TrySpawnAndPlayVFX_ToTarget("VFX_01", 3f, role.Transform, Vector3.zero);
        }

        void OnPlayManualy() {
            vfxCore.TryPlayManualy(preSpawnVFXID);
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