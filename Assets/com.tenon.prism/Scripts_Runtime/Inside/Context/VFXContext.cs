using System;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Prism {

    internal class VFXContext {

        // Service
        VFXIDService vfxIDService;
        internal VFXIDService VFXIDService => vfxIDService;

        // Repo
        VFXRepo repo;
        internal VFXRepo Repo => repo;

        // Root
        Transform vfxRoot;
        internal Transform VFXRoot => vfxRoot;

        // Prefab
        internal Dictionary<string, GameObject> prefabDict;

        // Const
        internal string AssetsLabel;

        internal VFXContext() {
            repo = new VFXRepo();
            vfxIDService = new VFXIDService();
            prefabDict = new Dictionary<string, GameObject>();
        }

        internal void Inject(Transform vfxRoot) {
            this.vfxRoot = vfxRoot;
        }

        internal void Asset_AddPrefab(string name, GameObject prefab) {
            prefabDict.Add(name, prefab);
        }

        internal GameObject GetVFXAssetOrDefault(string name) {
            bool has = prefabDict.TryGetValue(name, out GameObject go);
            if (has) {
                return go;
            }
            PLog.Error($"VFXAssets 找不到 {name}");
            return null;
        }

        internal void ClearAll() {
            vfxIDService.Reset();
            repo.Clear();
            prefabDict.Clear();
        }
    }

}