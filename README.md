# Prism
Prism, a lightweight special effects management library. <br/>
**Prism，特效管理库，取名自“棱镜”。**

![](https://github.com/onovich/Prism/blob/main/Assets/com.tenon.prism/Resources_Sample/sample_cover.jpg)

Prism provides managed functionality for the VFX lifecycle within Unity, currently supporting particle effects and sequence frame effects, with support for effects based on AnimationClip planned.<br/>
**Prism 为 Unity 内的 VFX 生命周期提供托管功能，目前已支撑粒子特效、序列帧特效，基于 AnimationClip 的特效支持则在计划中。**

Effects can be generated at fixed locations in the scene or on moving objects. For attachments to moving objects, position synchronization is used instead of setting them as child objects. Therefore, even if the moving object is destroyed, the playback of the effect will not be interrupted, which better meets the practical needs of games.<br/>
**特效可以生成在场景中的固定位置，也可以生成在移动中的物体上。对移动物体的附加，采用了位置同步，而非将其设为子对象，因此即使移动物体被销毁，也不会中断特效的播放，这更符合游戏的实际需要。**

Effects can be played automatically or manually. Automatically played effects will be recycled automatically; currently, there is no object pooling implemented, so there will be some GC involved, and optimizations will be made in the future. Manually played effects will not be automatically recycled and can be manually replayed and destroyed.<br/>
**特效可以自动播放，或手动播放。自动播放的特效会自动回收，目前暂未实现对象池，会有一定 GC，优化会在将来完成。手动播放的特效则不会自动回收，并可手动重播，以及手动销毁。**

For frame animations, added features include: allowing for a delay after the end of playback through manual configuration, suitable for effects like shell casings or bloodstains. Disappearance can be immediate or can fade out gradually through transparency.
**针对帧动画，增加的特性有：允许通过手动配置，实现播放完后延迟一段时间再消失，这样可以适用于弹壳或血迹之类的效果。消失可以立即消失，也可以以透明度渐变淡出的方式消失。**

The project provides a wealth of runtime examples.<br/>
**项目内提供了丰富的运行时示例。**

# Readiness
Stable and available.<br/>
**稳定可用。**

# Dependency
Easing Function Library
[Swing](https://github.com/onovich/Swing)

# UPM URL
ssh://git@github.com/onovich/Prism.git?path=/Assets/com.tenon.prism#main
