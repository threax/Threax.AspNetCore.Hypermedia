using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class CreateEntity : CreateInputModel
    {
        private Type modelIdType;

        public CreateEntity(String args, Type modelIdType)
            :base(args)
        {
            this.modelIdType = modelIdType;
        }

        public override void StartType(StringBuilder sb, string name, string pluralName)
        {
            if (modelIdType == typeof(Guid))
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : Guid.NewGuid(),"
                );
            }
            else if(modelIdType.IsNumeric())
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : new Random().Next(0, {modelIdType.Name}.MaxValue),"
                );
            }
            else if(modelIdType == typeof(String))
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : seed + ""id"","
                );
            }
            else //Some other unknown type, likely an enum
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : default({modelIdType.Name}),"
                );
            }
        }
    }
}
