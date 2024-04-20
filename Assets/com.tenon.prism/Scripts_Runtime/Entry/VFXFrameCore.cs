using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TenonKit.Prism {

    public class VFXFrameCore {

        VFXFrameContext ctx;

        public VFXFrameCore(string assetsLabel, Transform vfxRoot) {
            ctx = new VFXFrameContext();
            ctx.assetsLabel = assetsLabel;
            ctx.Inject(vfxRoot);
        }

        // Load
        public async Task LoadAssets() {
            try {
                var lable = ctx.assetsLabel;
                var list = await Addressables.LoadAssetsAsync<GameObject>(lable, null).Task;
                foreach (var prefab in list) {
                    ctx.Asset_AddPrefab(prefab.name, prefab);
                }
            } catch (Exception e) {
                PLog.Error(e.ToString());
            }
        }

        // 预生成到对象
        public int TryPreSpawnVFX_ToTarget(string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Transform attachTarget, Vector3 offset) {
            return VFXFrameDomain.TryPreSpawnVFX_ToTarget(ctx, vfxName, frames, isLoop, frameInterval, attachTarget, offset);
        }

        // 预生成到世界坐标
        public int TryPreSpawnVFX_ToWorldPos(string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Vector3 pos) {
            return VFXFrameDomain.TryPreSpawnVFX_ToWorldPos(ctx, vfxName, frames, isLoop, frameInterval, pos);
        }

        // 生成到对象
        public int TrySpawnAndPlayVFX_ToTarget(string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Transform attachTarget, Vector3 offset) {
            return VFXFrameDomain.TrySpawnAndPlayVFX_ToTarget(ctx, vfxName, frames, isLoop, frameInterval, attachTarget, offset);
        }

        // 生成到世界坐标
        public int TrySpawnAndPlayVFX_ToWorldPos(string vfxName, Sprite[] frames, bool isLoop, float frameInterval, Vector3 pos) {
            return VFXFrameDomain.TrySpawnAndPlayVFX_ToWorldPos(ctx, vfxName, frames, isLoop, frameInterval, pos);
        }

        public bool TryPlayManualy(int vfxID) {
            return VFXFrameDomain.TryPlayManualy(ctx, vfxID);
        }

        public bool TryStopManualy(int vfxID) {
            return VFXFrameDomain.TryStopVFXManualy(ctx, vfxID);
        }

        public void Tick(float dt) {
            var vfxRepo = ctx.Repo;

            // 检测是否自动播放
            vfxRepo.Foreach((vfxID, entity) => {
                // 播放
                entity.TickPlay(dt);

                if (entity.state == VFXFrameState.Playing) {
                    // 同步坐标
                    VFXFrameDomain.SyncPos(ctx, entity);
                    return;
                }
            });

            // 销毁待移除对象
            vfxRepo.RemoveAll(vfx => vfx.state == VFXFrameState.Stop);
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