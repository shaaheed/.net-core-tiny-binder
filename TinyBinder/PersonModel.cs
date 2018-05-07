using Msi.TinyBinder;

namespace TinyBinderExample
{
    public class PersonModel
    {
        [WildCard("Name")]
        public string Hello { get; set; }
        public string Email { get; set; }
    }
}
