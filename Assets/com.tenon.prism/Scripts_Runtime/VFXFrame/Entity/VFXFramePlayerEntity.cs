using UnityEngine;

namespace TenonKit.Prism {

    internal class VFXFramePlayerEntity {

        // Base Info
        internal int vfxID;
        internal string vfxName;

        // State
        internal VFXFrameState state;
        bool isFlipX;

        // Frames
        Sprite[] allFrame;
        int currentFrameIndex;

        // Attr
        internal bool isLoop;
        float frameInterval;

        // Timer
        float timer;

        // Render
        SpriteRenderer sr;

        // Attach
        internal bool hasAttachTarget;
        internal Transform attachTarget;

        // GameObject
        internal GameObject go;

        // Pos
        internal Vector3 offset;

        internal VFXFramePlayerEntity() { }

        internal void Init(string vfxName, int id, Sprite[] frames, bool isLoop, float frameInterval, VFXFrameState state) {
            this.vfxName = vfxName;
            this.vfxID = id;
            this.allFrame = frames;
            this.isLoop = isLoop;
            this.frameInterval = frameInterval;
            this.state = state;
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
                sr.sprite = allFrame[currentFrameIndex];
                return;
            }

            if (isLoop) {
                currentFrameIndex = 0;
                return;
            }
            state = VFXFrameState.End;
        }

        internal void Stop() {
            state = VFXFrameState.Stop;
        }

        internal void SetParent(Transform parent, bool isWorldPosStays = false) {
            go.transform.SetParent(parent, isWorldPosStays);
        }

    }

}