using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Morpeh.ReactiveSSP {
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ReactiveStashSSP<T> : ISystem where T : struct, IComponent {
        public World World { get; set; }

        public event Action<Entity> Added;
        public event Action<Entity> Removed;

        private SystemStateProcessor<State> processor;
        private readonly bool emitExistingOnAwake;

        public ReactiveStashSSP(bool emitExistingOnAwake = true) {
            this.emitExistingOnAwake = emitExistingOnAwake;
        }

        public void OnAwake() {
            processor = World.Filter.With<T>().ToSystemStateProcessor(Create, Remove);
            if (emitExistingOnAwake) processor.Process();
        }

        public void OnUpdate(float dt) => processor.Process();

        public void Dispose() {
            processor.Dispose();
            Added = null;
            Removed = null;
        }

        private State Create(Entity e) {
            Added?.Invoke(e);
            return new State { entity = e };
        }

        private void Remove(ref State s) => Removed?.Invoke(s.entity);

        private struct State : ISystemStateComponent { public Entity entity; }
    }

    public static class ReactiveStashSSPExtensions {
        public static ReactiveStashSSP<T> AddReactiveStashSSP<T>(this World world, int order = 0, bool emitExistingOnAwake = true)
            where T : struct, IComponent {
            var sys = new ReactiveStashSSP<T>(emitExistingOnAwake);
            var group = world.CreateSystemsGroup();
            group.AddSystem(sys);
            world.AddSystemsGroup(order, group);
            return sys;
        }
    }
}