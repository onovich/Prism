using System;
using System.Drawing;
using System.Reflection;
using UnityEngine;

namespace TenonKit.Prism.Sample {

    public class RoleEntity : MonoBehaviour {

        public void Tick(float dt, Vector2 pos) {
            var pathPointer = pos;
            this.transform.position = pathPointer;
        }

    }

}