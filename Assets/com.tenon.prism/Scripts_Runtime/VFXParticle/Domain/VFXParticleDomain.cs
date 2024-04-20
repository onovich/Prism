using UnityEngine;

namespace TenonKit.Prism {

    internal static class VFXParticleDomain {

        // 同步坐标
        internal static void SyncPos(VFXParticleContext ctx, VFXParticlePlayerEntity entity) {
            if (entity == null || !entity.HasAttachTarget || entity.AttachTarget == null) {
                return;
            }
            entity.VFXGO.transform.position = entity.AttachTarget.position;
            var offset = entity.AttachTarget.right * entity.Offset.x + entity.AttachTarget.up * entity.Offset.y;
            entity.VFXGO.transform.position += offset;
        }

        // 手动执行播放
        internal static bool TryPlayManualy(VFXParticleContext ctx, int vfxID) {
            var repo = ctx.Repo;
            if (!repo.TryGet(vfxID, out var entity)) {
                return false;
            }

            entity.SetState(VFXParticleState.Prepare);
            return true;
        }

        // 预生成, 附加到对象, 不自动播放, 只能通过手动执行播放, 默认无延时
        internal static int TryPreSpawnVFX_ToTarget(VFXParticleContext ctx, string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir, VFXParticleState.Idle);
        }

        // 预生成, 附加到世界坐标, 不自动播放, 只能通过手动执行播放, 默认无延时
        internal static int TryPreSpawnVFX_ToWorldPos(VFXParticleContext ctx, string vfxName, float maintainSec, Vector3 pos, bool adjustDir = false) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, maintainSec, null, pos, adjustDir, VFXParticleState.Idle);
        }

        // 生成, 附加到对象, 并自动播放, 默认无延时
        internal static int TrySpawnAndPlayVFX_ToTarget(VFXParticleContext ctx, string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir, VFXParticleState.Prepare);
        }

        // 生成, 附加到世界坐标, 并自动播放，默认无延时
        internal static int TrySpawnAndPlayVFX_ToWorldPos(VFXParticleContext ctx, string vfxName, float maintainSec, Vector3 pos) {
            return TrySpawnAndDelayPlayVFX(ctx, vfxName, maintainSec, null, pos, false, VFXParticleState.Prepare);
        }

        // 生成, 并播放
        static int TrySpawnAndDelayPlayVFX(VFXParticleContext ctx, string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir, VFXParticleState state) {
            if (vfxName == null) {
                return -1;
            }

            if (!TrySpawnVFX(ctx, vfxName, maintainSec, state, out var entity)) {
                return -1;
            }

            entity.SetParent(ctx.VFXRoot, true);

            if (attachTarget != null) {
                entity.SetHasAttachTarget(true);
                entity.SetAttachTarget(attachTarget);
                entity.VFXGO.transform.position = attachTarget.position + new Vector3(offset.x, offset.y, offset.z);
                entity.SetOffset(offset);
            } else {
                entity.SetHasAttachTarget(false);
                entity.VFXGO.transform.position = offset;
            }

            if (adjustDir) {
                entity.VFXGO.transform.rotation = attachTarget.rotation;
            }

            var repo = ctx.Repo;
            repo.Add(entity);

            return entity.VFXID;
        }

        static bool TrySpawnVFX(VFXParticleContext ctx, string vfxName, float maintainSec, VFXParticleState state, out VFXParticlePlayerEntity entity) {

            var repo = ctx.Repo;
            entity = VFXParticleFactory.SpawnVFXPlayer(ctx, vfxName, maintainSec, state);
            PLog.Log($"生成特效: 特效名称: {vfxName}; 特效状态: {state.ToCustomString()}");

            var vfxPrefab = ctx.GetVFXAssetOrDefault(vfxName);
            if (vfxPrefab == null) {
                PLog.Error($"特效资源不存在: {vfxName}");
                return false;
            }

            var go = GameObject.Instantiate(vfxPrefab);
            entity.SetVFXGO(go);

            return true;
        }

        internal static bool TryStopVFXManualy(VFXParticleContext ctx, int vfxID) {
            var repo = ctx.Repo;
            if (!repo.TryGet(vfxID, out var entity)) {
                return false;
            }

            entity.StopAll();
            return true;
        }

    }

}