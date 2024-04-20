namespace TenonKit.Prism {

    internal static class VFXParticleFactory {

        internal static VFXParticlePlayerEntity SpawnVFXPlayer(VFXParticleContext ctx, string vfxName, float maintainSec, VFXParticleState state) {
            var entity = new VFXParticlePlayerEntity();
            var id = ctx.VFXIDService.PickVFXID();
            entity.Init(vfxName, id, maintainSec, state);
            return entity;
        }

    }

}