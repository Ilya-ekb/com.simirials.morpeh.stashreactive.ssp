// MIT License
using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;

namespace Morpeh.ReactiveSSP {
    /// Реактивные события на вход/выход сущности в Filter.With<T>().
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ReactiveStashSSP<T> : UpdateSystem where T : struct, IComponent {
        public event Action<Entity> Added;
        public event Action<Entity> Removed;

        private SystemStateProcessor<State> processor;
        private readonly bool emitExistingOnAwake;

        public ReactiveStashSSP(bool emitExistingOnAwake = true) {
            this.emitExistingOnAwake = emitExistingOnAwake;
        }

        public override void OnAwake() {
            processor = World.Filter.With<T>()
                .ToSystemStateProcessor(Create, Remove);
            if (emitExistingOnAwake) {
                processor.Process();
            }
        }

        public override void OnUpdate(float deltaTime) => processor.Process();

        public override void Dispose() {
            processor.Dispose();
            Added = null;
            Removed = null;
        }

        private State Create(Entity e) {
            Added?.Invoke(e);
            return new State { entity = e };
        }

        private void Remove(ref State s) => Removed?.Invoke(s.entity);

        private struct State : ISystemStateComponent {
            public Entity entity;
        }
    }

    internal static class ReactiveRegistry<T> where T : struct, IComponent {
        private sealed class Entry {
            public ReactiveStashSSP<T> System;
            public readonly Dictionary<int, SystemsGroup> GroupsByOrder = new();
        }
        private static readonly ConditionalWeakTable<World, Entry> table = new();

        public static ReactiveStashSSP<T> GetOrCreate(World world, int order, bool emitExistingOnAwake) {
            var entry = table.GetValue(world, _ => new Entry());
            if (entry.System != null)
                return entry.System;

            var sys = new ReactiveStashSSP<T>(emitExistingOnAwake);

            if (!entry.GroupsByOrder.TryGetValue(order, out var group)) {
                group = world.CreateSystemsGroup();
                try { world.AddSystemsGroup(order, group); }
                catch (ArgumentException) { world.AddPluginSystemsGroup(group); } // order занят — уходим в plugin
                entry.GroupsByOrder[order] = group;
            }

            group.AddSystem(sys);
            entry.System = sys;
            return sys;
        }
    }

    public static class ReactiveStashSSPExtensions {
        public static ReactiveStashSSP<T> AddReactiveStashSSP<T>(
            this World world, int order = 100, bool emitExistingOnAwake = true)
            where T : struct, IComponent
            => ReactiveRegistry<T>.GetOrCreate(world, order, emitExistingOnAwake);
    }
}
