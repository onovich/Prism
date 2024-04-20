using System;
using System.Collections.Generic;
using System.Linq;

namespace TenonKit.Prism {

    internal class VFXFrameRepo {

        Dictionary<int, VFXFramePlayerEntity> all;

        internal VFXFrameRepo() {
            this.all = new Dictionary<int, VFXFramePlayerEntity>();
        }

        internal void Add(VFXFramePlayerEntity entity) {
            all[entity.vfxID] = entity;
        }

        internal void Foreach(Action<int, VFXFramePlayerEntity> action) {
            var e = all.GetEnumerator();
            while (e.MoveNext()) {
                var kvp = e.Current;
                action(kvp.Key, kvp.Value);
            }
        }

        internal void Remove(int id) {
            all.Remove(id);
        }

        internal void RemoveAll(Predicate<VFXFramePlayerEntity> condition) {
            var allValues = all.Values.ToArray();
            for (int i = allValues.Length - 1; i >= 0; i--) {
                var vfx = allValues[i];
                if (condition(vfx)) {
                    vfx.TearDown();
                    all.Remove(vfx.vfxID);
                }
            }
        }

        internal bool TryGet(int id, out VFXFramePlayerEntity entity) {
            var has = all.TryGetValue(id, out entity);
            return has;
        }

        internal void Clear() {
            var allValues = all.Values;
            for (int i = allValues.Count - 1; i >= 0; i--) {
                var vfx = allValues.ElementAt(i);
                vfx.TearDown();
            }
            all.Clear();
        }

    }

}