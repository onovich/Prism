using UnityEngine;

namespace TenonKit.Prism {

    internal static class VFXFrameFactory {

        internal static VFXFramePlayerEntity SpawnVFXPlayer(VFXFrameContext ctx,
                                                            GameObject go,
                                                            string vfxName,
                                                            Sprite[] frames,
                                                            bool isManural,
                                                            bool isFlipX,
                                                            bool isLoop,
                                                            float frameInterval,
                                                            VFXFrameState state,
                                                            string sortingLayerName,
                                                            int sortingOrder) {
            var entity = new VFXFramePlayerEntity();
            var id = ctx.VFXIDService.PickVFXID();
            entity.Init(go,
                        vfxName,
                        id,
                        frames,
                        isManural,
                        isFlipX,
                        isLoop,
                        frameInterval,
                        state,
                        sortingLayerName,
                        sortingOrder);
            return entity;
        }

    }

}