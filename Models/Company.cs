using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceManagerApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string KRS { get; set; }

        public string NIP { get; set; }

        public string Name { get; set; }

        private List<Company> Companies { get; set; }

        private void LoadData()
        {
            Companies = new List<Company>()
            {
                new Company { Id = 0, KRS = "0000016432", NIP = "5210420047", Name = "HDI"},
                new Company { Id = 1, KRS = "0000016432", NIP = "5210420047", Name = "Warta"},
                new Company { Id = 2, KRS = "0000493693", NIP = "1080016534", Name = "RESO"},
                new Company { Id = 3, KRS = "10623", NIP = "5262349108", Name = "Proama"},
                new Company { Id = 4, KRS = "6691", NIP = "5260214686", Name = "Compensa"},
                new Company { Id = 5, KRS = "0000028261", NIP = "5251565015", Name = "Allianz"},
                new Company { Id = 6, KRS = "0000271543", NIP = "1070006155", Name = "AXA"},
                new Company { Id = 7, KRS = "0000701305", NIP = "121265113", Name = "EUROINS"},
                new Company { Id = 8, KRS = "10623", NIP = "5262349108", Name = "Generali"},
                new Company { Id = 9, KRS = "0000033882", NIP = "5240302393", Name = "GOTHAER"},
                new Company { Id = 10, KRS = "0000033882", NIP = "5240302393", Name = "WIENER"},
            };
        }

        public List<Company> GetCompanies()
        {
            LoadData();
            return Companies;
        }
    }
}
