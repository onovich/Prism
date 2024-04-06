using System;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class PathEntity : MonoBehaviour {

        [SerializeField] PathElement[] elements;
        [SerializeField] bool isLoop;
        [SerializeField] float speed = 1f;

        int currentIndex;
        int nextIndex;
        Vector2 pointer;
        public Vector2 Pointer => pointer;
        int direction;
        float durationSec;
        float currentSec;

        public void Ctor() {
            for (int i = 0; i < elements.Length; i++) {
                var element = elements[i];
                element.Rename(elements.Length);
            }
            direction = 1;
        }

        public void InitMoveState() {
            currentIndex = 0;
            if (elements.Length > 2) {
                nextIndex = 1;
            }
            pointer = elements[0].Pos;
            currentSec = 0;
            var dis = Vector2.Distance(elements[currentIndex].Pos, elements[nextIndex].Pos);
            durationSec = dis / speed;
        }

        public Vector2 TickPointerMove(float dt) {
            if (currentSec >= durationSec) {
                ArriveTarget();
            }
            var startPos = elements[currentIndex].Pos;
            var endPos = elements[nextIndex].Pos;
            currentSec += dt;
            var currentPos = Vector2.Lerp(startPos, endPos, currentSec / durationSec);
            pointer = currentPos;
            return pointer;
        }

        void ArriveTarget() {
            currentIndex = nextIndex;
            if (currentIndex >= elements.Length - 1) {
                if (isLoop) {
                    nextIndex = 0;
                } else {
                    direction *= -1;
                    nextIndex = currentIndex + 1 * direction;
                }
            } else {
                nextIndex = currentIndex + 1 * direction;
            }

            currentSec = 0;
            var dis = Vector2.Distance(elements[currentIndex].Pos, elements[nextIndex].Pos);
            durationSec = dis / speed;
        }

        [ContextMenu("GetNodes")]
        void GetNodes() {
            elements = transform.GetComponentsInChildren<PathElement>();
            PLog.Log($"Get {elements.Length} Nodes.");
        }

        void OnDrawGizmos() {
            if (elements == null) {
                return;
            }
            for (int i = 0; i < elements.Length; i++) {
                var current = elements[i];
                PathElement next = null;
                if (i == elements.Length - 1) {
                    if (isLoop) {
                        next = elements[0];
                    }
                } else {
                    next = elements[i + 1];
                }
                if (next == null) {
                    return;
                }
                Gizmos.color = Color.green;
                Gizmos.DrawLine(current.Pos, next.Pos);
            }
        }

    }

}