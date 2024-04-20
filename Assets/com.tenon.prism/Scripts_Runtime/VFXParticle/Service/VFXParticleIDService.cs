namespace TenonKit.Prism {

    internal class VFXParticleIDService {

        // Battle
        int vfxIDRecord;

        internal VFXParticleIDService() {
            this.vfxIDRecord = 0;
        }

        internal int PickVFXID() {
            vfxIDRecord += 1;
            return vfxIDRecord;
        }
        internal void Reset() {
            vfxIDRecord = 0;
        }

    }

}