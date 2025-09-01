# Morpeh.StashReactive.SSP

Реактивные события **Added/Removed** для `Stash<T>` на базе **Morpeh.SystemStateProcessor**.

- Unity **2021.3+** (совместимо с Unity 6)
- Лицензия: MIT
- Требуются пакеты Morpeh и Morpeh.SystemStateProcessor

## Установка

1. Установите зависимости в **Project → Package Manager**:
   - Morpeh: `https://github.com/scellecs/morpeh.git?path=Scellecs.Morpeh`
   - SystemStateProcessor: `https://github.com/codewriter-packages/Morpeh.SystemStateProcessor.git`

> Примечание: Git-зависимости **нельзя** указывать в `package.json` UPM-пакета — только в манифесте проекта (`Packages/manifest.json`).

2. Установите этот пакет:
   - *Add package from git URL* → URL вашего репозитория **или**
   - *Add package from disk* → выберите `package.json`

## Использование

```csharp
var sys = World.Default.AddReactiveStashSSP<MyComponent>();
sys.Added   += e => /* ... */;
sys.Removed += e => /* ... */;
```

См. пример в `Samples~/Basic Usage`.

## Почему так

- События строятся на **SystemStateProcessor**: вход/выход сущности в фильтр `With<T>()` → колбэки `Create/Remove`, вызов `.Process()` в каждом кадре.
- Git-зависимости между пакетами недоступны в `package.json`, поэтому Morpeh и SystemStateProcessor ставятся в проект отдельно.

## Сборка

Пакет — **Runtime only** (`asmdef` с ссылками на `Scellecs.Morpeh` и `Scellecs.Morpeh.SystemStateProcessor`).

---

© 2025 Simirials. MIT.
