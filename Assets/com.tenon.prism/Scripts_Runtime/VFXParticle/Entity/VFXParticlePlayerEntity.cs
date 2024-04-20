using UnityEngine;

namespace TenonKit.Prism {

    internal class VFXParticlePlayerEntity {

        int vfxID;
        internal int VFXID => vfxID;

        VFXParticleState state;
        internal VFXParticleState State => state;
        internal void SetState(VFXParticleState value) => state = value;

        ParticleSystem rootParticle;
        ParticleSystem[] allParticle;

        internal bool IsLoop => rootParticle.main.loop;
        internal bool IsPlaying_Root => rootParticle.isPlaying;

        string vfxName;
        internal string VFXName => vfxName;

        float maintainSec;
        internal float MaintainSec => maintainSec;

        float currentSec;
        internal float CurrentSec => currentSec;
        internal void SpendCurrentSec(float dt) => currentSec += dt;
        internal void ResetCurrentSec() => currentSec = 0;

        GameObject vfxGO;
        internal GameObject VFXGO => vfxGO;

        bool hasAttachTarget;
        internal bool HasAttachTarget => hasAttachTarget;
        internal void SetHasAttachTarget(bool value) => hasAttachTarget = value;

        Transform attachTarget;
        internal Transform AttachTarget => attachTarget;
        internal void SetAttachTarget(Transform value) => attachTarget = value;

        Vector3 offset;
        internal Vector3 Offset => offset;
        internal void SetOffset(Vector3 value) => offset = value;

        internal VFXParticlePlayerEntity() { }

        internal void Init(string vfxName, int id, float maintainSec, VFXParticleState state) {
            this.vfxName = vfxName;
            this.vfxID = id;
            this.maintainSec = maintainSec;
            this.state = state;
        }

        internal void TearDown() {
            GameObject.Destroy(vfxGO);
        }

        internal void Reset() {
            vfxGO = null;
            currentSec = 0;
        }

        internal void Play() {
            if (allParticle != null) {
                foreach (var ps in allParticle) {
                    if (ps == null) continue;
                    ps.Play();
                }
            }
            vfxGO.SetActive(true);
        }

        internal void StopAll() {
            if (allParticle != null) {
                foreach (var ps in allParticle) {
                    if (ps == null) continue;
                    ps.Stop();
                }
            }
            vfxGO.SetActive(false);
        }

        internal void SetParent(Transform parent, bool isWorldPosStays = false) {
            vfxGO.transform.SetParent(parent, isWorldPosStays);
        }

        internal void SetVFXGO(GameObject vfxGo) {
            this.vfxGO = vfxGo;
            this.rootParticle = vfxGo.GetComponentInChildren<ParticleSystem>();
            this.allParticle = vfxGo.GetComponentsInChildren<ParticleSystem>();
        }

    }

}