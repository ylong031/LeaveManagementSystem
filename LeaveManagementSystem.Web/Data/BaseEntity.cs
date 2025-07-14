using Humanizer;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace LeaveManagementSystem.Web.Data
{

    /*
    It refers to the process of hiding complex implementation details 
    and showing only the essential features of an object or system.
    
    An abstract class can contain:
    
    Abstract methods: Methods without an implementation(just the signature).
    These methods must be overrideden in the child class.
    
    Abstract class cannot be initiated

    Inheritance is required.You must inherit from an abstract class to use it.
    */
    public abstract class BaseEntity
    { 
        public int Id { get; set; } 
    
    }
}



//Polymorphism is an object-oriented programming principle that means "many forms." 
//It allows you to use a base class variable(Animal) to refer to objects of different derived classes (Dog, Cat, etc.).

/*
"many forms" in polymorphism does refer to the ability of different classes 
to provide different implementations (often through overridden methods) 
for the same method call.

/*public class Animal
{
    public virtual void MakeSound() { }
}

public class Dog : Animal
{
    public override void MakeSound() { Console.WriteLine("Woof!"); }
}

public class Cat : Animal
{
    public override void MakeSound() { Console.WriteLine("Meow!"); }
}

List<Animal> animals = new List<Animal> { new Dog(), new Cat() };
foreach (Animal a in animals)
{
    a.MakeSound(); // Calls the correct MakeSound() for Dog or Cat
}*/