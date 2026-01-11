using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MES.Common.IO
{
    /// <summary>
    /// 读取文本文件尾部内容（适用于日志等不断追加写入的文件）。
    /// - 通过 FileShare.ReadWrite 打开，避免与写入方冲突
    /// - 默认只读取尾部固定字节数，避免大文件一次性读入内存
    /// </summary>
    public static class TextFileTailReader
    {
        private const int DefaultMaxBytes = 256 * 1024;

        public static string ReadTailText(string path, int maxLines)
        {
            return ReadTailText(path, maxLines, DefaultMaxBytes);
        }

        public static string ReadTailText(string path, int maxLines, int maxBytes)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            if (maxLines <= 0) return string.Empty;
            if (!File.Exists(path)) return string.Empty;

            if (maxBytes <= 0)
            {
                maxBytes = DefaultMaxBytes;
            }

            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    long length = stream.Length;
                    if (length <= 0) return string.Empty;

                    long start = length - maxBytes;
                    if (start < 0) start = 0;

                    stream.Seek(start, SeekOrigin.Begin);
                    int toRead = (int)(length - start);
                    if (toRead <= 0) return string.Empty;

                    var buffer = new byte[toRead];
                    int read = 0;
                    while (read < toRead)
                    {
                        int n = stream.Read(buffer, read, toRead - read);
                        if (n <= 0) break;
                        read += n;
                    }

                    var text = Encoding.UTF8.GetString(buffer, 0, read);

                    // 如果不是从文件头开始读取，可能落在半行中，丢弃第一行残片
                    if (start > 0)
                    {
                        int firstNewLine = text.IndexOf('\n');
                        if (firstNewLine >= 0 && firstNewLine + 1 < text.Length)
                        {
                            text = text.Substring(firstNewLine + 1);
                        }
                    }

                    var lines = SplitLines(text);
                    if (lines.Count <= maxLines)
                    {
                        return JoinLines(lines, 0);
                    }

                    int startIndex = lines.Count - maxLines;
                    return JoinLines(lines, startIndex);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static List<string> SplitLines(string text)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(text)) return lines;

            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        private static string JoinLines(List<string> lines, int startIndex)
        {
            if (lines == null || lines.Count == 0) return string.Empty;
            if (startIndex < 0) startIndex = 0;
            if (startIndex >= lines.Count) return string.Empty;

            var sb = new StringBuilder();
            for (int i = startIndex; i < lines.Count; i++)
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.Append(lines[i] ?? string.Empty);
            }

            return sb.ToString();
        }
    }
}

