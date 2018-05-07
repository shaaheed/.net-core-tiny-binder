using System;

namespace Msi.TinyBinder
{
    public class WildCardAttribute : Attribute
    {

        public string WildCard { get; set; }

        public WildCardAttribute(string wildCard)
        {
            WildCard = wildCard;
        }

    }
}
