# G-injector

This injector is primarily designed for Gorilla Tag but can be used with any Mono-based process.
It is fully open-source and operates using SMI, an injection library.
The entire tool is implemented in C# as a console application, ensuring a minimalistic and efficient design.

# Tutorial
1. get your DLL that supported
2. drag the DLL into GInjector.exe
3. open your game of choice
4. injection completed.

# Compatabilty (DEVS)
to make your dll support all you have to do is add our premade class to your project and tweak it just a tiny bit to target your entry point

```csharp 
using InjectionTest;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Loader
{
    public class Loader
    {
        public static void Load()
        {
            gameObject = new GameObject();
            gameObject.AddComponent<Plugin>(); // Change this to your entry point e.g (your starting class, initilization class, etc)
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        public static GameObject gameObject = null;
    }
}
```
