using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Msi.TinyBinder
{
    public class TinyBinder
    {

        private static char WILD_CARD_CHAR = '%';
        private static char EOF_CHAR = '\uffff';

        public string BindContentFromFile(string path, object model, string[] ignoreProperties = null)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                return BindContent(content, model, ignoreProperties);
            }
            return null;
        }

        /// <summary>
        /// Place wildcards in content and bind them with model's property. Wildcards are looks like %Name%.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BindContent(string content, object model, string[] ignoreProperties = null)
        {
            return BindContentFromReader(new StringReader(content), model, ignoreProperties);
        }

        /// <summary>
        /// Transform xml content into a given model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public TModel BindXml<TModel>(string xmlContent)
        {
            return BindXmlFromReader<TModel>(new StringReader(xmlContent));
        }

        public TModel BindXmlFromFile<TModel>(string xmlPath)
        {
            if (File.Exists(xmlPath))
            {
                return BindXmlFromReader<TModel>(new StringReader(File.ReadAllText(xmlPath)));
            }
            return default(TModel);
        }

        private TModel BindXmlFromReader<TModel>(StringReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TModel));
            TModel model = (TModel)serializer.Deserialize(reader);
            reader.Dispose();
            return model;
        }

        private string BindContentFromReader(StringReader reader, object model, string[] ignoreProperties = null)
        {
            Dictionary<string, string> propertyValue = GetPropertiesAndValues(model, ignoreProperties);
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter())
            {
                char c;
                bool wildCardWasRead = false;
                while (true)
                {
                    c = (char)reader.Read();
                    if (c == EOF_CHAR)
                    {
                        // end of file!
                        break;
                    }
                    if (c == WILD_CARD_CHAR && wildCardWasRead)
                    {
                        // wild card read complete!
                        wildCardWasRead = false;
                        string wildCard = builder.ToString();
                        if (propertyValue.ContainsKey(wildCard))
                        {
                            object value = propertyValue[wildCard];
                            if (value != null)
                            {
                                writer.Write(value.ToString());
                            }
                        }
                        builder.Length = 0;
                        continue;
                    }
                    if (wildCardWasRead)
                    {
                        builder.Append(c);
                    }
                    if (c != WILD_CARD_CHAR && !wildCardWasRead)
                    {
                        writer.Write(c);
                    }
                    if (c == WILD_CARD_CHAR && !wildCardWasRead)
                    {
                        wildCardWasRead = true;
                    }
                }
                reader.Dispose();
                builder.Clear();
                return writer.ToString();
            }
        }

        private Dictionary<string, string> GetPropertiesAndValues(object model, string[] ignoreProperties = null)
        {
            var properties = model.GetType().GetProperties();
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                var customAttribute = property.GetCustomAttribute<WildCardAttribute>();
                var key = customAttribute == null || string.IsNullOrWhiteSpace(customAttribute.WildCard) ? property.Name : customAttribute.WildCard;
                if (ignoreProperties == null || !ignoreProperties.Contains(property.Name))
                {
                    var value = property.GetValue(model, null);
                    if (value != null)
                    {
                        values.Add(key, value.ToString());
                    }
                }
            }
            return values;
        }

    }
}
