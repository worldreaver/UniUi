# Readme

## Requirements
[![Unity 2018.3+](https://img.shields.io/badge/unity-2018.3+-brightgreen.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)
[![.Net 2.0 Scripting Runtime](https://img.shields.io/badge/.NET-2.0-blueviolet.svg?style=flat&cacheSeconds=2592000)](https://docs.unity3d.com/2019.1/Documentation/Manual/ScriptingRuntimeUpgrade.html)

## Installation

```
"dependencies": {
    ...
    "com.worldreaver.unirxex": "https://github.com/worldreaver/UniRxEx.git#upm",
    ...
  },
```

## Usages

```csharp
var subject = new Subject<int>();
var observer = new TestObserver<int>();

subject.Subscribe(observer);

subject.OnNext(1);
subject.OnNext(2);
subject.OnCompleted();

Assert.AreEqual(2, observer.OnNextCount);
Assert.AreEqual(1, observer.OnNextValues[0]);
Assert.AreEqual(2, observer.OnNextValues[1]);
Assert.AreEqual(1, observer.OnCompletedCount);
```

### SubjectProperty

SubjectProperty is like a ReactiveProperty, but it's Subject.
It's IObserver & IObservable, and not holding latest value in stream.

```csharp
var property = new SubjectProperty<int>();
property.Subscribe(observer); // behave as IObservable
observable.Subscribe(property); // behave as IObserver
property.Value; // and it's easy to access value
```

### IsDefault(), IsNotDefault()

IsDefault(), IsNotDefault() are operators for `IObservable<T>`.
These operators stream values when the streamed value is default (or not default).

```csharp
var intSubject = new Subject<int>();
intSubject.IsDefault().Subscribe(x => Debug.Log(x)); // 0
intSubject.IsNotDefault().Subscribe(x => Debug.Log(x)); // 100
intSubject.OnNext(0);
intSubject.OnNext(100);

var objectSubject = new Subject<AnyClass>();
objectSubject.IsDefault().Subscribe(x => Debug.Log(x)); // "null"
objectSubject.IsNotDefault().Subscribe(x => Debug.Log(x)); // "AnyClass"
objectSubject.OnNext(null);
objectSubject.OnNext(new AnyClass());
```

Both methods accept a selector as an argument to select values to determine.

```csharp
var intSubject = new Subject<int>();
intSubject.IsDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 0
intSubject.IsDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 0, 100
intSubject.IsNotDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 100
intSubject.OnNext(0);
intSubject.OnNext(100);

class AnyClass { bool Value; }

var objectSubject = new Subject<AnyClass>();
objectSubject.IsDefault().Subscribe(x => Debug.Log(x)); // null, "AnyClass"
objectSubject.IsDefault().Subscribe(x => Debug.Log(x.Value)); // throw NullReferenceException in Subscribe()
objectSubject.IsNotDefault().Subscribe(x => Debug.Log(x.Value)); // true
objectSubject.OnNext(null);
objectSubject.OnNext(new AnyClass() { Value = false });
objectSubject.OnNext(new AnyClass() { Value = true });
```

### Notify tense

```csharp
var tenseSubject = new TenseSubject();
tenseSubject.WhenDo().Subscribe(_ => Debug.Log("`Do` notified"));
tenseSubject.WhenWill().Subscribe(_ => Debug.Log("`Will` notified"));
tenseSubject.WhenDid().Subscribe(_ => Debug.Log("`Did` notified"));

tenseSubject.Do(); // Notify `do`
tenseSubject.Will(); // Notify `will`
tenseSubject.Did(); // Notify `did`
```

### Notify tense with value

```csharp
var tenseSubject = new TenseSubject<int>();
tenseSubject.WhenDo().Subscribe(x => Debug.Log($"`Do` notified with value: {x}")); // `Do` notified with value: 1
tenseSubject.WhenWill().Subscribe(x => Debug.Log($"`Will` notified with value: {x}")); // `Will` notified with value: 2
tenseSubject.WhenDid().Subscribe(x => Debug.Log($"`Did` notified with value: {x}")); // `Did` notified with value: 3

tenseSubject.Do(1); // Notify `do`
tenseSubject.Will(2); // Notify `will`
tenseSubject.Did(3); // Notify `did`
```

```csharp
using UniRx;

public class Sample {

    void Start() {
        ObservableUnityWebRequest.GetText("https://www.google.com/").Subscribe(
            (responseText) => {
                Debug.Log(responseText);
            }
        );
    }

}
```

* Provide some methods for defined in `UnityEngine.Networking.UnityWebRequest`.

### .

- Provide Operator pause observable
- Provide Operator FromCoroutineWithInitialValueObservable


### State Machine

```csharp
using UnityEngine;
using UniRx;
using HC.AI;


[DisallowMultipleComponent]
public class Example : State
{
    #region event

    private void Start()
    {
        BeginStream.Subscribe(_ => Debug.Log("State Begin"));
        UpdateStream.Subscribe(_ => Debug.Log("State Update"));
        FixedUpdateStream.Subscribe(_ => Debug.Log("State FixedUpdate"));
        LateUpdateStream.Subscribe(_ => Debug.Log("State LateUpdate"));
        EndStream.Subscribe(_ => Debug.Log("State End"));
        OnDrawGizmosStream.Subscribe(_ => Debug.Log("State OnDrawGizmos"));
        OnGUIStream.Subscribe(_ => Debug.Log("State OnGUI"));
    }

    #endregion
}
```

```c#
var myReactiveProperty = new ReactiveProperty<float>(1.0f);
mySlider.BindValueTo(myReactiveProperty);
```

However if I was working with a 3rd party library I may not have ReactiveProperty objects, so for that I would do:

```c#
var myNormalValue = 1.0f;
mySlider.BindValueTo(() => myNormalValue, x => myNormalValue = x);
```

You can also specify if you want one way or two way bindings explicitly.

```c#
var myReactiveProperty = new ReactiveProperty<float>(1.0f);
mySlider.BindValueTo(myReactiveProperty, BindingTypes.OneWay);
```

Here is an example of making a dropdown in the UI bind to a reactive collection:

```c#
var exampleOptions = new ReactiveCollection<string>();
exampleOptions.Add("Option 1");
exampleOptions.Add("Option 2");
exampleOptions.Add("Some Other Option");

someDropdownUIElement.BindOptionsTo(exampleOptions);
```

## Dependencies

- [![UniRx](https://img.shields.io/badge/UniRx-7.1.0+-brightgreen.svg?style=flat&cacheSeconds=2592000)](https://github.com/worldreaver/UniRx/tree/7.1.1)

##License
- Under the MIT license
- Some code is borrowed from @umm, @monry, @tomori-hikage, @grofit
- Thanks for *Tetsuya Mori*, *Tomori Hikage* *grofit*
