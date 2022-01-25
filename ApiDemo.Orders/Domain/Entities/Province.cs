    using ApiDemo.Api.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ApiDemo.Api.Domain.Entities
{
    public class Province
    {
        private static readonly Province[] _provinces = new[]
        {
            Gauteng, WesternCape, KwaZuluNatal, EasternCape, FreeState, Limpopo, Mpumalanga, NorthernCape, NorthWest
        };

        public int Id { get; }
        public string Name { get; set; }

        public Province(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static implicit operator string(Province province)
        {
            return province.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public static Province From(int id)
        {
            return 
                !_provinces.Select(p => p.Id).Contains(id)
                ? throw new InvalidProvinceException(id)
                : new Province(id, _provinces.First(p => p.Id == id).Name);
        }

        public static Province Gauteng => new (1, "Gauteng");
        public static Province WesternCape => new (2, "Western Cape");
        public static Province KwaZuluNatal => new (3, "KwaZulu-Natal");
        public static Province EasternCape => new (4, "EasternCape");
        public static Province FreeState => new (5, "Free State");
        public static Province Limpopo => new (6, "Limpopo");
        public static Province Mpumalanga => new (7, "Mpumalanga");
        public static Province NorthernCape => new (8, "Northern Cape");
        public static Province NorthWest => new (9, "North West");
    }
}
