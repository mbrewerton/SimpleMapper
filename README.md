# SimpleMapper

SimpleMapper is a simple object mapper for mapping C# objects between classes. It's designed to be extremely easy and fast to use with customisation on additional mapping. It will automatically map properties with a matching name and type. If the type is different, it will not map the properties - these will need to be done as a custom Action in the `additionalMappings` params.

SimpleMapper uses an internal memory cache to help speed up subsequent mappings of the same class combinations.

## How to Use

### Map<TInput, TOutput>()

The `Map` method is used for mapping a single object between two C# classes.

#### Example 1 - Basic Mapping:

```c#
var model = new SimpleModel
{
    Id = 1,
    Date = DateTime.Now,
    Decimal = 1,
    Name = "Item 1"
};

var output = model.Map<SimpleModel, SimpleOutput>(); // output is typeof SimpleOutput
```

#### Example 2: Basic Mapping with Custom Map
```c#
class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public void MapPerson()
{
    var model = new Person
    {
        Id = 1,
        FirstName = "Matt",
        LastName = "Brewerton"
    }
    
    var output = model.Map<Person, PersonDto>(
        (person, dto) => {
            // Perform your mapping in Actions. You can pass multiple Actions to the params which run synchronously. 
            dto.FullName = $"{person.FirstName} {person.LastName}";
         }
     );
}
```

### MapCollection<TInput, TOutput>()
The `MapCollection` method uses `Map()` under the hood but allows you to explicitly map a collection of classes. Usage is exactly the same, but it takes an `IEnumerable<TInput>` to map from, and returns an `IEnumerable<TOutput>`.

#### Example 1 - Basic Mapping:

```c#
var models = new SimpleModel[]
{
    new SimpleModel
    {
        Id = 1,
        Date = DateTime.Now,
        Decimal = 1,
        Name = "Item 1"
    },
    new SimpleModel
    {
        Id = 2,
        Date = DateTime.Now,
        Decimal = 2,
        Name = "Item 2"
    }
};

var output = model.MapCollection<SimpleModel, SimpleOutput>(); // output is typeof IEnumerable<SimpleOutput>.
```

#### Example 2: Basic Mapping with Custom Map
```c#
class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public void MapPerson()
{
    var people = new Person[]
    {
        new Person
        {
            Id = 1,
            FirstName = "Matt",
            LastName = "Brewerton"
        },
        new Person
        {
            Id = 2,
            FirstName = "Chris
            LastName = "Hodges"
        }
    }
    
    var output = model.MapCollection<Person, PersonDto>(
        (person, dto) => {
            // Perform your mapping in Actions. You can pass multiple Actions to the params which run synchronously.
            // NOTE: These are run for each object in the collection.
            dto.FullName = $"{person.FirstName} {person.LastName}";
         }
     );
}
```