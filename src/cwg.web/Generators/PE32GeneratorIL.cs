﻿namespace cwg.web.Generators
{
    public class PE32PlusILGenerator : BaseGenerator
    {
        public override string Name => "PE32+ (IL)";

        public override bool Active => false;

        protected override string SourceName => "sourcePE+IL.exe";
        
        protected override string CleanSourceName => "sourceCleanPE32.exe";

        protected override string OutputExtension => "exe";

        public override bool Packable => false;
    }
}