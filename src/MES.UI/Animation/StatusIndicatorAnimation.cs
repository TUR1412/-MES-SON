using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Animation
{
    /// <summary>
    /// 状态指示器动画
    /// </summary>
    public class StatusIndicatorAnimation
    {
        private List<Control> registeredControls;
        private int animationStep = 0;
        private float currentCpuUsage = 0;
        private float currentMemoryUsage = 0;
        private int performanceUpdateCounter = 0;
        private Random random;
        
        // 呼吸效果参数
        private const int BREATHING_CYCLE = 120; // 4秒循环 (30FPS * 4秒)
        
        public StatusIndicatorAnimation()
        {
            registeredControls = new List<Control>();
            random = new Random();
            // 初始化模拟数据
            currentCpuUsage = 15.0f;
            currentMemoryUsage = 45.0f;
        }
        
        /// <summary>
        /// 注册状态指示器控件
        /// </summary>
        public void RegisterControl(Control control)
        {
            if (!registeredControls.Contains(control))
            {
                registeredControls.Add(control);
                control.Paint += Control_Paint;
            }
        }
        
        /// <summary>
        /// 更新动画
        /// </summary>
        public void Update()
        {
            animationStep++;
            if (animationStep >= BREATHING_CYCLE)
            {
                animationStep = 0;
            }
            
            // 每30帧更新一次性能数据 (约1秒)
            performanceUpdateCounter++;
            if (performanceUpdateCounter >= 30)
            {
                UpdatePerformanceData();
                performanceUpdateCounter = 0;
            }
            
            // 刷新所有注册的控件
            foreach (Control control in registeredControls)
            {
                if (control.IsHandleCreated && !control.IsDisposed)
                {
                    control.Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 更新性能数据（模拟数据）
        /// </summary>
        private void UpdatePerformanceData()
        {
            try
            {
                // 模拟CPU使用率变化 (10-80%)
                currentCpuUsage += (float)(random.NextDouble() - 0.5) * 5;
                currentCpuUsage = Math.Max(10, Math.Min(80, currentCpuUsage));
                
                // 模拟内存使用率变化 (30-70%)
                currentMemoryUsage += (float)(random.NextDouble() - 0.5) * 3;
                currentMemoryUsage = Math.Max(30, Math.Min(70, currentMemoryUsage));
            }
            catch (Exception ex)
            {
                LogManager.Error("性能数据更新失败", ex);
            }
        }
        
        /// <summary>
        /// 控件绘制事件
        /// </summary>
        private void Control_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var control = sender as Control;
                if (control == null) return;
                
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // 绘制状态指示器
                DrawStatusIndicators(e.Graphics, control);
            }
            catch (Exception ex)
            {
                LogManager.Error("状态指示器绘制失败", ex);
            }
        }
        
        /// <summary>
        /// 绘制状态指示器
        /// </summary>
        private void DrawStatusIndicators(Graphics g, Control control)
        {
            var rect = control.ClientRectangle;
            
            // 计算呼吸效果的透明度
            double breathingProgress = (double)animationStep / BREATHING_CYCLE;
            double breathingWave = (Math.Sin(breathingProgress * Math.PI * 2) + 1) / 2; // 0.0 到 1.0
            int breathingAlpha = (int)(50 + breathingWave * 50); // 50-100透明度
            
            // 绘制系统状态灯
            DrawStatusLight(g, new Point(rect.Right - 80, rect.Top + 10), 
                GetStatusColor(currentCpuUsage), breathingAlpha, "CPU");
            
            DrawStatusLight(g, new Point(rect.Right - 40, rect.Top + 10), 
                GetStatusColor(currentMemoryUsage), breathingAlpha, "MEM");
            
            // 绘制性能数据文本
            DrawPerformanceText(g, rect);
        }
        
        /// <summary>
        /// 绘制状态指示灯
        /// </summary>
        private void DrawStatusLight(Graphics g, Point position, Color baseColor, int alpha, string label)
        {
            var lightColor = Color.FromArgb(alpha, baseColor);
            var size = 12;
            
            // 绘制外圈（呼吸效果）
            using (var outerBrush = new SolidBrush(Color.FromArgb(alpha / 3, baseColor)))
            {
                g.FillEllipse(outerBrush, position.X - 2, position.Y - 2, size + 4, size + 4);
            }
            
            // 绘制内圈
            using (var innerBrush = new SolidBrush(lightColor))
            {
                g.FillEllipse(innerBrush, position.X, position.Y, size, size);
            }
            
            // 绘制标签
            using (var font = new Font("微软雅黑", 7F))
            using (var brush = new SolidBrush(Color.FromArgb(108, 117, 125)))
            {
                var textSize = g.MeasureString(label, font);
                g.DrawString(label, font, brush, 
                    position.X + size / 2 - textSize.Width / 2, 
                    position.Y + size + 2);
            }
        }
        
        /// <summary>
        /// 绘制性能数据文本
        /// </summary>
        private void DrawPerformanceText(Graphics g, Rectangle rect)
        {
            using (var font = new Font("微软雅黑", 8F))
            using (var brush = new SolidBrush(Color.FromArgb(108, 117, 125)))
            {
                var cpuText = string.Format("CPU: {0}%", currentCpuUsage.ToString("F1"));
                var memText = string.Format("内存: {0}%", currentMemoryUsage.ToString("F1"));
                
                g.DrawString(cpuText, font, brush, rect.Right - 150, rect.Bottom - 35);
                g.DrawString(memText, font, brush, rect.Right - 150, rect.Bottom - 20);
            }
        }
        
        /// <summary>
        /// 根据使用率获取状态颜色
        /// </summary>
        private Color GetStatusColor(float usage)
        {
            if (usage < 30)
                return Color.FromArgb(40, 167, 69); // 绿色 - 正常
            else if (usage < 70)
                return Color.FromArgb(255, 193, 7); // 黄色 - 警告
            else
                return Color.FromArgb(220, 53, 69); // 红色 - 高负载
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            foreach (Control control in registeredControls)
            {
                if (control != null && !control.IsDisposed)
                {
                    control.Paint -= Control_Paint;
                }
            }
            registeredControls.Clear();
        }
    }
}
