using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    /// <summary>
    /// This class makes it easy to test the client gen in an integration test.
    /// Pass it the IClientGenerator from your system and it will make sure all
    /// of the code can be generated correctly, which validates your hypermedia api.
    /// </summary>
    public class ClientGenTester
    {
        private IClientGenerator clientGenerator;

        public ClientGenTester(IClientGenerator clientGenerator)
        {
            this.clientGenerator = clientGenerator;
        }

        /// <summary>
        /// Call this function to verify the client generator. It will throw an exception with more information
        /// if there are any errors, otherwise it will appear to do nothing, which means the tests pass.
        /// </summary>
        public void Verify()
        {
            var list = this.clientGenerator.GetEndpointDefinitions().ToList();
        }
    }
}
