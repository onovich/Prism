namespace TenonKit.Prism {

    internal static class VFXFactory {

        internal static VFXPlayerEntity SpawnVFXPlayer(VFXContext ctx, string vfxName, float maintainSec, VFXState state) {
            var entity = new VFXPlayerEntity();
            var id = ctx.VFXIDService.PickVFXID();
            entity.Init(vfxName, id, maintainSec, state);
            return entity;
        }

    }

}