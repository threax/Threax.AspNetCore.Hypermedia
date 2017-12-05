using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class CreateViewModel : CreateInputModel
    {
        private Type modelIdType;

        public CreateViewModel(String args, Type modelIdType)
            :base(args)
        {
            this.modelIdType = modelIdType;
        }

        public override void StartType(StringBuilder sb, string name, string pluralName)
        {
            if (modelIdType == typeof(Guid))
            {
                sb.AppendLine(
$@"        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : Guid.NewGuid(),"
                );
            }
            else if (modelIdType.IsNumeric())
            {
                sb.AppendLine(
$@"        private static {modelIdType.Name} currentViewModelId = 0;
        private static Object viewModelIdLock = new Object();
        private static {modelIdType.Name} GetNextId()
        {{
            lock(viewModelIdLock)
            {{
                if(currentViewModelId == {modelIdType.Name}.MaxValue)
                {{
                    throw new InvalidOperationException(""Ran out of key values for {name} entities suggest modifying your tests to create keys manually."");
                }}
                return currentViewModelId++;
            }}
        }}

        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : GetNextId(),"
                );
            }
            else if (modelIdType == typeof(String))
            {
                sb.AppendLine(
$@"        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {name}Id = {name}Id != null ? {name}Id : seed + Guid.NewGuid().ToString(),"
                );
            }
            else //Some other unknown type, likely an enum, won't be able to generate test values for this
            {
                sb.AppendLine(
$@"        public static {name} CreateView({modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}), String seed = """"{args})
        {{
            return new {name}()
            {{
                {name}Id = {name}Id != null ? ({modelIdType.Name}){name}Id : default({modelIdType.Name}),"
                );
            }
        }
    }
}
