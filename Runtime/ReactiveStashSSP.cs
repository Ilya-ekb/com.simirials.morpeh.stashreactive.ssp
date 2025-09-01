// MIT License
using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
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

    internal static class ReactiveSspGroups {
        private sealed class GroupsPerWorld {
            public readonly Dictionary<int, SystemsGroup> byOrder = new();
        }

        private static readonly ConditionalWeakTable<World, GroupsPerWorld> cache = new();

        public static SystemsGroup GetOrCreate(World world, int order) {
            var bucket = cache.GetValue(world, _ => new GroupsPerWorld());

            if (bucket.byOrder.TryGetValue(order, out var group))
                return group;

            group = world.CreateSystemsGroup();
            try {
                world.AddSystemsGroup(order, group); 
            } catch (ArgumentException) {
                world.AddPluginSystemsGroup(group);
            }
            bucket.byOrder[order] = group;
            return group;
        }
    }

    public static class ReactiveStashSSPExtensions {
        public static ReactiveStashSSP<T> AddReactiveStashSSP<T>(
            this World world, int order = 100, bool emitExistingOnAwake = true)
            where T : struct, IComponent {

            var sys = new ReactiveStashSSP<T>(emitExistingOnAwake);
            var group = ReactiveSspGroups.GetOrCreate(world, order);
            group.AddSystem(sys);
            return sys;
        }

        public static ReactiveStashSSP<T> AddReactiveStashSSP<T>(
            this SystemsGroup group, bool emitExistingOnAwake = true)
            where T : struct, IComponent {

            var sys = new ReactiveStashSSP<T>(emitExistingOnAwake);
            group.AddSystem(sys);
            return sys;
        }
    }
}
