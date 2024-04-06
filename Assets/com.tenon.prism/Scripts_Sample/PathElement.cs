using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class PathElement : MonoBehaviour {

        public int index;
        public Vector2 Pos => transform.position;

        public void Rename(int count) {
            if (index == 0) {
                this.name = $"Path - START - {index}";
            } else if (index == count - 1) {
                this.name = $"Path - END - {index}";
            } else {
                this.name = $"Path - Node - {index}";
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(Pos, 0.1f * Vector3.one);
        }

    }

}