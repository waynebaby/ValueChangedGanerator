# ValueChangedGanerator

A Roslyn Code Fix provider for generating PropertyChanged from inner struct members.

This inlcudes VSIX and NuGet packages of an analyzer created by using .NET Compiler Platform (Roslyn).

## Usage

- Insert an inner struct named `NotifyRecord` into a class that you want to generete its properties.
- Use 'Quick Action' (Lightbulb) to fix the code

## Sample

original source: 

```cs
class Point : BindableBase
{
    struct NotifyRecord
    {
        public int X;
        public int Y;
        public int Z => X * Y;

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;
    }
}
```

fixed result of the original source (`partial` modifier added):

```cs
partial class Point : BindableBase
{
    struct NotifyRecord
    {
        public int X;
        public int Y;
        public int Z => X * Y;

        /// <summary>
        /// Name.
        /// </summary>
        public string Name;
    }
}
```

the generetated source code:

```cs
using System.ComponentModel;

partial class Point
{
    private NotifyRecord _value;

    public int X { get { return _value.X; } set { SetProperty(ref _value.X, value, XProperty); OnPropertyChanged(ZProperty); } }
    private static readonly PropertyChangedEventArgs XProperty = new PropertyChangedEventArgs(nameof(X));
    public int Y { get { return _value.Y; } set { SetProperty(ref _value.Y, value, YProperty); OnPropertyChanged(ZProperty); } }
    private static readonly PropertyChangedEventArgs YProperty = new PropertyChangedEventArgs(nameof(Y));

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get { return _value.Name; } set { SetProperty(ref _value.Name, value, NameProperty); } }
    private static readonly PropertyChangedEventArgs NameProperty = new PropertyChangedEventArgs(nameof(Name));
    public int Z => _value.Z;
    private static readonly PropertyChangedEventArgs ZProperty = new PropertyChangedEventArgs(nameof(Z));
}
```

