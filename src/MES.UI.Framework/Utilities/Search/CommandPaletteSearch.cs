using System;

namespace MES.UI.Framework.Utilities.Search
{
    /// <summary>
    /// Command Palette 搜索打分器：支持多词查询 + 模糊匹配。
    /// 说明：该类为纯逻辑，可被单元测试覆盖，不依赖 WinForms UI。
    /// </summary>
    public static class CommandPaletteSearch
    {
        public static int Score(string title, string subtitle, string keywords, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return 0;

            var safeTitle = title ?? string.Empty;
            var safeSubtitle = subtitle ?? string.Empty;
            var safeKeywords = keywords ?? string.Empty;

            var q = query.Trim();
            var tokens = q.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return 0;

            // SearchText 作为更宽的兜底（Title + Subtitle + Keywords）
            var searchText = string.Join(" ", new[] { safeTitle, safeSubtitle, safeKeywords }).Trim();

            int total = 0;
            foreach (var token in tokens)
            {
                var tokenScore = ScoreToken(safeTitle, safeSubtitle, safeKeywords, searchText, token);
                if (tokenScore <= 0) return 0;
                total += tokenScore;
            }

            return total;
        }

        private static int ScoreToken(string title, string subtitle, string keywords, string searchText, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return 0;

            int score = 0;

            if (title.Equals(token, StringComparison.OrdinalIgnoreCase)) score += 1200;
            if (title.StartsWith(token, StringComparison.OrdinalIgnoreCase)) score += 700;
            if (title.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) score += 500;

            if (subtitle.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) score += 180;
            if (keywords.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) score += 150;

            int fuzzyTitle = FuzzyScore(title, token);
            if (fuzzyTitle >= 0) score += 120 + fuzzyTitle;

            int fuzzySearch = FuzzyScore(searchText, token);
            if (fuzzySearch >= 0) score += 40 + fuzzySearch;

            return score;
        }

        /// <summary>
        /// 返回 -1 表示不匹配；返回 >=0 表示匹配度分数（越大越好）。
        /// </summary>
        public static int FuzzyScore(string text, string query)
        {
            if (string.IsNullOrWhiteSpace(text)) return -1;
            if (string.IsNullOrWhiteSpace(query)) return -1;

            var t = text.ToLowerInvariant();
            var q = query.ToLowerInvariant();

            int score = 0;
            int tIndex = 0;
            int consecutive = 0;

            for (int i = 0; i < q.Length; i++)
            {
                char c = q[i];
                if (char.IsWhiteSpace(c)) continue;

                int found = t.IndexOf(c, tIndex);
                if (found < 0) return -1;

                // 连续命中更高分，越靠前越高分
                if (found == tIndex) consecutive++;
                else consecutive = 1;

                score += 2 + consecutive * 3;
                if (found == 0) score += 6;
                else
                {
                    char prev = t[found - 1];
                    if (prev == ' ' || prev == '_' || prev == '-' || prev == '/') score += 5;
                }

                tIndex = found + 1;
            }

            // 更短的跨度略加分
            score += Math.Max(0, 16 - (tIndex - q.Length));
            return score;
        }
    }
}

