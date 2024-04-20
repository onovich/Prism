namespace TenonKit.Prism {

    internal enum VFXFrameState {
        None,
        Stop,
        Playing,
        End,
    }

    internal static class VFXFrameStateTypeExtensions {
        internal static string ToCustomString(this VFXFrameState attr) {
            switch (attr) {
                case VFXFrameState.None:
                    return "None";
                case VFXFrameState.Stop:
                    return "暂停";
                case VFXFrameState.Playing:
                    return "播放中";
                case VFXFrameState.End:
                    return "结束";
                default:
                    return "Unknown";
            }
        }
    }

}