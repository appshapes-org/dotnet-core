using System;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Core
{
    public class ReflectionHelperTests
    {
        [Fact]
        public void GetFieldFromDerivedTypeMustReturnFieldWhenFieldExists()
        {
            Actor source = new Actor("Bob", "Smith", "Bobby");
            Assert.Equal(source.FirstName, ReflectionHelper.GetField(typeof(Person), source, "itsFirstName"));
        }

        [Fact]
        public void GetFieldFromDerivedTypeMustThrowExceptionWhenFieldDoesNotExist()
        {
            Assert.Equal($"Can't find field 'itsMiddleName' in {typeof(Person)}", Assert.Throws<Exception>(() => ReflectionHelper.GetField(typeof(Person), new Actor(), "itsMiddleName")).Message);
        }

        [Fact]
        public void GetFieldMustReturnFieldWhenFieldExists()
        {
            Person source = new Person("Bob", "Smith");
            Assert.Equal(source.FirstName, ReflectionHelper.GetField(source, "itsFirstName"));
        }

        [Fact]
        public void GetFieldMustThrowExceptionWhenFieldDoesNotExist()
        {
            Person source = new Person();
            Assert.Equal($"Can't find field 'itsMiddleName' in {source}", Assert.Throws<Exception>(() => ReflectionHelper.GetField(source, "itsMiddleName")).Message);
        }

        [Fact]
        public void GetFieldOrDefaultFromDerivedTypeMustReturnDefaultWhenFieldDoesNotExist()
        {
            Assert.Equal(default, ReflectionHelper.GetFieldOrDefault(typeof(Person), new Actor("Bob", "Smith", "Bobby"), "itsMiddleName"));
        }

        [Fact]
        public void GetFieldOrDefaultFromDerivedTypeMustReturnFieldWhenFieldExists()
        {
            Assert.Equal("Bob", ReflectionHelper.GetFieldOrDefault(typeof(Person), new Actor("Bob", "Smith", "Bobby"), "itsFirstName"));
        }

        [Fact]
        public void GetFieldOrDefaultMustReturnDefaultWhenFieldDoesNotExist()
        {
            Assert.Equal(default, ReflectionHelper.GetFieldOrDefault(new Person(), "itsMiddleName"));
        }

        [Fact]
        public void GetFieldOrDefaultMustReturnFieldWhenFieldExists()
        {
            Assert.Equal("Bob", ReflectionHelper.GetFieldOrDefault(new Person("Bob", "Smith"), "itsFirstName"));
        }

        [Fact]
        public void GetPropertyGenericMustReturnPropertyWhenPropertyExists()
        {
            Assert.Equal("Bob", ReflectionHelper.GetProperty<string>(new Person("Bob", "Smith"), nameof(Person.FirstName)));
        }

        [Fact]
        public void GetPropertyMustReturnPropertyWhenPropertyExists()
        {
            Assert.Equal("Bob", ReflectionHelper.GetProperty(new Person("Bob", "Smith"), nameof(Person.FirstName)));
        }

        [Fact]
        public void InvokeConstructorGenericMustReturnInstanceOfTypeWhenConstructorExists()
        {
            Assert.Equal("Bob", ReflectionHelper.InvokeConstructor<Person>("Bob", "Smith").FirstName);
        }

        [Fact]
        public void InvokeConstructorMustReturnInstanceOfTypeWhenConstructorExists()
        {
            Assert.Equal("Bob", ((Person) ReflectionHelper.InvokeConstructor(typeof(Person), new[] {typeof(string), typeof(string)}, new object[] {"Bob", "Smith"})).FirstName);
        }

        [Fact]
        public void InvokeConstructorMustReturnNullWhenConstructorDoesNotExist()
        {
            Assert.Null(ReflectionHelper.InvokeConstructor(typeof(Person), new[] {typeof(string), typeof(string), typeof(string)}, new object[] {"Bob", "DoesNotExist", "Smith"}));
        }

        [Fact]
        public void InvokeConstructorOnDerivedTypeMustReturnInstanceOfTypeWhenDefaultConstructorExists()
        {
            Assert.Null(((Person) ReflectionHelper.InvokeConstructor(typeof(Person), null, new object[] { })).FirstName);
        }

        [Fact]
        public void InvokeMethodMustInvokeMethodWhenMethodExists()
        {
            Assert.Equal("Bob Smith", ReflectionHelper.InvokeMethod(new Person("Bob", "Smith"), "GetFullName"));
        }

        [Fact]
        public void InvokeMethodOnDerivedTypeMustInvokeMethodWhenMethodExists()
        {
            Assert.Equal("Bobby", ReflectionHelper.InvokeMethod(typeof(Person), new Actor("Bob", "Smith", "Bobby"), "GetFullName", new object[] { }));
        }

        [Fact]
        public void SetFieldMustSetFieldWhenFieldExists()
        {
            Person source = new Person();
            ReflectionHelper.SetField(source, "itsFirstName", "Bob");
            Assert.Equal("Bob", source.FirstName);
        }

        [Fact]
        public void SetFieldMustThrowExceptionWhenFieldDoesNotExist()
        {
            Person source = new Person();
            Assert.Equal($"Can't find field 'itsMiddleName' in {source}", Assert.Throws<Exception>(() => ReflectionHelper.SetField(source, "itsMiddleName", "Joe")).Message);
        }

        [Fact]
        public void SetFieldOnDerivedTypeMustSetFieldWhenFieldExists()
        {
            Actor source = new Actor();
            ReflectionHelper.SetField(typeof(Person), source, "itsFirstName", "Bob");
            Assert.Equal("Bob", source.FirstName);
        }

        [Fact]
        public void SetFieldOnDerivedTypeMustThrowExceptionWhenFieldDoesNotExist()
        {
            Assert.Equal($"Can't find field 'itsMiddleName' in {typeof(Person)}", Assert.Throws<Exception>(() => ReflectionHelper.SetField(typeof(Person), new Actor(), "itsMiddleName", "Joe")).Message);
        }

        [Fact]
        public void SetPropertyMustSetPropertyWhenPropertyExists()
        {
            Person source = new Person();
            ReflectionHelper.SetProperty(source, nameof(Person.FirstName), "Bob");
            Assert.Equal("Bob", source.FirstName);
        }

        private class Actor : Person
        {
            public Actor()
            {
            }

            public Actor(string firstName, string lastName, string stageName) : base(firstName, lastName)
            {
                StageName = stageName;
            }

            public string StageName { get; set; }

            protected override string GetFullName()
            {
                return StageName;
            }
        }

        private class Person
        {
            private string itsFirstName;
            private string itsLastName;

            public Person()
            {
            }

            public Person(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public string FirstName { get => itsFirstName; set => itsFirstName = value; }

            public string LastName { get => itsLastName; set => itsLastName = value; }

            protected virtual string GetFullName()
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}