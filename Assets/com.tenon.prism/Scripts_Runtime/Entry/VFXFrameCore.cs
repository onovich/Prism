using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MortiseFrame.Swing;
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
        public int TryPreSpawnVFX_ToTarget(string vfxName,
                                           Sprite[] frames,
                                           bool isLoop,
                                           float frameInterval,
                                           Transform attachTarget,
                                           Vector3 offset,
                                           bool isFlipX = false,
                                           string sortingLayerName = "Default",
                                           int sortingOrder = 0) {
            return VFXFrameDomain.TryPreSpawnVFX_ToTarget(ctx,
                                                          vfxName,
                                                          frames,
                                                          isFlipX,
                                                          isLoop,
                                                          frameInterval,
                                                          attachTarget,
                                                          offset,
                                                          sortingLayerName,
                                                          sortingOrder);
        }

        // 预生成到世界坐标
        public int TryPreSpawnVFX_ToWorldPos(string vfxName,
                                             Sprite[] frames,
                                             bool isLoop,
                                             float frameInterval,
                                             Vector3 pos,
                                             bool isFlipX = false,
                                             string sortingLayerName = "Default",
                                             int sortingOrder = 0) {
            return VFXFrameDomain.TryPreSpawnVFX_ToWorldPos(ctx,
                                                            vfxName,
                                                            frames,
                                                            isFlipX,
                                                            isLoop,
                                                            frameInterval,
                                                            pos,
                                                            sortingLayerName,
                                                            sortingOrder);
        }

        // 生成到对象
        public int TrySpawnAndPlayVFX_ToTarget(string vfxName,
                                               Sprite[] frames,
                                               bool isLoop,
                                               float frameInterval,
                                               Transform attachTarget,
                                               Vector3 offset,
                                               bool isFlipX = false,
                                               string sortingLayerName = "Default",
                                               int sortingOrder = 0) {
            return VFXFrameDomain.TrySpawnAndPlayVFX_ToTarget(ctx,
                                                              vfxName,
                                                              frames,
                                                              isFlipX,
                                                              isLoop,
                                                              frameInterval,
                                                              attachTarget,
                                                              offset,
                                                              sortingLayerName,
                                                              sortingOrder);
        }

        // 生成到世界坐标
        public int TrySpawnAndPlayVFX_ToWorldPos(string vfxName,
                                                 Sprite[] frames,
                                                 bool isLoop,
                                                 float frameInterval,
                                                 Vector3 pos,
                                                 bool isFlipX = false,
                                                 string sortingLayerName = "Default",
                                                 int sortingOrder = 0) {
            return VFXFrameDomain.TrySpawnAndPlayVFX_ToWorldPos(ctx,
                                                                vfxName,
                                                                frames,
                                                                isFlipX,
                                                                isLoop,
                                                                frameInterval,
                                                                pos,
                                                                sortingLayerName,
                                                                sortingOrder);
        }

        // 手动播放预生成动画
        public bool TryPlayManualy(int vfxID) {
            return VFXFrameDomain.TryPlayManualy(ctx, vfxID);
        }

        // 手动重播预生成动画
        public bool TryRePlayManualy(int vfxID) {
            return VFXFrameDomain.TryRePlayManualy(ctx, vfxID);
        }

        // 手动停止预生成动画
        public bool TryStopManualy(int vfxID) {
            return VFXFrameDomain.TryStopVFXManualy(ctx, vfxID);
        }

        // 手动翻转帧动画
        public void FlipX(int vfxID, bool isFlipX) {
            VFXFrameDomain.FlipX(ctx, vfxID, isFlipX);
        }

        // 手动设置帧动画延迟结束时长
        public void SetDelayEndSec(int vfxID, float delaySec) {
            VFXFrameDomain.SetDelayEndSec(ctx, vfxID, delaySec);
        }

        // 手动设置帧动画淡出配置
        public void SetFadingOut(int vfxID, float fadeOutDuration, EasingType easingType = EasingType.Linear, EasingMode easingMode = EasingMode.None) {
            VFXFrameDomain.SetFadingOut(ctx, vfxID, fadeOutDuration, easingType, easingMode);
        }

        public void Tick(float dt) {
            var vfxRepo = ctx.Repo;

            // 检测是否自动播放
            vfxRepo.Foreach((vfxID, entity) => {
                // 播放
                entity.TickPlay(dt);

                // 延迟结束
                entity.TickEnd(dt);

                if (entity.State == VFXFrameState.Playing) {
                    // 同步坐标
                    VFXFrameDomain.SyncPos(ctx, entity);
                    return;
                }
            });

            // 销毁待移除对象
            vfxRepo.RemoveAll(vfx => vfx.State == VFXFrameState.End);
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