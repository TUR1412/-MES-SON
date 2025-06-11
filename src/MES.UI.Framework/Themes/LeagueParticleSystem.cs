using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟粒子效果系统
    /// 实现游戏级的粒子特效
    /// </summary>
    public class LeagueParticleSystem
    {
        private List<Particle> particles;
        private Random random;
        private Rectangle bounds;

        public LeagueParticleSystem(Rectangle bounds)
        {
            this.bounds = bounds;
            this.particles = new List<Particle>();
            this.random = new Random();
        }

        /// <summary>
        /// 更新粒子系统
        /// </summary>
        public void Update()
        {
            // 更新现有粒子
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update();
                if (particles[i].IsDead)
                {
                    particles.RemoveAt(i);
                }
            }

            // 随机生成新粒子
            if (random.NextDouble() < 0.3) // 30%概率生成新粒子
            {
                GenerateParticle();
            }
        }

        /// <summary>
        /// 绘制粒子系统
        /// </summary>
        public void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }

        /// <summary>
        /// 生成新粒子
        /// </summary>
        private void GenerateParticle()
        {
            var particle = new Particle
            {
                X = bounds.X + random.Next(bounds.Width),
                Y = bounds.Y + random.Next(bounds.Height),
                VelocityX = (float)(random.NextDouble() - 0.5) * 2,
                VelocityY = (float)(random.NextDouble() - 0.5) * 2,
                Life = 1.0f,
                MaxLife = 60 + random.Next(120), // 1-3秒寿命
                Size = 1 + random.Next(3),
                Color = GetRandomParticleColor()
            };

            particles.Add(particle);
        }

        /// <summary>
        /// 获取随机粒子颜色
        /// </summary>
        private Color GetRandomParticleColor()
        {
            var colors = new Color[]
            {
                Color.FromArgb(150, LeagueColors.TextGold),
                Color.FromArgb(120, LeagueColors.PrimaryGold),
                Color.FromArgb(100, LeagueColors.PrimaryGoldLight),
                Color.FromArgb(80, Color.White)
            };

            return colors[random.Next(colors.Length)];
        }

        /// <summary>
        /// 爆发效果 - 在指定位置生成大量粒子
        /// </summary>
        public void Burst(Point center, int count = 20)
        {
            for (int i = 0; i < count; i++)
            {
                double angle = (Math.PI * 2 * i) / count;
                float speed = 2 + (float)random.NextDouble() * 3;

                var particle = new Particle
                {
                    X = center.X,
                    Y = center.Y,
                    VelocityX = (float)(Math.Cos(angle) * speed),
                    VelocityY = (float)(Math.Sin(angle) * speed),
                    Life = 1.0f,
                    MaxLife = 30 + random.Next(60),
                    Size = 2 + random.Next(4),
                    Color = GetRandomParticleColor()
                };

                particles.Add(particle);
            }
        }
    }

    /// <summary>
    /// 单个粒子类
    /// </summary>
    public class Particle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Life { get; set; }
        public int MaxLife { get; set; }
        public int Size { get; set; }
        public Color Color { get; set; }
        public int Age { get; private set; }

        public bool IsDead => Age >= MaxLife;

        /// <summary>
        /// 更新粒子状态
        /// </summary>
        public void Update()
        {
            // 更新位置
            X += VelocityX;
            Y += VelocityY;

            // 更新年龄和生命值
            Age++;
            Life = 1.0f - (float)Age / MaxLife;

            // 添加重力效果
            VelocityY += 0.05f;

            // 添加阻力
            VelocityX *= 0.99f;
            VelocityY *= 0.99f;
        }

        /// <summary>
        /// 绘制粒子
        /// </summary>
        public void Draw(Graphics g)
        {
            if (IsDead) return;

            // 计算透明度
            int alpha = (int)(Life * Color.A);
            var drawColor = Color.FromArgb(alpha, Color);

            // 绘制粒子
            using (var brush = new SolidBrush(drawColor))
            {
                var rect = new RectangleF(X - Size / 2f, Y - Size / 2f, Size, Size);
                g.FillEllipse(brush, rect);
            }

            // 添加发光效果
            if (Life > 0.5f)
            {
                var glowAlpha = (int)((Life - 0.5f) * 2 * 60);
                var glowColor = Color.FromArgb(glowAlpha, Color.White);
                using (var pen = new Pen(glowColor, 1))
                {
                    var glowRect = new RectangleF(X - Size, Y - Size, Size * 2, Size * 2);
                    g.DrawEllipse(pen, glowRect);
                }
            }
        }
    }
}
