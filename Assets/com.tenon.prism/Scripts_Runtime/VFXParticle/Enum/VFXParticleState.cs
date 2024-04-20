namespace TenonKit.Prism {

    internal enum VFXParticleState {

        None,
        Idle,
        Prepare,
        Playing,
        End,

    }

    internal static class VFXParticleStateTypeExtensions {
        internal static string ToCustomString(this VFXParticleState attr) {
            switch (attr) {
                case VFXParticleState.None:
                    return "None";
                case VFXParticleState.Idle:
                    return "待机";
                case VFXParticleState.Prepare:
                    return "准备";
                case VFXParticleState.Playing:
                    return "播放中";
                case VFXParticleState.End:
                    return "已结束";
                default:
                    return "Unknown";
            }
        }
    }

}