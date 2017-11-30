using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class ViewModelWriter : ClassWriter
    {
        public ViewModelWriter(bool hasCreated, bool hasModified) : base(hasCreated, hasModified)
        {
        }

        public override string AddUsings(string ns)
        {
            return $@"{base.AddUsings(ns)}
using {ns}.Models;
using {ns}.Controllers.Api;";
        }

        public override String StartType(String name, String pluralName)
        {
            return 
$@"    [HalModel]
    [HalSelfActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Get))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Update))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Delete))]
    public partial class {name} : I{name}, I{name}Id {GetAdditionalInterfaces()}
    {{
{CreateProperty($"{name}Id", new TypeWriterPropertyInfo<Guid>())}";
        }

        public override string AddMaxLength(int length, string errorMessage)
        {
            return "";
        }

        public override string AddRequired(string errorMessage)
        {
            return "";
        }

        public override string CreateProperty(string name, IWriterPropertyInfo info)
        {
            return
                $@"        [UiOrder]
{base.CreateProperty(name, info)}";
        }
    }
}
