using System;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Local
namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class ModelHelperTests
    {
        [Fact]
        public void GetIndexMustReturnIndexWhenIndexExists()
        {
            using PersonContext context = new PersonContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            Assert.NotNull(new ModelHelper(builder.Model).GetIndex<Person>(nameof(Person.LegacyId)));
        }

        [Fact]
        public void GetIndexMustThrowExceptionWhenEntityDoesNotExist()
        {
            using DbContext context = new DbContext(new DbContextOptions<DbContext>());
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            ArgumentException exception = Assert.Throws<ArgumentException>(() => GetIndex<Person>(builder, nameof(Person.Id)));
            Assert.Equal("Collection does not contain expected value.", exception.Message);
        }

        [Fact]
        public void GetIndexMustThrowExceptionWhenIndexDoesNotExist()
        {
            using GroupContext context = new GroupContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => GetIndex<Group>(builder, "Id"));
            Assert.Equal("Value cannot be null. (Parameter 'actual')", exception.Message);
        }

        [Fact]
        public void GetPropertyMustReturnPropertyWhenPropertyExists()
        {
            using PersonContext context = new PersonContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            Assert.NotNull(new ModelHelper(builder.Model).GetProperty<Person>(nameof(Person.LegacyId)));
        }

        [Fact]
        public void GetPropertyMustThrowExceptionWhenEntityDoesNotExist()
        {
            using DbContext context = new DbContext(new DbContextOptions<DbContext>());
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            ArgumentException exception = Assert.Throws<ArgumentException>(() => GetProperty<Group>(builder, "Id"));
            Assert.Equal("Collection does not contain expected value.", exception.Message);
        }

        [Fact]
        public void GetPropertyMustThrowExceptionWhenPropertyDoesNotExist()
        {
            using GroupContext context = new GroupContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            ArgumentException exception = Assert.Throws<ArgumentException>(() => GetProperty<Group>(builder, "Id"));
            Assert.Equal("Collection does not contain expected value.", exception.Message);
        }

        private Index GetIndex<T>(ModelBuilder builder, string name)
        {
            return new ModelHelper(builder.Model).GetIndex<T>(name);
        }

        private Property GetProperty<T>(ModelBuilder builder, string name)
        {
            return new ModelHelper(builder.Model).GetProperty<T>(name);
        }

        private class Group
        {
        }

        private class GroupContext : DbContext
        {
            public void InvokeOnModelCreating(ModelBuilder builder)
            {
                OnModelCreating(builder);
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.Entity<Group>();
            }
        }

        private class Person
        {
            public Guid Id { get; protected set; } = Guid.NewGuid();

            public int LegacyId { get; set; }
        }

        private class PersonContext : DbContext
        {
            public void InvokeOnModelCreating(ModelBuilder builder)
            {
                OnModelCreating(builder);
            }

            public DbSet<Person> Persons { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.Entity<Person>().HasIndex(x => x.LegacyId);
                builder.Entity<Person>().Property(x => x.Id).ValueGeneratedNever();
            }
        }
    }
}