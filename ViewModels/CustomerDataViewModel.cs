using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceManagerApp.ViewModels
{
    public class CustomerDataViewModel
    {
        public string PESEL { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string CellPhone { get; set; }

        public string HomePhone { get; set; }

        public string Email { get; set; }

        public string House { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }


    }
}
