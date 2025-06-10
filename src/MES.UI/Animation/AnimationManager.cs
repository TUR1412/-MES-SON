using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Animation
{
    /// <summary>
    /// 动画管理器 - 统一管理所有界面动画效果
    /// </summary>
    public class AnimationManager
    {
        private Timer animationTimer;
        private bool isAnimationEnabled = true;
        private bool isWindowMinimized = false;
        
        // 动画组件
        private FloatingShapesAnimation floatingShapes;
        private BackgroundGradientAnimation backgroundGradient;
        private StatusIndicatorAnimation statusIndicator;
        
        // 动画设置
        public bool IsAnimationEnabled 
        { 
            get { return isAnimationEnabled; } 
            set 
            { 
                isAnimationEnabled = value;
                if (animationTimer != null)
                {
                    animationTimer.Enabled = value && !isWindowMinimized;
                }
            } 
        }
        
        public bool IsFloatingShapesEnabled { get; set; }
        public bool IsBackgroundGradientEnabled { get; set; }
        public bool IsStatusIndicatorEnabled { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AnimationManager()
        {
            IsFloatingShapesEnabled = true;
            IsBackgroundGradientEnabled = true;
            IsStatusIndicatorEnabled = true;
        }
        
        /// <summary>
        /// 初始化动画管理器
        /// </summary>
        public void Initialize(Form mainForm)
        {
            try
            {
                // 创建动画定时器 (30FPS)
                animationTimer = new Timer();
                animationTimer.Interval = 33; // 约30FPS
                animationTimer.Tick += AnimationTimer_Tick;
                
                // 初始化动画组件
                floatingShapes = new FloatingShapesAnimation();
                backgroundGradient = new BackgroundGradientAnimation();
                statusIndicator = new StatusIndicatorAnimation();
                
                // 监听窗体状态变化
                mainForm.WindowStateChanged += MainForm_WindowStateChanged;
                
                // 启动动画
                if (isAnimationEnabled)
                {
                    animationTimer.Start();
                }
                
                LogManager.Info("动画管理器初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("动画管理器初始化失败", ex);
            }
        }
        
        /// <summary>
        /// 动画定时器事件
        /// </summary>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 更新各个动画组件
                if (IsFloatingShapesEnabled)
                {
                    floatingShapes.Update();
                }
                
                if (IsBackgroundGradientEnabled)
                {
                    backgroundGradient.Update();
                }
                
                if (IsStatusIndicatorEnabled)
                {
                    statusIndicator.Update();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("动画更新失败", ex);
            }
        }
        
        /// <summary>
        /// 窗体状态变化事件
        /// </summary>
        private void MainForm_WindowStateChanged(object sender, EventArgs e)
        {
            var form = sender as Form;
            if (form != null)
            {
                isWindowMinimized = (form.WindowState == FormWindowState.Minimized);
                
                // 窗体最小化时暂停动画以节省性能
                if (animationTimer != null)
                {
                    animationTimer.Enabled = isAnimationEnabled && !isWindowMinimized;
                }
            }
        }
        
        /// <summary>
        /// 注册浮动图形面板
        /// </summary>
        public void RegisterFloatingShapesPanel(Panel panel)
        {
            if (floatingShapes != null)
            {
                floatingShapes.RegisterPanel(panel);
            }
        }
        
        /// <summary>
        /// 注册背景渐变面板
        /// </summary>
        public void RegisterBackgroundGradientPanel(Panel panel)
        {
            if (backgroundGradient != null)
            {
                backgroundGradient.RegisterPanel(panel);
            }
        }
        
        /// <summary>
        /// 注册状态指示器控件
        /// </summary>
        public void RegisterStatusIndicator(Control control)
        {
            if (statusIndicator != null)
            {
                statusIndicator.RegisterControl(control);
            }
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (animationTimer != null)
                {
                    animationTimer.Stop();
                    animationTimer.Dispose();
                    animationTimer = null;
                }
                
                if (floatingShapes != null)
                {
                    floatingShapes.Dispose();
                    floatingShapes = null;
                }
                
                if (backgroundGradient != null)
                {
                    backgroundGradient.Dispose();
                    backgroundGradient = null;
                }
                
                if (statusIndicator != null)
                {
                    statusIndicator.Dispose();
                    statusIndicator = null;
                }
                
                LogManager.Info("动画管理器资源已释放");
            }
            catch (Exception ex)
            {
                LogManager.Error("动画管理器资源释放失败", ex);
            }
        }
    }
}
