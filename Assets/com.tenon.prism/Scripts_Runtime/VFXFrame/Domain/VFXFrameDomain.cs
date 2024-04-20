using UnityEngine;

namespace TenonKit.Prism {

    internal static class VFXFrameDomain {

        // 同步坐标
        internal static void SyncPos(VFXFrameContext ctx, VFXFramePlayerEntity entity) {
            if (entity == null || !entity.hasAttachTarget || entity.attachTarget == null) {
                return;
            }
            entity.go.transform.position = entity.attachTarget.position;
            var offset = entity.attachTarget.right * entity.offset.x + entity.attachTarget.up * entity.offset.y;
            entity.go.transform.position += offset;
        }

        // 手动执行播放
        internal static bool TryPlayManualy(VFXFrameContext ctx, int vfxID) {
            var repo = ctx.Repo;
            if (!repo.TryGet(vfxID, out var entity)) {
                return false;
            }

            entity.Play();
            return true;
        }

        internal static bool TryRePlayManualy(VFXFrameContext ctx, int vfxID) {
            var repo = ctx.Repo;
            if (!repo.TryGet(vfxID, out var entity)) {
                return false;
            }

            entity.RePlay();
            return true;
        }

        // 预生成, 附加到对象, 不自动播放, 只能通过手动执行播放, 默认无延时
        internal static int TryPreSpawnVFX_ToTarget(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Transform attachTarget, Vector3 offset) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, frames, true, isLoop, frameInterval, attachTarget, offset, VFXFrameState.Stop);
        }

        // 预生成, 附加到世界坐标, 不自动播放, 只能通过手动执行播放, 默认无延时
        internal static int TryPreSpawnVFX_ToWorldPos(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Vector3 pos) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, frames, true, isLoop, frameInterval, null, pos, VFXFrameState.Stop);
        }

        // 生成, 附加到对象, 并自动播放, 默认无延时
        internal static int TrySpawnAndPlayVFX_ToTarget(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Transform attachTarget, Vector3 offset) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, frames, false, isLoop, frameInterval, attachTarget, offset, VFXFrameState.Playing);
        }

        // 生成, 附加到世界坐标, 并自动播放，默认无延时
        internal static int TrySpawnAndPlayVFX_ToWorldPos(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Vector3 pos) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, frames, false, isLoop, frameInterval, null, pos, VFXFrameState.Playing);
        }

        // 生成
        static int TrySpawnAndDelayPlayVFX(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isManural, bool isLoop, float frameInterval, Transform attachTarget, Vector3 offset, VFXFrameState state) {
            if (vfxName == null) {
                return -1;
            }

            if (!TrySpawnVFX(ctx, vfxName, frames, isManural, isLoop, frameInterval, state, out var entity)) {
                return -1;
            }

            entity.SetParent(ctx.VFXRoot, true);

            if (attachTarget != null) {
                entity.hasAttachTarget = true;
                entity.attachTarget = attachTarget;
                entity.go.transform.position = attachTarget.position + new Vector3(offset.x, offset.y, offset.z);
                entity.offset = offset;
            } else {
                entity.hasAttachTarget = false;
                entity.go.transform.position = offset;
            }

            var repo = ctx.Repo;
            repo.Add(entity);

            return entity.vfxID;
        }

        static bool TrySpawnVFX(VFXFrameContext ctx, string vfxName, Sprite[] frames, bool isManural, bool isLoop, float frameInterval, VFXFrameState state, out VFXFramePlayerEntity entity) {

            var repo = ctx.Repo;
            entity = VFXFrameFactory.SpawnVFXPlayer(ctx, vfxName, frames, isManural, isLoop, frameInterval, state);
            PLog.Log($"生成特效: 特效名称: {vfxName}; 特效状态: {state.ToCustomString()}");

            var vfxPrefab = ctx.GetVFXAssetOrDefault(vfxName);
            if (vfxPrefab == null) {
                PLog.Error($"特效资源不存在: {vfxName}");
                return false;
            }

            var go = GameObject.Instantiate(vfxPrefab);
            entity.go = go;
            entity.spr = go.AddComponent<SpriteRenderer>();

            if (isManural) {
                entity.Stop();
            } else {
                entity.Play();
            }

            return true;
        }

        internal static bool TryStopVFXManualy(VFXFrameContext ctx, int vfxID) {
            var repo = ctx.Repo;
            if (!repo.TryGet(vfxID, out var entity)) {
                return false;
            }

            entity.Stop();
            return true;
        }

    }

}