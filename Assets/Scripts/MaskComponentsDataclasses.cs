using System.Collections.Generic;
using UnityEngine;

public class GenericMaskPartComps<T>
{
    public readonly T face;
    public readonly T eyes;
    public readonly T mouth;

    public GenericMaskPartComps(T e, T f, T m)
    {
        eyes = e;
        face = f;
        mouth = m;
    }

    public List<T> toList()
    {
        var l = new List<T>();
        l.Add(eyes);
        l.Add(face);
        l.Add(mouth);
        return l;
    }

}

public class MaskCompletePool : GenericMaskPartComps<List<Sprite>>
{
    public MaskCompletePool(List<Sprite> e, List<Sprite> f, List<Sprite> m) : base(e, f, m) { }
}

public class MaskComponents : GenericMaskPartComps<string>
{
    public MaskComponents(string e, string f, string m) : base(e, f, m) { }
}