using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Animation
{
    /// <summary>
    /// 背景渐变动画
    /// </summary>
    public class BackgroundGradientAnimation
    {
        private List<Panel> registeredPanels;
        private int animationStep = 0;
        private const int ANIMATION_CYCLE = 900; // 30秒循环 (30FPS * 30秒)
        
        // 渐变颜色定义
        private readonly Color baseColor1 = Color.FromArgb(248, 249, 250);
        private readonly Color baseColor2 = Color.FromArgb(250, 251, 252);
        private readonly Color accentColor1 = Color.FromArgb(245, 247, 250);
        private readonly Color accentColor2 = Color.FromArgb(252, 253, 254);
        
        public BackgroundGradientAnimation()
        {
            registeredPanels = new List<Panel>();
        }
        
        /// <summary>
        /// 注册需要背景渐变的面板
        /// </summary>
        public void RegisterPanel(Panel panel)
        {
            if (!registeredPanels.Contains(panel))
            {
                registeredPanels.Add(panel);
                panel.Paint += Panel_Paint;
            }
        }
        
        /// <summary>
        /// 更新动画
        /// </summary>
        public void Update()
        {
            animationStep++;
            if (animationStep >= ANIMATION_CYCLE)
            {
                animationStep = 0;
            }
            
            // 刷新所有注册的面板
            foreach (Panel panel in registeredPanels)
            {
                if (panel.IsHandleCreated && !panel.IsDisposed)
                {
                    panel.Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 面板绘制事件
        /// </summary>
        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var panel = sender as Panel;
                if (panel == null) return;
                
                // 计算当前动画进度 (0.0 到 1.0)
                double progress = (double)animationStep / ANIMATION_CYCLE;
                
                // 使用正弦波创建平滑的颜色过渡
                double wave = (Math.Sin(progress * Math.PI * 2) + 1) / 2; // 0.0 到 1.0
                
                // 计算当前的渐变颜色
                Color currentColor1 = InterpolateColor(baseColor1, accentColor1, wave);
                Color currentColor2 = InterpolateColor(baseColor2, accentColor2, wave);
                
                // 创建线性渐变画刷
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, panel.Width, panel.Height),
                    currentColor1,
                    currentColor2,
                    LinearGradientMode.Vertical))
                {
                    // 添加渐变混合
                    var blend = new ColorBlend();
                    
                    // 创建颜色数组
                    var colors = new Color[4];
                    colors[0] = currentColor1;
                    colors[1] = InterpolateColor(currentColor1, currentColor2, 0.3f);
                    colors[2] = InterpolateColor(currentColor1, currentColor2, 0.7f);
                    colors[3] = currentColor2;
                    blend.Colors = colors;
                    
                    // 创建位置数组
                    var positions = new float[4];
                    positions[0] = 0.0f;
                    positions[1] = 0.3f;
                    positions[2] = 0.7f;
                    positions[3] = 1.0f;
                    blend.Positions = positions;
                    brush.InterpolationColors = blend;
                    
                    // 绘制背景
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("背景渐变绘制失败", ex);
            }
        }
        
        /// <summary>
        /// 颜色插值
        /// </summary>
        private Color InterpolateColor(Color color1, Color color2, double factor)
        {
            factor = Math.Max(0, Math.Min(1, factor)); // 确保在0-1范围内
            
            int r = (int)(color1.R + (color2.R - color1.R) * factor);
            int g = (int)(color1.G + (color2.G - color1.G) * factor);
            int b = (int)(color1.B + (color2.B - color1.B) * factor);
            
            return Color.FromArgb(r, g, b);
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            foreach (Panel panel in registeredPanels)
            {
                if (panel != null && !panel.IsDisposed)
                {
                    panel.Paint -= Panel_Paint;
                }
            }
            registeredPanels.Clear();
        }
    }
}
