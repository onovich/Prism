using UnityEngine;

namespace TenonKit.Prism {

    internal class VFXFramePlayerEntity {

        // Base Info
        internal int vfxID;
        internal string vfxName;

        // State
        VFXFrameState state;
        public VFXFrameState State => state;
        bool isFlipX;

        // Frames
        Sprite[] allFrame;
        int currentFrameIndex;

        // Attr
        internal bool isLoop;
        float frameInterval;
        bool isManural;

        // Timer
        float timer;

        // Attach
        internal bool hasAttachTarget;
        internal Transform attachTarget;

        // High Level
        internal GameObject go;
        internal SpriteRenderer spr;

        // Pos
        internal Vector3 offset;

        internal VFXFramePlayerEntity() { }

        internal void Init(string vfxName, int id, Sprite[] frames, bool isManural, bool isLoop, float frameInterval, VFXFrameState state) {
            this.vfxName = vfxName;
            this.vfxID = id;
            this.allFrame = frames;
            this.currentFrameIndex = 0;
            this.isManural = isManural;
            this.isLoop = isLoop;
            this.frameInterval = frameInterval;
            this.state = state;
            timer = 0;
        }

        internal void TearDown() {
            GameObject.Destroy(go);
        }

        internal void TickPlay(float dt) {
            if (state != VFXFrameState.Playing) {
                return;
            }

            timer += dt;
            if (timer < frameInterval) {
                return;
            }
            timer -= frameInterval;

            currentFrameIndex++;
            if (currentFrameIndex < allFrame.Length) {
                spr.sprite = allFrame[currentFrameIndex];
                return;
            }

            if (isLoop) {
                currentFrameIndex = 0;
                return;
            }

            if (isManural) {
                Stop();
            } else {
                End();
            }
        }

        void EnableSpr() {
            spr.enabled = true;
        }

        void DisableSpr() {
            spr.enabled = false;
        }

        internal void Stop() {
            state = VFXFrameState.Stop;
            DisableSpr();
        }

        internal void End() {
            state = VFXFrameState.End;
            DisableSpr();
        }

        internal void Play() {
            state = VFXFrameState.Playing;
            EnableSpr();
        }

        internal void RePlay() {
            state = VFXFrameState.Playing;
            currentFrameIndex = 0;
            EnableSpr();
        }

        internal void SetParent(Transform parent, bool isWorldPosStays = false) {
            go.transform.SetParent(parent, isWorldPosStays);
        }

    }

}