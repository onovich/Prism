# Prism
Prism, a lightweight special effects management library. <br/>
**Prism，特效管理库，取名自“棱镜”。**

![](https://github.com/onovich/Prism/blob/main/Assets/com.tenon.prism/Resources_Sample/sample_cover.jpg)

Prism provides managed functionality for the VFX lifecycle within Unity, currently supporting particle effects and sequence frame effects, with support for effects based on AnimationClip planned.<br/>
**Prism 为 Unity 里的 VFX 提供生命周期托管，目前已支持粒子特效、序列帧特效，暂未实现对 AnimationClip 的特效的支持，但仍在计划中。**

Effects can be generated at fixed locations in the scene or on moving objects. For attachments to moving objects, position synchronization is used instead of setting them as child objects. Therefore, even if the moving object is destroyed, the playback of the effect will not be interrupted, which better meets the practical needs of games.<br/>
**特效可以生成在场景中的固定位置，也可以使其跟随物体移动。这里的跟随采用了位置同步的方案，而非将其设为子对象，这样即使移动物体被销毁，特效也不会立刻中断，这更符合游戏里的实际需求。**

Effects can be played automatically or manually. Automatically played effects will be recycled automatically; currently, there is no object pooling implemented, so there will be some GC involved, and optimizations will be made in the future. Manually played effects will not be automatically recycled and can be manually replayed and destroyed.<br/>
**可以选择自动或手动播放特效。自动播放的特效会自动回收，目前暂未实现对象池，这里的 GC 将来会花时间优化。手动播放的特效则不会自动回收，还可以手动重播或手动销毁。**

Particle effects rely on loading through Addressables. A particle effect player can manage a group of effects under its child nodes, as in practical applications, the particles we use are often composite.<br/>
**粒子特效依赖 Addressable 的加载。一个粒子特效播放器可以管理其子节点下的一组特效，因为在实际的应用中，我们使用的粒子往往是组合式的。**

For frame animations, added features include: allowing for a delay after the end of playback through manual configuration, suitable for effects like shell casings or bloodstains. Disappearance can be immediate or can fade out gradually through transparency.<br/>
**针对帧动画，增加的特性有：允许通过手动配置，实现播放完后延迟一段时间再消失，这样可以适用于弹壳或血迹之类的效果。消失的方式支持了透明度渐变淡出。**

The project provides a wealth of runtime examples.<br/>
**项目内提供了丰富的运行时示例。**

# Sample
```
// Particel VFX
VFXParticelCore vfxCore;
bool isInit = false;
int preSpawnVFXID;

void Awake() {
    Transform vfxRoot = GameObject.Find("VFXRoot").transform;
    vfxCore = new VFXParticelCore("VFX_Particel", vfxRoot);
    Action main = async () => {
        await vfxCore.LoadAssets();
        vfxCore.TryPreSpawnVFX_ToWorldPos("VFX_01", 3f, preSpawnRoot.position);
        isInit = true;
    };
    main.Invoke();
}

void AddToWorld() {
    vfxCore.TrySpawnAndPlayVFX_ToWorldPos("VFX_01", 3f, role.Pos);
}

void AddToTarget() {
    vfxCore.TrySpawnAndPlayVFX_ToTarget("VFX_01", 3f, role.Transform, Vector3.zero);
}

void PlayManualy() {
    vfxCore.TryPlayManualy(preSpawnVFXID);
}

void StopManualy() {
    vfxCore.TryStopManualy(preSpawnVFXID);
}

void OnDestroy() {
    vfxCore.TearDown();
}

void LateUpdate() {
    if (!isInit) {
        return;
    }
    var dt = Time.deltaTime;
    vfxCore.Tick(dt);
}
```

```
// Frame VFX
VFXFrameCore vfxCore;

[SerializeField] Sprite[] frames;
[SerializeField] bool isLoop;
[SerializeField] bool isFlipX;
[SerializeField] string sortingLayerName;
[SerializeField] int sortingOrder;
[SerializeField] float delayEndSec;
[SerializeField] MortiseFrame.Swing.EasingMode easingOutMode;
[SerializeField] MortiseFrame.Swing.EasingType easingOutType;
[SerializeField] float easingOutDuration;
float frameInterval;
int preSpawnVFXID;

void Awake() {
    frameInterval = 1f / 12f;
    Transform vfxRoot = GameObject.Find("VFXRoot").transform;
    vfxCore = new VFXFrameCore(vfxRoot);
    preSpawnVFXID = vfxCore.TryPreSpawnVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, preSpawnRoot.position, isFlipX, sortingLayerName, sortingOrder);
    vfxCore.SetDelayEndSec(preSpawnVFXID, delayEndSec);
    vfxCore.SetFadingOut(preSpawnVFXID, easingOutDuration, easingOutType, easingOutMode);
}

void OnAddToWorld() {
    var id = vfxCore.TrySpawnAndPlayVFX_ToWorldPos("VFX_02", frames, isLoop, frameInterval, role.Pos, isFlipX, sortingLayerName, sortingOrder);
    vfxCore.SetDelayEndSec(id, delayEndSec);
    vfxCore.SetFadingOut(id, easingOutDuration, easingOutType, easingOutMode);
}

void OnAddToTarget() {
    var id = vfxCore.TrySpawnAndPlayVFX_ToTarget("VFX_02", frames, isLoop, frameInterval, role.Transform, Vector3.zero, isFlipX, sortingLayerName, sortingOrder);
    vfxCore.SetDelayEndSec(id, delayEndSec);
    vfxCore.SetFadingOut(id, easingOutDuration, easingOutType, easingOutMode);
}

void OnPlayManualy() {
    vfxCore.TryRePlayManualy(preSpawnVFXID);
}

void OnStopManualy() {
    vfxCore.TryStopManualy(preSpawnVFXID);
}

void OnDestroy() {
    vfxCore.TearDown();
}

void LateUpdate() {
    var dt = Time.deltaTime;
    vfxCore.Tick(dt);
}
```

# Readiness
Stable and available.<br/>
**稳定可用。**

# Dependency
Easing Function Library
[Swing](https://github.com/onovich/Swing)

# UPM URL
ssh://git@github.com/onovich/Prism.git?path=/Assets/com.tenon.prism#main
