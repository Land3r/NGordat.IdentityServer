using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.Dal.Migrations
{
    public class Options
    {
        [Option('m', "migrate", Required = false, HelpText = "Migrate all the databases contexts used by the IdentityServer infrastructure.")]
        public bool Migrate { get; set; }

        [Option('s', "seed", Required = false, HelpText = "Seed the databases with the specified data.")]
        public bool Seed { get; set; }
    }
}
