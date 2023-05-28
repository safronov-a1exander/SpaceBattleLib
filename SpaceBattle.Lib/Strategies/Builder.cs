namespace SpaceBattle.Lib;
using Scriban;

using System.Reflection;

public class Builder : IBuilder
{
    List<object> fields;
    public Builder()
    {
        fields = new();
    }
    public object Add(object arg)
    {
        fields.Add(arg);
        return this;
    }
    public string Build()
    {
        var target = (Type)this.fields[0];
        PropertyInfo[] properties = target.GetProperties();

        var rawstring = @"
        namespace {{target.namespace}};
        public class {{target.name}}Adapter : {{target.name}}
        {
            Dictionary<string, object> target;
            public {{target.name}}Adapter(object _target){target = (Dictionary<string, object>) _target;}
            {{for property in properties}}
            public {{property.property_type}} {{property.name}}
            {{if property.get_method}}
            {
                get => ({{property.property_type}}) target[""{{property.name}}""];
            {{end}}
            {{if property.set_method}}
                set => target[""{{property.name}}""] = value;
            {{end}}
            }
            {{end}}
        }
        ";
        rawstring = rawstring.Replace(Environment.NewLine, "");
        rawstring = rawstring.Replace("    ", "");
        var tpl = Template.Parse(rawstring);
        string result = tpl.Render(new {target, properties});
        return result;
    }
}
