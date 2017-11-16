using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class CreateEntity : CreateInputModel
    {
        public CreateEntity(String args)
            :base(args)
        {
            
        }

        public override string StartType(string name, string pluralName)
        {
            return 
$@"        public static {name}Entity CreateEntity(String seed = """", Guid? {name}Id = null{args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : Guid.NewGuid(),";
        }
    }
}
