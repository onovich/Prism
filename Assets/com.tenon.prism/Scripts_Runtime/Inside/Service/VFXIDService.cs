namespace TenonKit.Prism {

    internal class VFXIDService {

        // Battle
        int vfxIDRecord;

        internal VFXIDService() {
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