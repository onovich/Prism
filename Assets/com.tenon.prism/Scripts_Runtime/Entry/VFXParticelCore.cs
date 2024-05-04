using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TenonKit.Prism {

    public class VFXParticelCore {

        VFXParticleContext ctx;

        public VFXParticelCore(string assetsLabel, Transform vfxRoot) {
            ctx = new VFXParticleContext();
            ctx.AssetsLabel = assetsLabel;
            ctx.Inject(vfxRoot);
        }

        // Load
        public async Task LoadAssets() {
            try {
                var handle = Addressables.LoadAssetsAsync<GameObject>(ctx.AssetsLabel, null);
                var list = await handle.Task;
                foreach (var prefab in list) {
                    ctx.Asset_AddPrefab(prefab.name, prefab);
                }
                ctx.assetHandle = handle;
            } catch (Exception e) {
                PLog.Error(e.ToString());
            }
        }

        // Release
        public void ReleaseAssets() {
            if (ctx.assetHandle.IsValid()) {
                Addressables.Release(ctx.assetHandle);
            }
        }

        // 预生成到对象
        public int TryPreSpawnVFX_ToTarget(string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return VFXParticleDomain.TryPreSpawnVFX_ToTarget(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir);
        }

        // 预生成到世界坐标
        public int TryPreSpawnVFX_ToWorldPos(string vfxName, float maintainSec, Vector3 pos, bool adjustDir = false) {
            return VFXParticleDomain.TryPreSpawnVFX_ToWorldPos(ctx, vfxName, maintainSec, pos, adjustDir);
        }

        // 生成到对象
        public int TrySpawnAndPlayVFX_ToTarget(string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return VFXParticleDomain.TrySpawnAndPlayVFX_ToTarget(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir);
        }

        // 生成到世界坐标
        public int TrySpawnAndPlayVFX_ToWorldPos(string vfxName, float maintainSec, Vector3 pos) {
            return VFXParticleDomain.TrySpawnAndPlayVFX_ToWorldPos(ctx, vfxName, maintainSec, pos);
        }

        public bool TryPlayManualy(int vfxID) {
            return VFXParticleDomain.TryPlayManualy(ctx, vfxID);
        }

        public bool TryStopManualy(int vfxID) {
            return VFXParticleDomain.TryStopVFXManualy(ctx, vfxID);
        }

        public void Tick(float dt) {
            var vfxRepo = ctx.Repo;

            // 检测是否自动播放
            vfxRepo.Foreach((vfxID, entity) => {

                // 如果吸附对象不存在，则播放结束
                if (entity.HasAttachTarget && entity.AttachTarget == null) {
                    entity.SetState(VFXParticleState.End);
                }

                // 播放
                if (entity.State == VFXParticleState.Prepare) {
                    entity.SpendCurrentSec(dt);

                    entity.SetState(VFXParticleState.Playing);
                    entity.ResetCurrentSec();
                    entity.Play();

                    // 同步坐标
                    VFXParticleDomain.SyncPos(ctx, entity);
                    return;

                }

                // 等待播完,标记待销毁, 循环特效除外
                if (entity.State == VFXParticleState.Playing) {
                    entity.SpendCurrentSec(dt);
                    VFXParticleDomain.SyncPos(ctx, entity);

                    if (!entity.IsLoop && entity.CurrentSec >= entity.MaintainSec) {
                        entity.SetState(VFXParticleState.End);
                    }
                    return;
                }

                // 停止播放
                if (entity.State == VFXParticleState.End) {
                    entity.StopAll();
                    return;
                }

            });

            // 销毁待移除对象
            vfxRepo.RemoveAll(vfx => vfx.State == VFXParticleState.End);
        }

        public void TearDown() {
            var repo = ctx.Repo;
            repo.Foreach((id, vfx) => {
                vfx.TearDown();
            });
            repo.Clear();
        }

    }

}