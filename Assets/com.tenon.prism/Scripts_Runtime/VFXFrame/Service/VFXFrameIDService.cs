namespace TenonKit.Prism {

    internal class VFXFrameIDService {

        // Battle
        int vfxIDRecord;

        internal VFXFrameIDService() {
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