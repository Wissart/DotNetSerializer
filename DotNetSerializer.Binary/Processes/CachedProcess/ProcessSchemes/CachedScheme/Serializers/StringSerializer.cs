using DotNetSerializer.Binary.Converters.Default;
using System.Reflection;
using System.Text;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Serializers
{
    internal abstract class StringSerializer : TypeSerializer
    {
        protected readonly Encoding _encoding;
        protected readonly StringConverter _converter;
        public StringSerializer(PropertyInfo property, StringConverter converter, string encodingName) : base(property)
        {
            _converter = converter;
            _encoding = Encoding.GetEncoding(encodingName);
        }
    }
}
