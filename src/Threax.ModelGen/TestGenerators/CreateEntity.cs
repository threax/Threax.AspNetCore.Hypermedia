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
$@"        private static {modelIdType.Name} currentEntityId = 0;
        private static Object entityIdLock = new Object();
        private static {modelIdType.Name} GetNextEntityId()
        {{
            lock(entityIdLock)
            {{
                if(currentEntityId == {modelIdType.Name}.MaxValue)
                {{
                    throw new InvalidOperationException(""Ran out of key values for {name} entities suggest modifying your tests to create keys manually."");
                }}
                return currentEntityId++;
            }}
        }}

        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id.HasValue ? {name}Id.Value : GetNextId(),"
                );
            }
            else if(modelIdType == typeof(String))
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity(String seed = """", {modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}){args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id != null ? {name}Id : seed + Guid.NewGuid().ToString(),"
                );
            }
            else //Some other unknown type, likely an enum
            {
                sb.AppendLine(
$@"        public static {name}Entity CreateEntity({modelIdType.GetTypeAsNullable()} {name}Id = default({modelIdType.GetTypeAsNullable()}), String seed = """"{args})
        {{
            return new {name}Entity()
            {{
                {name}Id = {name}Id != null ? ({modelIdType.Name}){name}Id : default({modelIdType.Name}),"
                );
            }
        }
    }
}
