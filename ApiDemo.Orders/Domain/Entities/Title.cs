using ApiDemo.Api.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ApiDemo.Api.Domain.Entities
{
    public class Title
    {
        private readonly static Title[] _titles = new[]
        {
            Mr, Mrs, Ms, Jr, Dr, Prof, Esq, Hon, Sir
        };

        public int Id { get; }
        public string Description { get; set; }

        public Title(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public static Title From(int id)
        {
            if (!_titles.Select(t => t.Id).Contains(id))
            {
                throw new InvalidTitleException(id);
            }

            return new Title(id, _titles.First(t => t.Id == id).Description);
        }

        public static implicit operator string(Title title)
        {
            return title.ToString();
        }

        public override string ToString()
        {
            return Description;
        }

        public static Title Mr => new (1, "Mr.");
        public static Title Mrs => new (2, "Mrs.");
        public static Title Ms => new (3, "Ms.");
        public static Title Jr => new (4, "Jr.");
        public static Title Dr => new (5, "Dr.");
        public static Title Prof => new (6, "Prof.");
        public static Title Esq => new (7, "Esq.");
        public static Title Hon => new (8, "Hon.");
        public static Title Sir => new (9, "Sir");
    }
}
