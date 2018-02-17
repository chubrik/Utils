﻿using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kit {
    public class PathHelper {

        public static string Combine(params string[] paths) {
            var result = Path.Combine(paths).Replace(@"\", "/");

            while (result.Contains("../")) {

                if (result.StartsWith("../"))
                    return result;

                result = Regex.Replace(result, @"(?<=^|/)[^/]+/\.\./", "");
            }

            return result;
        }

        public static string Parent(string path) =>
            path.Contains('/') ? path.Substring(0, path.LastIndexOf('/')) : string.Empty;

        public static string SafeFileName(string fileName) {
            var safeName = fileName.Replace('\"', '\'');
            safeName = Regex.Replace(safeName, @"[^a-zа-яё0-9.,()'# -]", "_", RegexOptions.IgnoreCase);
            return safeName;
        }
    }
}