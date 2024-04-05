namespace TenonKit.Prism {

    internal enum VFXState {

        None,
        Idle,
        Prepare,
        Playing,
        End,

    }

    internal static class VFXStateTypeExtensions {
        internal static string ToCustomString(this VFXState attr) {
            switch (attr) {
                case VFXState.None:
                    return "None";
                case VFXState.Idle:
                    return "待机";
                case VFXState.Prepare:
                    return "准备";
                case VFXState.Playing:
                    return "播放中";
                case VFXState.End:
                    return "已结束";
                default:
                    return "Unknown";
            }
        }
    }

}