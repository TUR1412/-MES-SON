using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟动画管理器
    /// 管理所有动画效果和过渡
    /// </summary>
    public class LeagueAnimationManager
    {
        private static LeagueAnimationManager instance;
        private Timer animationTimer;
        private List<IAnimation> animations;
        private Dictionary<Control, ControlAnimationState> controlStates;

        public static LeagueAnimationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LeagueAnimationManager();
                return instance;
            }
        }

        private LeagueAnimationManager()
        {
            animations = new List<IAnimation>();
            controlStates = new Dictionary<Control, ControlAnimationState>();
            
            animationTimer = new Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        /// <summary>
        /// 动画定时器事件
        /// </summary>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // 更新所有动画
            for (int i = animations.Count - 1; i >= 0; i--)
            {
                animations[i].Update();
                if (animations[i].IsCompleted)
                {
                    animations.RemoveAt(i);
                }
            }

            // 更新控件状态
            List<Control> toRemove = null;
            foreach (var kvp in controlStates)
            {
                var control = kvp.Key;
                if (control == null || control.IsDisposed)
                {
                    if (toRemove == null) toRemove = new List<Control>();
                    toRemove.Add(control);
                    continue;
                }

                bool needsInvalidate = false;
                try
                {
                    needsInvalidate = kvp.Value.Update();
                }
                catch
                {
                    needsInvalidate = true;
                }

                if (needsInvalidate)
                {
                    try { control.Invalidate(); } catch { }
                }
            }

            if (toRemove != null)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    try
                    {
                        controlStates.Remove(toRemove[i]);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        /// <summary>
        /// 注册控件动画状态
        /// </summary>
        public void RegisterControl(Control control)
        {
            if (!controlStates.ContainsKey(control))
            {
                controlStates[control] = new ControlAnimationState();
            }
        }

        /// <summary>
        /// 获取控件动画状态
        /// </summary>
        public ControlAnimationState GetControlState(Control control)
        {
            RegisterControl(control);
            return controlStates[control];
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        public void AddAnimation(IAnimation animation)
        {
            animations.Add(animation);
        }

        /// <summary>
        /// 创建淡入动画
        /// </summary>
        public void FadeIn(Control control, int duration = 500)
        {
            var state = GetControlState(control);
            AddAnimation(new FadeAnimation(state, 0f, 1f, duration));
        }

        /// <summary>
        /// 创建淡出动画
        /// </summary>
        public void FadeOut(Control control, int duration = 500)
        {
            var state = GetControlState(control);
            AddAnimation(new FadeAnimation(state, state.Opacity, 0f, duration));
        }

        /// <summary>
        /// 创建脉冲动画
        /// </summary>
        public void Pulse(Control control, int duration = 1000)
        {
            var state = GetControlState(control);
            AddAnimation(new PulseAnimation(state, duration));
        }

        /// <summary>
        /// 创建发光动画
        /// </summary>
        public void Glow(Control control, int duration = 2000)
        {
            var state = GetControlState(control);
            AddAnimation(new GlowAnimation(state, duration));
        }
    }

    /// <summary>
    /// 控件动画状态
    /// </summary>
    public class ControlAnimationState
    {
        // 基础动画状态（可被外部动画修改）
        public float Opacity { get; set; } = 1.0f;
        public float GlowIntensity { get; set; } = 0f;
        public float PulseIntensity { get; set; } = 0f;
        public float Scale { get; set; } = 1.0f;
        public PointF Offset { get; set; } = PointF.Empty;

        // 微交互状态（由控件事件驱动，自动平滑到目标值）
        public float HoverProgress { get; private set; } = 0f;
        public float PressProgress { get; private set; } = 0f;

        private float _hoverTarget = 0f;
        private float _pressTarget = 0f;

        private float _lastOpacity = float.NaN;
        private float _lastGlow = float.NaN;
        private float _lastPulse = float.NaN;
        private float _lastScale = float.NaN;
        private float _lastHover = float.NaN;
        private float _lastPress = float.NaN;
        private float _lastOffsetX = float.NaN;
        private float _lastOffsetY = float.NaN;

        public void SetHover(bool hovered)
        {
            _hoverTarget = hovered ? 1f : 0f;
        }

        public void SetPressed(bool pressed)
        {
            _pressTarget = pressed ? 1f : 0f;
        }

        public bool Update()
        {
            // 首帧：初始化对比基线，避免“注册即持续刷新”
            if (float.IsNaN(_lastOpacity))
            {
                _lastOpacity = Opacity;
                _lastGlow = GlowIntensity;
                _lastPulse = PulseIntensity;
                _lastScale = Scale;
                _lastHover = HoverProgress;
                _lastPress = PressProgress;
                _lastOffsetX = Offset.X;
                _lastOffsetY = Offset.Y;
                return true;
            }

            // 确保值在合理范围内
            Opacity = Math.Max(0f, Math.Min(1f, Opacity));
            GlowIntensity = Math.Max(0f, Math.Min(1f, GlowIntensity));
            PulseIntensity = Math.Max(0f, Math.Min(1f, PulseIntensity));        
            Scale = Math.Max(0.1f, Math.Min(2f, Scale));

            // 微交互：Hover/Press 平滑过渡（避免“突然切换”的硬反馈）
            HoverProgress = Approach(HoverProgress, _hoverTarget, 0.18f);
            PressProgress = Approach(PressProgress, _pressTarget, 0.28f);

            bool changed =
                (Math.Abs(Opacity - _lastOpacity) > 0.0005f) ||
                (Math.Abs(GlowIntensity - _lastGlow) > 0.0005f) ||
                (Math.Abs(PulseIntensity - _lastPulse) > 0.0005f) ||
                (Math.Abs(Scale - _lastScale) > 0.0005f) ||
                (Math.Abs(HoverProgress - _lastHover) > 0.0005f) ||
                (Math.Abs(PressProgress - _lastPress) > 0.0005f) ||
                (Math.Abs(Offset.X - _lastOffsetX) > 0.0005f) ||
                (Math.Abs(Offset.Y - _lastOffsetY) > 0.0005f);

            _lastOpacity = Opacity;
            _lastGlow = GlowIntensity;
            _lastPulse = PulseIntensity;
            _lastScale = Scale;
            _lastHover = HoverProgress;
            _lastPress = PressProgress;
            _lastOffsetX = Offset.X;
            _lastOffsetY = Offset.Y;

            return changed;
        }

        private static float Approach(float current, float target, float speed)
        {
            // speed: 0..1（越大越快）
            if (speed <= 0f) return current;
            if (speed >= 1f) return target;

            var delta = target - current;
            if (Math.Abs(delta) < 0.001f)
            {
                return target;
            }

            return current + delta * speed;
        }
    }

    /// <summary>
    /// 动画接口
    /// </summary>
    public interface IAnimation
    {
        bool IsCompleted { get; }
        void Update();
    }

    /// <summary>
    /// 淡入淡出动画
    /// </summary>
    public class FadeAnimation : IAnimation
    {
        private ControlAnimationState state;
        private float startOpacity;
        private float endOpacity;
        private int duration;
        private int elapsed;

        public bool IsCompleted => elapsed >= duration;

        public FadeAnimation(ControlAnimationState state, float start, float end, int duration)
        {
            this.state = state;
            this.startOpacity = start;
            this.endOpacity = end;
            this.duration = duration;
            this.elapsed = 0;
        }

        public void Update()
        {
            elapsed += 16; // 假设16ms间隔
            float progress = Math.Min(1f, (float)elapsed / duration);
            
            // 使用缓动函数
            progress = EaseInOutQuad(progress);
            
            state.Opacity = startOpacity + (endOpacity - startOpacity) * progress;
        }

        private float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }
    }

    /// <summary>
    /// 脉冲动画
    /// </summary>
    public class PulseAnimation : IAnimation
    {
        private ControlAnimationState state;
        private int duration;
        private int elapsed;

        public bool IsCompleted => elapsed >= duration;

        public PulseAnimation(ControlAnimationState state, int duration)
        {
            this.state = state;
            this.duration = duration;
            this.elapsed = 0;
        }

        public void Update()
        {
            elapsed += 16;
            float progress = (float)elapsed / duration;
            
            // 正弦波脉冲
            state.PulseIntensity = (float)(Math.Sin(progress * Math.PI * 4) * 0.5 + 0.5);
        }
    }

    /// <summary>
    /// 发光动画
    /// </summary>
    public class GlowAnimation : IAnimation
    {
        private ControlAnimationState state;
        private int duration;
        private int elapsed;

        public bool IsCompleted => elapsed >= duration;

        public GlowAnimation(ControlAnimationState state, int duration)
        {
            this.state = state;
            this.duration = duration;
            this.elapsed = 0;
        }

        public void Update()
        {
            elapsed += 16;
            float progress = (float)elapsed / duration;
            
            // 渐强渐弱的发光效果
            if (progress < 0.5f)
            {
                state.GlowIntensity = progress * 2;
            }
            else
            {
                state.GlowIntensity = (1 - progress) * 2;
            }
        }
    }
}
