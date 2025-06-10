using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Animation
{
    /// <summary>
    /// 浮动几何图形动画
    /// </summary>
    public class FloatingShapesAnimation
    {
        private List<Panel> registeredPanels;
        private List<FloatingShape> shapes;
        private Random random;
        private int animationStep = 0;
        
        public FloatingShapesAnimation()
        {
            registeredPanels = new List<Panel>();
            shapes = new List<FloatingShape>();
            random = new Random();
            InitializeShapes();
        }
        
        /// <summary>
        /// 初始化浮动图形
        /// </summary>
        private void InitializeShapes()
        {
            // 创建5个不同的浮动图形
            var shape1 = new FloatingShape();
            shape1.Type = ShapeType.Circle;
            shape1.X = 100;
            shape1.Y = 150;
            shape1.Size = 30;
            shape1.Color = Color.FromArgb(30, 40, 167, 69); // 绿色半透明
            shape1.SpeedX = 0.5f;
            shape1.SpeedY = 0.3f;
            shape1.RotationSpeed = 1.0f;
            shapes.Add(shape1);
            
            var shape2 = new FloatingShape();
            shape2.Type = ShapeType.Triangle;
            shape2.X = 300;
            shape2.Y = 200;
            shape2.Size = 25;
            shape2.Color = Color.FromArgb(30, 0, 123, 255); // 蓝色半透明
            shape2.SpeedX = -0.4f;
            shape2.SpeedY = 0.6f;
            shape2.RotationSpeed = -0.8f;
            shapes.Add(shape2);
            
            var shape3 = new FloatingShape();
            shape3.Type = ShapeType.Diamond;
            shape3.X = 500;
            shape3.Y = 100;
            shape3.Size = 20;
            shape3.Color = Color.FromArgb(30, 220, 53, 69); // 红色半透明
            shape3.SpeedX = 0.3f;
            shape3.SpeedY = -0.4f;
            shape3.RotationSpeed = 1.2f;
            shapes.Add(shape3);
            
            var shape4 = new FloatingShape();
            shape4.Type = ShapeType.Circle;
            shape4.X = 200;
            shape4.Y = 300;
            shape4.Size = 35;
            shape4.Color = Color.FromArgb(25, 108, 117, 125); // 灰色半透明
            shape4.SpeedX = -0.6f;
            shape4.SpeedY = -0.2f;
            shape4.RotationSpeed = 0.5f;
            shapes.Add(shape4);
            
            var shape5 = new FloatingShape();
            shape5.Type = ShapeType.Triangle;
            shape5.X = 400;
            shape5.Y = 50;
            shape5.Size = 28;
            shape5.Color = Color.FromArgb(25, 255, 193, 7); // 黄色半透明
            shape5.SpeedX = 0.2f;
            shape5.SpeedY = 0.5f;
            shape5.RotationSpeed = -1.5f;
            shapes.Add(shape5);
        }
        
        /// <summary>
        /// 注册需要绘制浮动图形的面板
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
            
            foreach (FloatingShape shape in shapes)
            {
                // 更新位置
                shape.X += shape.SpeedX;
                shape.Y += shape.SpeedY;
                
                // 更新旋转角度
                shape.Rotation += shape.RotationSpeed;
                if (shape.Rotation >= 360) shape.Rotation -= 360;
                if (shape.Rotation < 0) shape.Rotation += 360;
                
                // 边界检测和反弹
                foreach (Panel panel in registeredPanels)
                {
                    if (shape.X <= 0 || shape.X >= panel.Width - shape.Size)
                    {
                        shape.SpeedX = -shape.SpeedX;
                        shape.X = Math.Max(0, Math.Min(panel.Width - shape.Size, shape.X));
                    }
                    
                    if (shape.Y <= 0 || shape.Y >= panel.Height - shape.Size)
                    {
                        shape.SpeedY = -shape.SpeedY;
                        shape.Y = Math.Max(0, Math.Min(panel.Height - shape.Size, shape.Y));
                    }
                }
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
                
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                foreach (FloatingShape shape in shapes)
                {
                    DrawShape(e.Graphics, shape);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("浮动图形绘制失败", ex);
            }
        }
        
        /// <summary>
        /// 绘制图形
        /// </summary>
        private void DrawShape(Graphics g, FloatingShape shape)
        {
            using (var brush = new SolidBrush(shape.Color))
            {
                var matrix = g.Transform;
                g.TranslateTransform(shape.X + shape.Size / 2, shape.Y + shape.Size / 2);
                g.RotateTransform(shape.Rotation);
                g.TranslateTransform(-shape.Size / 2, -shape.Size / 2);
                
                switch (shape.Type)
                {
                    case ShapeType.Circle:
                        g.FillEllipse(brush, 0, 0, shape.Size, shape.Size);
                        break;
                        
                    case ShapeType.Triangle:
                        var trianglePoints = new PointF[3];
                        trianglePoints[0] = new PointF(shape.Size / 2, 0);
                        trianglePoints[1] = new PointF(0, shape.Size);
                        trianglePoints[2] = new PointF(shape.Size, shape.Size);
                        g.FillPolygon(brush, trianglePoints);
                        break;
                        
                    case ShapeType.Diamond:
                        var diamondPoints = new PointF[4];
                        diamondPoints[0] = new PointF(shape.Size / 2, 0);
                        diamondPoints[1] = new PointF(shape.Size, shape.Size / 2);
                        diamondPoints[2] = new PointF(shape.Size / 2, shape.Size);
                        diamondPoints[3] = new PointF(0, shape.Size / 2);
                        g.FillPolygon(brush, diamondPoints);
                        break;
                }
                
                g.Transform = matrix;
            }
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
            shapes.Clear();
        }
    }
    
    /// <summary>
    /// 浮动图形类型
    /// </summary>
    public enum ShapeType
    {
        Circle,
        Triangle,
        Diamond
    }
    
    /// <summary>
    /// 浮动图形数据
    /// </summary>
    public class FloatingShape
    {
        public ShapeType Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Size { get; set; }
        public Color Color { get; set; }
        public float SpeedX { get; set; }
        public float SpeedY { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
    }
}
