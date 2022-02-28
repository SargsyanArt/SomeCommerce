using System.Collections.Generic;

namespace SomeCommerce.Web.Configuration
{
    public static class Dropdown
    {
        public const int DefaultCapacity = 10;

        /// <summary>
        /// Used to load select2 options via AJAX call
        /// </summary>
        public class Option
        {
            public Option(string id, string text)
            {
                Id = id;
                Text = text;
            }

            public string Id { get; set; }

            public string Text { get; set; }
        }

        public struct OptGroup
        {
            public OptGroup(string text, IEnumerable<Option> children)
            {
                Text = text;
                Children = children;
            }

            public string Text { get; set; }

            public IEnumerable<Option> Children { get; set; }
        }

        public struct OptGroupCount
        {
            public OptGroupCount(string text, IEnumerable<Option> children, int count)
            {
                Text = text;
                Children = children;
                Count = count;
            }

            public string Text { get; set; }

            public IEnumerable<Option> Children { get; set; }
            public int Count { get; set; }
        }
    }
}
