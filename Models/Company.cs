using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceManagerApp.Models
{
    public class Company
    {
        public string KRS { get; set; }

        public string NIP { get; set; }

        public string Name { get; set; }

        private List<Company> Companies { get; set; }

        private void LoadData()
        {
            Companies = new List<Company>()
            {
                new Company { KRS = "0000016432", NIP = "521 04 20 047", Name = "HDI"},
                new Company { KRS = "0000016432", NIP = "521 04 20 047", Name = "Warta"},
                new Company { KRS = "0000493693", NIP = "108 001 65 34", Name = "RESO"},
                new Company { KRS = "10623", NIP = "526 23 49 108", Name = "Proama"},
                new Company { KRS = "6691", NIP = "526 02 14 686", Name = "Compensa"},
                new Company { KRS = "0000028261", NIP = "525 15 65 015", Name = "Allianz"},
            };
        }

        public List<Company> GetCompanies()
        {
            LoadData();
            return Companies;
        }
    }
}
