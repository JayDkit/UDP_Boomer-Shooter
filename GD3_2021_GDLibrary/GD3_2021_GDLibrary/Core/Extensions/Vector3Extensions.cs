using Microsoft.Xna.Framework;

/// <summary>
/// Adds extra methods to Vector3 using Extensions feature in C#
/// Note - We CANNOT use a namespace when we add extension methods. If we do, then the extensions will not appear in the target class e.g. Vector3
/// </summary>
///<seealso cref="https://www.tutorialsteacher.com/csharp/csharp-extension-method"/>
public static class Vector3Extensions
{
    #region Set to a value in target

    public static void Set(this ref Vector3 target, float? x, float? y, float? z)
    {
        if (x.HasValue)
            target.X = x.Value;
        if (y.HasValue)
            target.Y = y.Value;
        if (z.HasValue)
            target.Z = z.Value;
    }

    public static void Set(this ref Vector3 target, Vector3 translation)
    {
        target.X = translation.X;
        target.Y = translation.Y;
        target.Z = translation.Z;
    }

    public static void Set(this ref Vector3 target, ref Vector3 translation)
    {
        target.X = translation.X;
        target.Y = translation.Y;
        target.Z = translation.Z;
    }

    #endregion Set to a value in target

    #region Add/Remove a value from target

    public static void Add(this ref Vector3 target, float? x, float? y, float? z)
    {
        if (x.HasValue)
            target.X += x.Value;
        if (y.HasValue)
            target.Y += y.Value;
        if (z.HasValue)
            target.Z += z.Value;
    }

    public static void Add(this ref Vector3 target, Vector3 delta)
    {
        target.X += delta.X;
        target.Y += delta.Y;
        target.Z += delta.Z;
    }

    public static void Add(this ref Vector3 target, ref Vector3 delta)
    {
        target.X += delta.X;
        target.Y += delta.Y;
        target.Z += delta.Z;
    }

    #endregion Add/Remove a value from target
}