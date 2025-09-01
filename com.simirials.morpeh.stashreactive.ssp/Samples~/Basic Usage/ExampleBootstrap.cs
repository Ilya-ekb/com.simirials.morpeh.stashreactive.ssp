using Morpeh.ReactiveSSP;
using Scellecs.Morpeh;
using UnityEngine;

public struct Health : IComponent { public int hp; }

public class ExampleBootstrap : MonoBehaviour {
    private World world;
    private ReactiveStashSSP<Health> reactive;

    private void Start() {
        world = World.Default;

        reactive = world.AddReactiveStashSSP<Health>(order: 50, emitExistingOnAwake: true);
        reactive.Added   += e => Debug.Log($"[Health] ADDED   -> {e.ID}");
        reactive.Removed += e => Debug.Log($"[Health] REMOVED -> {e.ID}");

        var stash = world.GetStash<Health>();
        var ent = world.CreateEntity();
        ref var h = ref stash.Add(ent);
        h.hp = 100;
        world.Commit();

        stash.Remove(ent);
        world.Commit();
    }
}
