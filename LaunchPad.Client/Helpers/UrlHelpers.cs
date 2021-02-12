using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaunchPad.Client.Helpers
{
    public static class UrlHelpers
    {
        /// <summary>
        /// TODO:  Loop through IEnumerables and add to Properties
        /// </summary>
        /// <param name="object"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToQueryString(this object @object, string separator = ",")
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            // Get all properties on the object
            var properties = @object.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(@object, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(@object, null));

            // Get names for all IEnumerable properties (excl. string)
            var enumProperties = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Unwrap IEnumerable properties and add to seperate List (to allow for duplicate keys)
            var enumProperties2 = new List<DictionaryList>();
            foreach (var key in enumProperties)
            {
                var enumerable = properties[key] as IEnumerable;

                foreach (var enume in enumerable)
                {
                    enumProperties2.Add(new DictionaryList()
                    {
                        Key = key,
                        Value = enume
                    });
                }
            }

            // Remove enums from properties
            var enums2 = properties.Keys.Intersect(enumProperties).ToList();
            if (enums2.Count > 0)
            {
                foreach (var property in enums2)
                {
                    properties.Remove(property);
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            var props = string.Join("&", properties
                    .Select(x => string.Concat(
                        Uri.EscapeDataString(x.Key), "=",
                        Uri.EscapeDataString(x.Value.ToString()))));

            var enums = string.Join("&", enumProperties2
                    .Select(x => string.Concat(
                        Uri.EscapeDataString(x.Key), "=",
                        Uri.EscapeDataString(x.Value.ToString()))));

            if (!string.IsNullOrEmpty(enums))
            {
                props = props + "&" + enums;
            }

            return props;
        }
    }

    public class DictionaryList
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
