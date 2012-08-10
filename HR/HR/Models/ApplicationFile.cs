using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Microsoft.Win32;
using System.IO;

namespace HR {
    partial class ApplicationFile {

        public bool isTypeAllowed() {
            List<string> allowed = new List<string> { "pdf", "doc", "docx", "rtf", "txt", "odt" };
            string extension = Path.GetExtension(this.fileName).Substring(1);
            return allowed.Contains(extension);
        }

        public bool isSizeAllowed() {
            int limit = 3145728; // 3 Megabytes
            return (this.size <= limit);
        }

        public string GetMimeType() {
            string mimetype = "";
            string extension = Path.GetExtension(this.fileName).Substring(1);
            mimetype = GetMimeTypeFromExtension(extension);

            return mimetype;
        }

        private string GetMimeTypeFromExtension(string extension) {
            string result;
            RegistryKey key;
            object value;

            if (!extension.StartsWith("."))
                extension = "." + extension;

            key = Registry.ClassesRoot.OpenSubKey(extension, false);
            value = key != null ? key.GetValue("Content Type", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }
    }
}