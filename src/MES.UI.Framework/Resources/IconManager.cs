using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using MES.Common.Logging;

namespace MES.UI.Framework.Resources
{
    /// <summary>
    /// 图标资源管理器 - 统一管理系统图标资源
    /// </summary>
    public static class IconManager
    {
        #region 私有字段

        private static readonly Dictionary<string, Icon> _iconCache = new Dictionary<string, Icon>();
        private static readonly Dictionary<string, Bitmap> _imageCache = new Dictionary<string, Bitmap>();

        #endregion

        #region 系统图标枚举

        /// <summary>
        /// 系统预定义图标类型
        /// </summary>
        public enum SystemIconType
        {
            /// <summary>应用程序图标</summary>
            Application,
            /// <summary>信息图标</summary>
            Information,
            /// <summary>警告图标</summary>
            Warning,
            /// <summary>错误图标</summary>
            Error,
            /// <summary>问题图标</summary>
            Question,
            /// <summary>成功图标</summary>
            Success,
            /// <summary>文件夹图标</summary>
            Folder,
            /// <summary>文件图标</summary>
            File,
            /// <summary>设置图标</summary>
            Settings,
            /// <summary>用户图标</summary>
            User,
            /// <summary>搜索图标</summary>
            Search,
            /// <summary>刷新图标</summary>
            Refresh,
            /// <summary>保存图标</summary>
            Save,
            /// <summary>删除图标</summary>
            Delete,
            /// <summary>编辑图标</summary>
            Edit,
            /// <summary>添加图标</summary>
            Add,
            /// <summary>导出图标</summary>
            Export,
            /// <summary>导入图标</summary>
            Import,
            /// <summary>打印图标</summary>
            Print
        }

        #endregion

        #region 图标获取方法

        /// <summary>
        /// 获取系统图标
        /// </summary>
        /// <param name="iconType">图标类型</param>
        /// <param name="size">图标大小</param>
        /// <returns>图标对象</returns>
        public static Icon GetSystemIcon(SystemIconType iconType, Size size = default)
        {
            if (size == default)
                size = new Size(16, 16);

            string cacheKey = $"{iconType}_{size.Width}x{size.Height}";
            
            if (_iconCache.ContainsKey(cacheKey))
                return _iconCache[cacheKey];

            try
            {
                Icon icon = CreateSystemIcon(iconType, size);
                _iconCache[cacheKey] = icon;
                return icon;
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取系统图标失败: {iconType}", ex);
                return SystemIcons.Application;
            }
        }

        /// <summary>
        /// 获取系统图标的位图版本
        /// </summary>
        /// <param name="iconType">图标类型</param>
        /// <param name="size">图标大小</param>
        /// <returns>位图对象</returns>
        public static Bitmap GetSystemIconBitmap(SystemIconType iconType, Size size = default)
        {
            if (size == default)
                size = new Size(16, 16);

            string cacheKey = $"{iconType}_bitmap_{size.Width}x{size.Height}";
            
            if (_imageCache.ContainsKey(cacheKey))
                return _imageCache[cacheKey];

            try
            {
                Icon icon = GetSystemIcon(iconType, size);
                Bitmap bitmap = icon.ToBitmap();
                _imageCache[cacheKey] = bitmap;
                return bitmap;
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取系统图标位图失败: {iconType}", ex);
                return SystemIcons.Application.ToBitmap();
            }
        }

        /// <summary>
        /// 从文件加载图标
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="size">图标大小</param>
        /// <returns>图标对象</returns>
        public static Icon LoadIconFromFile(string filePath, Size size = default)
        {
            if (!File.Exists(filePath))
            {
                LogManager.Warning($"图标文件不存在: {filePath}");
                return SystemIcons.Application;
            }

            try
            {
                string cacheKey = $"file_{Path.GetFileName(filePath)}_{size.Width}x{size.Height}";
                
                if (_iconCache.ContainsKey(cacheKey))
                    return _iconCache[cacheKey];

                Icon icon = new Icon(filePath, size);
                _iconCache[cacheKey] = icon;
                return icon;
            }
            catch (Exception ex)
            {
                LogManager.Error($"从文件加载图标失败: {filePath}", ex);
                return SystemIcons.Application;
            }
        }

        /// <summary>
        /// 从嵌入资源加载图标
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="assembly">程序集</param>
        /// <returns>图标对象</returns>
        public static Icon LoadIconFromResource(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            try
            {
                string cacheKey = $"resource_{resourceName}";
                
                if (_iconCache.ContainsKey(cacheKey))
                    return _iconCache[cacheKey];

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        LogManager.Warning($"嵌入资源不存在: {resourceName}");
                        return SystemIcons.Application;
                    }

                    Icon icon = new Icon(stream);
                    _iconCache[cacheKey] = icon;
                    return icon;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"从嵌入资源加载图标失败: {resourceName}", ex);
                return SystemIcons.Application;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建系统图标
        /// </summary>
        /// <param name="iconType">图标类型</param>
        /// <param name="size">图标大小</param>
        /// <returns>图标对象</returns>
        private static Icon CreateSystemIcon(SystemIconType iconType, Size size)
        {
            // 根据图标类型返回对应的系统图标
            switch (iconType)
            {
                case SystemIconType.Application:
                    return SystemIcons.Application;
                case SystemIconType.Information:
                    return SystemIcons.Information;
                case SystemIconType.Warning:
                    return SystemIcons.Warning;
                case SystemIconType.Error:
                    return SystemIcons.Error;
                case SystemIconType.Question:
                    return SystemIcons.Question;
                case SystemIconType.Success:
                    return SystemIcons.Information; // 使用信息图标代替
                case SystemIconType.Folder:
                    return SystemIcons.Application; // 使用应用程序图标代替
                case SystemIconType.File:
                    return SystemIcons.Application;
                case SystemIconType.Settings:
                    return SystemIcons.Application;
                case SystemIconType.User:
                    return SystemIcons.Application;
                case SystemIconType.Search:
                    return SystemIcons.Application;
                case SystemIconType.Refresh:
                    return SystemIcons.Application;
                case SystemIconType.Save:
                    return SystemIcons.Application;
                case SystemIconType.Delete:
                    return SystemIcons.Application;
                case SystemIconType.Edit:
                    return SystemIcons.Application;
                case SystemIconType.Add:
                    return SystemIcons.Application;
                case SystemIconType.Export:
                    return SystemIcons.Application;
                case SystemIconType.Import:
                    return SystemIcons.Application;
                case SystemIconType.Print:
                    return SystemIcons.Application;
                default:
                    return SystemIcons.Application;
            }
        }

        #endregion

        #region 缓存管理

        /// <summary>
        /// 清理图标缓存
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                foreach (var icon in _iconCache.Values)
                {
                    icon?.Dispose();
                }
                _iconCache.Clear();

                foreach (var image in _imageCache.Values)
                {
                    image?.Dispose();
                }
                _imageCache.Clear();

                LogManager.Info("图标缓存已清理");
            }
            catch (Exception ex)
            {
                LogManager.Error("清理图标缓存失败", ex);
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public static string GetCacheStatistics()
        {
            return $"图标缓存: {_iconCache.Count} 项, 图像缓存: {_imageCache.Count} 项";
        }

        #endregion
    }
}
