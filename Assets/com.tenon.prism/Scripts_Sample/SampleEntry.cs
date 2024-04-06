using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class SampleEntry : MonoBehaviour {

        VFXCore vfxCore;
        bool isInit = false;

        [SerializeField] RoleEntity role;
        [SerializeField] PathEntity path;

        void Awake() {

            vfxCore = new VFXCore("VFX");

            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            vfxCore.Inject(vfxRoot);

            // Action main = async () => {
            //     await vfxCore.LoadAssets();
            //     isInit = true;
            // };
            isInit = true;

            PLog.Log = Debug.Log;
            PLog.Error = Debug.LogError;
            PLog.Warning = Debug.LogWarning;

            path.Ctor();
            path.InitMoveState();
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