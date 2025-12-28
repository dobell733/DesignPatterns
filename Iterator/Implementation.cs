namespace Iterator
{
    public class Person
    {
        public string Name { get; set; }
        public string Country { get; set; }

        public Person(string name, string country)
        {
            Name = name;
            Country = country;
        }
    }

    /// <summary>
    /// Iterator
    /// </summary>
    public interface IPeopleIterator
    {
        Person First();
        Person Next();
        bool IsDone { get; }
        Person CurrentItem { get; }
    }

    /// <summary>
    /// Aggregate
    /// </summary>
    public interface IPeopleCollection
    {
        IPeopleIterator CreateIterator();
    }

    /// <summary>
    /// ConcreteAggregate
    /// </summary>
    public class PeopleCollection : List<Person>, IPeopleCollection
    {
        public IPeopleIterator CreateIterator()
        {
            return new PeopleIterator(this);
        }
    }

    /// <summary>
    /// ConcreteIterator
    /// </summary>
    public class PeopleIterator : IPeopleIterator
    {
        private readonly List<Person> _sorted;
        private int _current;

        public PeopleIterator(IEnumerable<Person> collection)
        {
            _sorted = collection.OrderBy(p => p.Name).ToList();
            _current = 0;
        }

        public bool IsDone => _current >= _sorted.Count;

        public Person CurrentItem => IsDone ? null : _sorted[_current];

        public Person First()
        {
            _current = 0;
            return CurrentItem;
        }

        public Person Next()
        {
            _current++;
            return CurrentItem;
        }
    }
}
