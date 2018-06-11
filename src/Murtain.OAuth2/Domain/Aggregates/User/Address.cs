using Microsoft.EntityFrameworkCore;
using Murtain.Extensions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Domain.Aggregates.User
{
    [Owned]
    public class Address : ValueObject
    {

        private Address() { }

        public Address(string street, string city, string state, string country, string zipcode)
        {
            Country = country;
            Province = Province;
            City = city;
            Street = Street;
        }

        [MaxLength(250)]
        public virtual string Street { get; set; }
        [MaxLength(50)]
        public virtual string City { get; set; }
        [MaxLength(50)]
        public virtual string Province { get; set; }
        [MaxLength(50)]
        public virtual string Country { get; set; }

        public override string ToString()
        {
            return $"{Country} {Province} {City} {Street}";
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Country;
            yield return Province;
            yield return City;
            yield return Street;
        }
    }
}
