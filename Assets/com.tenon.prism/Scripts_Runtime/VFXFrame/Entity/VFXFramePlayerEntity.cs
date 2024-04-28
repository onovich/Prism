using MortiseFrame.Swing;
using UnityEngine;
using UnityEngine.UIElements;

namespace TenonKit.Prism {

    internal class VFXFramePlayerEntity {

        // Base Info
        internal int vfxID;
        internal string vfxName;

        // State
        VFXFrameState state;
        public VFXFrameState State => state;
        bool isPreEnd;

        // Frames
        Sprite[] allFrame;
        int currentFrameIndex;

        // Attr
        internal bool isLoop;
        float frameInterval;
        bool isManural;
        float delayEndSec;
        bool isFlipX;

        // Fading Out
        MortiseFrame.Swing.EasingMode easingOutMode;
        EasingType easingOutType;
        float easingOutDuration;

        // Timer
        float timer;
        float delayEndTimer;
        float fadingOutTimer;

        // Attach
        internal bool hasAttachTarget;
        internal Transform attachTarget;

        // High Level
        internal GameObject go;
        internal SpriteRenderer spr;

        // Pos
        internal Vector3 offset;

        bool isInitDone;

        internal VFXFramePlayerEntity() {
            isInitDone = false;
        }

        internal void Init(GameObject go,
                           string vfxName,
                           int id,
                           Sprite[] frames,
                           bool isManural,
                           bool isFlipX,
                           bool isLoop,
                           float frameInterval,
                           VFXFrameState state,
                           string sortingLayerName,
                           int sortingOrder) {
            this.vfxName = vfxName;
            this.vfxID = id;
            this.allFrame = frames;
            this.currentFrameIndex = 0;
            this.isManural = isManural;
            this.isLoop = isLoop;
            this.frameInterval = frameInterval;
            this.state = state;
            timer = frameInterval;
            this.go = go;
            this.spr = go.AddComponent<SpriteRenderer>();
            SetFlipX(isFlipX);
            spr.sortingLayerName = sortingLayerName;
            spr.sortingOrder = sortingOrder;

            isPreEnd = false;
            delayEndTimer = 0;
            fadingOutTimer = 0;
            EnableSpr();

            isInitDone = true;
        }

        internal void TearDown() {
            GameObject.Destroy(go);
        }

        internal void TickPlay(float dt) {
            if (!isInitDone) {
                return;
            }
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

            isPreEnd = true;

        }

        internal void TickEnd(float dt) {
            if (!isInitDone) {
                return;
            }
            if (!isPreEnd) {
                return;
            }

            delayEndTimer += dt;
            if (delayEndTimer < delayEndSec) {
                return;
            }

            fadingOutTimer += dt;
            if (fadingOutTimer < easingOutDuration) {
                var a = EasingHelper.Easing(1, 0, fadingOutTimer, easingOutDuration, easingOutType, easingOutMode);
                var color = spr.color;
                color.a = a;
                spr.color = color;
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
            var color = spr.color;
            color.a = 1;
            spr.color = color;
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
            isPreEnd = false;
            timer = 0;
            delayEndTimer = 0;
            fadingOutTimer = 0;
            EnableSpr();
        }

        internal void SetParent(Transform parent, bool isWorldPosStays = false) {
            go.transform.SetParent(parent, isWorldPosStays);
        }

        internal void SetFlipX(bool isFlipX) {
            this.isFlipX = isFlipX;
            spr.flipX = isFlipX;
        }

        internal void SetDelayEndSec(float sec) {
            delayEndSec = sec;
        }

        internal void SetFadingOut(float duration, EasingType type, MortiseFrame.Swing.EasingMode mode) {
            easingOutDuration = duration;
            easingOutType = type;
            easingOutMode = mode;
        }

    }

}