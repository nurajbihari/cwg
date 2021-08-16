﻿using System;
using System.IO;
using System.Text;

using cwg.web.Data;

namespace cwg.web.Generators
{
    public class HTAPEEmbeddedController : BaseGenerator
    {
        public override string Name => "HTA (PE32 Embedded)";

        protected override string SourceName => "sourceHTA";

        protected override string CleanSourceName => "sourceHTAs";

        protected override string OutputExtension => "hta";

        public override bool Packable => false;

        public override bool Active => false;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var sourceFile = File.ReadAllText(SourceName);

            var exe = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "sourcePE+IL.exe"));

            var exeString = System.Convert.ToBase64String(exe);

            sourceFile = sourceFile.Replace("$TIMESTAMP", $"Own3d on {DateTime.Now}");

            sourceFile = sourceFile.Replace("$BASE64PAYLOAD", exeString);
            
            var bytes = Encoding.ASCII.GetBytes(sourceFile);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.{OutputExtension}"), bytes);

            return (sha1Sum, $"{sha1Sum}.{OutputExtension}");
        }
    }
}