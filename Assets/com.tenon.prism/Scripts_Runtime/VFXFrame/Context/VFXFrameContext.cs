using System;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Prism {

    internal class VFXFrameContext {

        // Service
        VFXFrameIDService vfxIDService;
        internal VFXFrameIDService VFXIDService => vfxIDService;

        // Repo
        VFXFrameRepo repo;
        internal VFXFrameRepo Repo => repo;

        // Root
        Transform vfxRoot;
        internal Transform VFXRoot => vfxRoot;

        // Const
        internal string assetsLabel;

        internal VFXFrameContext() {
            repo = new VFXFrameRepo();
            vfxIDService = new VFXFrameIDService();
        }

        internal void Inject(Transform vfxRoot) {
            this.vfxRoot = vfxRoot;
        }

        internal void ClearAll() {
            vfxIDService.Reset();
            repo.Clear();
        }
    }

}