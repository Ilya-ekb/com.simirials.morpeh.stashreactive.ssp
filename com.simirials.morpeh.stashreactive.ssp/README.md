Morpeh.StashReactive.SSP
Reactive Added/Removed events for Stash<T> based on Morpeh.SystemStateProcessor.
	•	Unity 2021.3+ (compatible with Unity 6)
	•	License: MIT
	•	Requires packages: Morpeh and Morpeh.SystemStateProcessor

Installation

Install the dependencies via Project → Package Manager:
	•	Morpeh: https://github.com/scellecs/morpeh.git?path=Scellecs.Morpeh
	•	SystemStateProcessor: https://github.com/codewriter-packages/Morpeh.SystemStateProcessor.git

Note: Git dependencies cannot be declared in a UPM package’s package.json—only in the project manifest (Packages/manifest.json).

Install this package:
	•	Add package from git URL → your repository URL, or
	•	Add package from disk → select package.json

Usage

var sys = World.Default.AddReactiveStashSSP<MyComponent>();
sys.Added   += e => /* ... */;
sys.Removed += e => /* ... */;

See the example in Samples~/Basic Usage.

Why this design
	•	Events are built on SystemStateProcessor: entity entering/leaving the With<T>() filter triggers Create/Remove callbacks; call .Process() every frame.
	•	Git dependencies between packages aren’t supported in package.json, so Morpeh and SystemStateProcessor are installed separately in the project.

Build

Runtime-only package (an asmdef referencing Scellecs.Morpeh and Scellecs.Morpeh.SystemStateProcessor).

© 2025 Simirials. MIT.
