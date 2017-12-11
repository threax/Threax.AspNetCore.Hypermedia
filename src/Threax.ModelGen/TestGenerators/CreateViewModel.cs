using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    class CreateViewModel : CreateInputModel
    {
        private Type modelIdType;
        private String ModelId;

        public CreateViewModel(JsonSchema4 schema, String args)
            :base(args)
        {
            this.modelIdType = schema.GetKeyType();
            this.ModelId = NameGenerator.CreatePascal(schema.GetKeyName());
        }

        public override void StartType(StringBuilder sb, string name, string pluralName)
        {
            if (modelIdType == typeof(Guid))
            {
                sb.AppendLine(
$@"        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {ModelId} = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {ModelId} = {ModelId}.HasValue ? {ModelId}.Value : Guid.NewGuid(),"
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

        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {ModelId} = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {ModelId} = {ModelId}.HasValue ? {ModelId}.Value : GetNextId(),"
                );
            }
            else if (modelIdType == typeof(String))
            {
                sb.AppendLine(
$@"        public static {name} CreateView(String seed = """", {modelIdType.GetTypeAsNullable()} {ModelId} = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}()
            {{
                {ModelId} = {ModelId} != null ? {ModelId} : seed + Guid.NewGuid().ToString(),"
                );
            }
            else //Some other unknown type, likely an enum, won't be able to generate test values for this
            {
                sb.AppendLine(
$@"        public static {name} CreateView({modelIdType.GetTypeAsNullable()} {ModelId} = default({modelIdType.GetTypeAsNullable()}), String seed = """"{args})
        {{
            return new {name}()
            {{
                {ModelId} = {ModelId} != null ? ({modelIdType.Name}){ModelId} : default({modelIdType.Name}),"
                );
            }
        }
    }
}
