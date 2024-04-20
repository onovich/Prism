using UnityEngine;

namespace TenonKit.Prism {

    internal static class VFXFrameFactory {

        internal static VFXFramePlayerEntity SpawnVFXPlayer(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isManural, bool isLoop, float frameInterval, VFXFrameState state) {
            var entity = new VFXFramePlayerEntity();
            var id = ctx.VFXIDService.PickVFXID();
            entity.Init(vfxName, id, frames, isManural, isLoop, frameInterval, state);
            return entity;
        }

    }

}