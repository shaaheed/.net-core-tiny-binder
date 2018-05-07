using Msi.TinyBinder;
using System;

namespace TinyBinderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var binder = new TinyBinder();

            // Transform xml into PersonModel
            var a = typeof(Program).Assembly.FullName;
            string xmlPath = @"E:\DotNet\github\TinyBinder\TinyBinder\XmlTemplates\person.xml";
            var model = binder.BindXmlFromFile<PersonXmlModel>(xmlPath);

            // Bind content with model
            var data = new PersonModel { Hello = "Shahidul Islam", Email = "shahidcse6@gmail.com" };
            var details = binder.BindContent(model.Details, data, new string[] { "Email" });

            Console.ReadLine();
        }
    }
}
