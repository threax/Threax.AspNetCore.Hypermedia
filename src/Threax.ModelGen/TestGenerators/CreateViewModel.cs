using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class CreateViewModel : CreateInputModel
    {
        public CreateViewModel(String args)
            :base(args)
        {
            
        }

        public override void StartType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine( 
$@"        public static {name} CreateView(String seed = """", Guid? {name}Id = null{args})
        {{
            return new {name}()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : Guid.NewGuid(),"
            );
        }
    }
}
