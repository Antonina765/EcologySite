using System.Reflection;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.VisualBasic;
using ReflectionExample;

//var user = new User();

//var type = typeof(User);
//var typeFromObj = user.GetType();

//var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

//foreach (var field in fields)
//{
//    Console.WriteLine(field.Name);
//}

//var ageFieldInfo = type
//    .GetField("_age", BindingFlags.NonPublic | BindingFlags.Instance);
//var value = ageFieldInfo
//    .GetValue(user);

//Console.WriteLine($"use age: {value}");

//ageFieldInfo.SetValue(user, 33);
//user.SayYouAge();


var baseRepositoryClassType = typeof(BaseRepository<>);
var baseRepositoryInterfaceType = typeof(IBaseRepository<>);
var baseRepositoryWithBrandType = typeof(BaseRepository<CommentData>);
var assembly = Assembly.GetAssembly(baseRepositoryClassType);

var repositoryInterfaces = assembly
    .GetTypes()
    .Where(t => t.IsInterface // ICakeRepositoryReal
                && t.GetInterfaces()
                    .Any(i => //ICakeRepository<CakeData>
                        i.GetInterfaces()
                            .Any(i2 => i2.IsGenericType 
                                       && i2.GetGenericTypeDefinition() == baseRepositoryInterfaceType)
                    )
    );

var repositoryClasses = assembly
    .GetTypes()
    .Where(t => t.IsClass
                && t.BaseType != null
                && t.BaseType.IsGenericType
                && t.BaseType.GetGenericTypeDefinition() == baseRepositoryClassType);

foreach (var interfaceFromData in repositoryInterfaces)
{
    var clasType = repositoryClasses
        .FirstOrDefault(c => c.GetInterfaces().Any(i => i == interfaceFromData));
    if (clasType == null)
    {
        Console.WriteLine($"!!! {interfaceFromData.Name} => NO CLASSES");
    }
    else
    {

        Console.WriteLine($"{interfaceFromData.Name} => {clasType.Name}");
    }
}