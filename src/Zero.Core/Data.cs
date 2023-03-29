// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

namespace Zero.Core
{
    public class Data
    {
        // Zero.BUILD-INFO
        public int buildYear = 2023;

        public int majorV = 2;

        public int minorV = 0;

        public int build = 128;

        public string getVersion()
        {
            string version = "v" + majorV + "." + minorV + " Build " + build;
            return version;
        }

        public string SqlLocalDBurl = "https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi";
    }
}
