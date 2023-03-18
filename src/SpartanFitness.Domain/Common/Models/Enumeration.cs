using System.Reflection;

namespace SpartanFitness.Domain.Common.Models;

public abstract class Enumeration : IComparable
{
    public static IEnumerable<T> Getall<T>() 
    where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        // var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        // return typeMatches && valueMatches;
        return valueMatches;
    }

    public int CompareTo(object? obj) =>
       obj is null
           ? 0
           : Id.CompareTo(((Enumeration)obj).Id);

    public override int GetHashCode() => Id.GetHashCode();
}