using System;
using System.Collections;
using System.Collections.Generic;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class MockDbSetTests
    {
        [Fact]
        public async void AddAsyncMustAddEntityAndReturnEntityEntryWhenCalled()
        {
            StubMockDbSet<Person> persons = new StubMockDbSet<Person>();
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            EntityEntry<Person> entry = await persons.AddAsync(person);
            Assert.Equal(person.Id, entry.Entity.Id);
            Assert.Contains(persons.EntitiesGetter, x => x.Id == person.Id);
        }

        [Fact]
        public void AddMustAddEntityAndReturnEntityEntryWhenCalled()
        {
            StubMockDbSet<Person> persons = new StubMockDbSet<Person>();
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            EntityEntry<Person> entry = persons.Add(person);
            Assert.Equal(person.Id, entry.Entity.Id);
            Assert.Contains(persons.EntitiesGetter, x => x.Id == person.Id);
        }

        [Fact]
        public void ConstructorMustInitializeExpressionWhenCalled()
        {
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            StubMockDbSet<Person> persons = new StubMockDbSet<Person>(new[] {person});
            Assert.NotNull(persons.Expression);
            Assert.NotNull(persons.Provider);
            Assert.Contains(persons.Provider.CreateQuery<Person>(persons.Expression), x => x.Id == person.Id);
        }

        [Fact]
        public void ConstructorMustSetEntitiesToEmptyListWhenEntitiesIsNull()
        {
            Assert.Empty(new StubMockDbSet<Person>().EntitiesGetter);
        }

        [Fact]
        public void ConstructorMustSetEntitiesToListContainingEntitiesWhenEntitiesIsNotNull()
        {
            Assert.Contains(new StubMockDbSet<Person>(new[] {new Person {FirstName = "Bob", LastName = "Smitih"}}).EntitiesGetter, x => x.FirstName == "Bob");
        }

        [Fact]
        public void ConstructorMustSetPredicateToIdEqualityWhenPredicateIsNull()
        {
            Person person = new Person();
            Assert.True(new StubMockDbSet<Person>().PredicateGetter(person, new object[] {person.Id}));
        }

        [Fact]
        public void ConstructorMustSetPredicateToPredicateWhenPredicateIsNotNull()
        {
            Person person = new Person();
            Assert.True(new StubMockDbSet<Person>(null, (x, y) => Equals(x.LastName, y[0])).PredicateGetter(person, new object[] {person.LastName}));
        }

        [Fact]
        public void FindMustReturnEntityWhenPredicateReturnsTrue()
        {
            Person expected = new Person {FirstName = "Bob", LastName = "Smith"};
            MockDbSet<Person> persons = new MockDbSet<Person>(new[] {expected});
            Person actual = persons.Find(expected.Id);
            Assert.NotNull(actual);
            Assert.Equal(expected.FirstName, actual.FirstName);
        }

        [Fact]
        public void FindMustReturnNullWhenPredicateReturnsFalseForAllEntities()
        {
            MockDbSet<Person> persons = new MockDbSet<Person>(new[] {new Person {FirstName = "Bob", LastName = "Smith"}});
            Assert.Null(persons.Find(Guid.NewGuid()));
        }

        [Fact]
        public void GetEnumeratorMustReturnEntitiesEnumeratorWhenCalled()
        {
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            Assert.Contains(new MockDbSet<Person>(new[] {person}), x => x.Id == person.Id);
        }

        [Fact]
        public void GetEnumeratorMustReturnEnumeratorWhenCalled()
        {
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            IEnumerator enumerator = ((IEnumerable) new MockDbSet<Person>(new[] {person})).GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(person.Id, ((Person) enumerator.Current)?.Id);
        }

        [Fact]
        public void RemoveMustRemoveEntityAndReturnEntityEntryWhenCalled()
        {
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            MockDbSet<Person> persons = new MockDbSet<Person>(new[] {person});
            Assert.Contains(persons, x => x.Id == person.Id);
            EntityEntry<Person> entry = persons.Remove(person);
            Assert.Equal(person.Id, entry.Entity.Id);
            Assert.DoesNotContain(persons, x => x.Id == person.Id);
        }

        [Fact]
        public void RemoveRangeMustRemoveEntitiesWhenCalled()
        {
            Person person = new Person {FirstName = "Bob", LastName = "Smith"};
            MockDbSet<Person> persons = new MockDbSet<Person>(new[] {person});
            Assert.Contains(persons, x => x.Id == person.Id);
            persons.RemoveRange(person);
            Assert.DoesNotContain(persons, x => x.Id == person.Id);
        }

        private class Person
        {
            public string FirstName { get; set; }

            public Guid Id { get; protected set; } = Guid.NewGuid();

            public string LastName { get; set; }
        }

        private class StubMockDbSet<T> : MockDbSet<T> where T : class
        {
            public StubMockDbSet(IEnumerable<T> entities = null, Func<T, object[], bool> predicate = null) : base(entities, predicate)
            {
            }

            public List<T> EntitiesGetter => Entities;

            public Func<T, object[], bool> PredicateGetter => Predicate;
        }
    }
}