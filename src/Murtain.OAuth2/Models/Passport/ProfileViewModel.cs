using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Models.Passport
{
    public class ProfileViewModel : PorfileInput
    {
        public string NickName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Birthday { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Gender { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

    }

    public class PorfileInput
    {
        public string SubjectId { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }
    }
}
