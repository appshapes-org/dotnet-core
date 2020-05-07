using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Xunit;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class MockEntityEntryTests
    {
        [Fact]
        public void ConstructorMustSetEntityToEntityWhenCalled()
        {
            Person person = new Person();
            StubEntityType<Person> entityType = new StubEntityType<Person>();
            FakeStateManager manager = new FakeStateManager();
            manager.StateChanged += (_, __) => { };
            manager.Tracked += (_, __) => { };
            Assert.Equal(person.Id, (new MockEntityEntry(person, manager, entityType).Entity as Person)?.Id);
        }

        private class FakeStateManager : IStateManager
        {
            public void AcceptAllChanges()
            {
                throw new NotImplementedException();
            }

            public void CascadeChanges(bool force)
            {
                throw new NotImplementedException();
            }

            public void CascadeDelete(InternalEntityEntry entry, bool force, IEnumerable<IForeignKey> foreignKeys = null)
            {
                throw new NotImplementedException();
            }

            public CascadeTiming CascadeDeleteTiming { get; set; }

            public int ChangedCount { get; set; }

            public DbContext Context { get; }

            public int Count { get; }

            public IEntityFinder CreateEntityFinder(IEntityType entityType)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry CreateEntry(IDictionary<string, object> values, IEntityType entityType)
            {
                throw new NotImplementedException();
            }

            public CascadeTiming DeleteOrphansTiming { get; set; }

            public StateManagerDependencies Dependencies { get; }

            public IEntityMaterializerSource EntityMaterializerSource { get; }

            public IEnumerable<InternalEntityEntry> Entries { get; }

            public InternalEntityEntry FindPrincipal(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry FindPrincipalUsingPreStoreGeneratedValues(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry FindPrincipalUsingRelationshipSnapshot(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public int GetCountForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<InternalEntityEntry> GetDependents(InternalEntityEntry principalEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<InternalEntityEntry> GetDependentsFromNavigation(InternalEntityEntry principalEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot(InternalEntityEntry principalEntry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
            {
                throw new NotImplementedException();
            }

            public IList<IUpdateEntry> GetEntriesToSave(bool cascadeChanges)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TEntity> GetNonDeletedEntities<TEntity>() where TEntity : class
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry GetOrCreateEntry(object entity)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry GetOrCreateEntry(object entity, IEntityType entityType)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Tuple<INavigation, InternalEntityEntry>> GetRecordedReferrers(object referencedEntity, bool clear)
            {
                throw new NotImplementedException();
            }

            public IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }

            public void InvokeStateChanged()
            {
                StateChanged?.Invoke(this, null);
            }

            public void InvokeTracked()
            {
                Tracked?.Invoke(this, null);
            }

            public IModel Model { get; }

            public void OnStateChanged(InternalEntityEntry internalEntityEntry, EntityState oldState)
            {
                throw new NotImplementedException();
            }

            public void OnTracked(InternalEntityEntry internalEntityEntry, bool fromQuery)
            {
                throw new NotImplementedException();
            }

            public void RecordReferencedUntrackedEntity(object referencedEntity, INavigation navigation, InternalEntityEntry referencedFromEntry)
            {
                throw new NotImplementedException();
            }

            public void ResetState()
            {
                throw new NotImplementedException();
            }

            public Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public int SaveChanges(bool acceptAllChangesOnSuccess)
            {
                throw new NotImplementedException();
            }

            public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public bool SensitiveLoggingEnabled { get; }

            public InternalEntityEntry StartTracking(InternalEntityEntry entry)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity, in ValueBuffer valueBuffer)
            {
                throw new NotImplementedException();
            }

            public event EventHandler<EntityStateChangedEventArgs> StateChanged;

            public void StateChanging(InternalEntityEntry entry, EntityState newState)
            {
                throw new NotImplementedException();
            }

            public void StopTracking(InternalEntityEntry entry, EntityState oldState)
            {
                throw new NotImplementedException();
            }

            public event EventHandler<EntityTrackedEventArgs> Tracked;

            public InternalEntityEntry TryGetEntry(IKey key, object[] keyValues)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry TryGetEntry(IKey key, object[] keyValues, bool throwOnNullKey, out bool hasNullKey)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry TryGetEntry(object entity, bool throwOnNonUniqueness = true)
            {
                throw new NotImplementedException();
            }

            public InternalEntityEntry TryGetEntry(object entity, IEntityType type, bool throwOnTypeMismatch = true)
            {
                throw new NotImplementedException();
            }

            public void Unsubscribe()
            {
                throw new NotImplementedException();
            }

            public void UpdateDependentMap(InternalEntityEntry entry, IForeignKey foreignKey)
            {
                throw new NotImplementedException();
            }

            public void UpdateIdentityMap(InternalEntityEntry entry, IKey principalKey)
            {
                throw new NotImplementedException();
            }

            public IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

            public IValueGenerationManager ValueGenerationManager { get; }
        }

        private class Person
        {
            public Guid Id { get; protected set; } = Guid.NewGuid();
        }

        private class StubEntityType<T> : EntityType
        {
            public StubEntityType() : base(typeof(T), new Model(), ConfigurationSource.Convention)
            {
            }
        }
    }
}