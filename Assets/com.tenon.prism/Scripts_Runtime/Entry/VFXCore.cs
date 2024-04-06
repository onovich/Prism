using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TenonKit.Prism {

    public class VFXCore {

        VFXContext ctx;

        public VFXCore(string assetsLabel, Transform vfxRoot) {
            ctx = new VFXContext();
            ctx.AssetsLabel = assetsLabel;
            ctx.Inject(vfxRoot);
        }

        // Load
        public async Task LoadAssets() {
            try {
                var lable = ctx.AssetsLabel;
                var list = await Addressables.LoadAssetsAsync<GameObject>(lable, null).Task;
                foreach (var prefab in list) {
                    ctx.Asset_AddPrefab(prefab.name, prefab);
                }
            } catch (Exception e) {
                PLog.Error(e.ToString());
            }
        }

        // 预生成到对象
        public int TryPreSpawnVFX_ToTarget(string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return VFXDomain.TryPreSpawnVFX_ToTarget(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir);
        }

        // 预生成到世界坐标
        public int TryPreSpawnVFX_ToWorldPos(string vfxName, float maintainSec, Vector3 pos, bool adjustDir = false) {
            return VFXDomain.TryPreSpawnVFX_ToWorldPos(ctx, vfxName, maintainSec, pos, adjustDir);
        }

        // 生成到对象
        public int TrySpawnAndPlayVFX_ToTarget(string vfxName, float maintainSec, Transform attachTarget, Vector3 offset, bool adjustDir = false) {
            return VFXDomain.TrySpawnAndPlayVFX_ToTarget(ctx, vfxName, maintainSec, attachTarget, offset, adjustDir);
        }

        // 生成到世界坐标
        public int TrySpawnAndPlayVFX_ToWorldPos(string vfxName, float maintainSec, Vector3 pos) {
            return VFXDomain.TrySpawnAndPlayVFX_ToWorldPos(ctx, vfxName, maintainSec, pos);
        }

        public bool TryPlayManualy(int vfxID) {
            return VFXDomain.TryPlayManualy(ctx, vfxID);
        }

        public bool TryStopManualy(int vfxID) {
            return VFXDomain.TryStopVFXManualy(ctx, vfxID);
        }

        public void Tick(float dt) {
            var vfxRepo = ctx.Repo;

            // 检测是否自动播放
            vfxRepo.Foreach((vfxID, entity) => {

                // 如果吸附对象不存在，则播放结束
                if (entity.HasAttachTarget && entity.AttachTarget == null) {
                    entity.SetState(VFXState.End);
                }

                // 播放
                if (entity.State == VFXState.Prepare) {
                    entity.SpendCurrentSec(dt);

                    entity.SetState(VFXState.Playing);
                    entity.ResetCurrentSec();
                    entity.Play();

                    // 同步坐标
                    VFXDomain.SyncPos(ctx, entity);
                    return;

                }

                // 等待播完,标记待销毁, 循环特效除外
                if (entity.State == VFXState.Playing) {
                    entity.SpendCurrentSec(dt);
                    VFXDomain.SyncPos(ctx, entity);

                    if (!entity.IsLoop && entity.CurrentSec >= entity.MaintainSec) {
                        entity.SetState(VFXState.End);
                    }
                    return;
                }

                // 停止播放
                if (entity.State == VFXState.End) {
                    entity.StopAll();
                    return;
                }

            });

            // 销毁待移除对象
            vfxRepo.RemoveAll(vfx => vfx.State == VFXState.End);
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