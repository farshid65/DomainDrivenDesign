using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Framework
{
    public abstract class Value<T>
       where T : Value<T>
    {
        //protected abstract IEnumerable<object> GetEqualityComponents();
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static readonly Member[] Members = GetMembers().ToArray();



        public override bool Equals(object? obj)
        {
            //var valueObject= obj as T;
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return obj.GetType() == typeof(T) && Members.All(m =>
            {
                var otherValue = m.GetValue(obj);
                var thisValue = m.GetValue(this);
                return m.IsNonStringEnumerable
                ? GetEnumerableVaues(otherValue).SequenceEqual(GetEnumerableVaues(thisValue))
                : (otherValue?.Equals(thisValue) ?? thisValue == null);
            });
        }
        public override int GetHashCode() =>

            CombineHashCode(
                Members.Select(m => m.IsNonStringEnumerable
                ? CombineHashCode(GetEnumerableVaues(m.GetValue(this)))
                : m.GetValue(this)));


        public static bool operator ==(Value<T> left, Value<T> right) => Equals(left, right);

        public static bool operator !=(Value<T> left, Value<T> right) => !Equals(left, right);
        public override string ToString()
        {
            if (Members.Length == 1)
            {
                var m = Members[0];
                var value = m.GetValue(this);
                return m.IsNonStringEnumerable
                    ? $"{string.Join("|", GetEnumerableVaues(value))}"
                    : value.ToString();
            }
            var values = Members.Select(m =>
            {
                var value = m.GetValue(this);
                return m.IsNonStringEnumerable
                ? $"{m.Name}:{string.Join("|", GetEnumerableVaues(value))}"
                : m.Type != typeof(string)
                ? $"{m.Name}:{value}"
                : value == null
            ? $"{m.Name}:null"
                  : $"{m.Name}:\"{value}\"";
            });
            return $"{typeof(T).Name}[{string.Join("|", values)}]";
        }
        private static IEnumerable<Member> GetMembers()
        {
            var t = typeof(T);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            while (t != typeof(object))
            {
                if (t == null) continue;
                foreach (var p in t.GetProperties(flags)) yield return new Member(p);
                foreach (var f in t.GetFields(flags)) yield return new Member(f);
                t = t.BaseType;
            }

        }
        private static IEnumerable<object> GetEnumerableVaues(object obj)
        {
            var enumerator = ((IEnumerable)obj).GetEnumerator();
            while (enumerator.MoveNext()) yield return enumerator.Current;
        }
        private static int CombineHashCode(IEnumerable<object> objs)
        {
            unchecked
            {
                return objs.Aggregate(17, (current, obj) => current * 59 + (obj?.GetHashCode() ?? 0));
            }
        }
        private class Member

        {
            public readonly string Name;
            public readonly Func<object, object> GetValue;
            public readonly bool IsNonStringEnumerable;
            public readonly Type Type;
            private object prop;

            public Member(MemberInfo info)
            {
                switch (info)
                {
                    case FieldInfo field:
                        Name = field.Name;
                        GetValue = obj => field.GetValue(obj);
                        IsNonStringEnumerable = typeof(IEnumerable).IsAssignableFrom(prop.GetType())
                            && prop.GetType() != typeof(string);
                        Type = prop.GetType();
                        break;
                    default:
                        throw new ArgumentException("Member is not a field or property?!?!", info.Name);
                }
            }
        }
    }
}

    