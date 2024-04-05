using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class SampleEntry : MonoBehaviour {

        VFXCore vfxCore;
        bool isInit = false;

        void Awake() {

            vfxCore = new VFXCore("VFX");

            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            vfxCore.Inject(vfxRoot);

            Action main = async () => {
                await vfxCore.LoadAssets();
                isInit = true;

            };

        }

        void OnDestroy() {
            vfxCore.TearDown();
        }

        void Update() {

            if (!isInit) {
                return;
            }

            vfxCore.Tick(Time.deltaTime);

        }

    }

}