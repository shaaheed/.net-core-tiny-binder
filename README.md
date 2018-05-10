# .net-core-tiny-binder
A super tiny binder that bind content's %wildcard% with model's property.

##### Content binding with model

```c#

string content = "Person Name: %Name%, Person Email: %Email%";

var model = new PersonModel {
  Name = "Shahidul Islam",
  Email = "shahidcse6@gmail.com"
};

var binder = new TinyBinder();
var result = binder.BindContent(content, model);

// Output: Person Name: Shahidul Islam, Person Email: shahidcse6@gmail.com

```
