using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceManagerApp.Models
{
    public class Keyword
    {
        public int Id { get; set; }

        public string CompanyKrs { get; set; }

        public string PropertyName { get; set; }

        public string Word { get; set; }


        //-------------------------------------------------

        private List<Keyword> Keywords { get; set; }

        private void LoadData()
        {
            Keywords = new List<Keyword>()
            {
                //HDI
                //Warta
                new Keyword { Id = 1, CompanyKrs = "0000016432", PropertyName = "StartIndex", Word = "DANE OSOBOWE" },
                new Keyword { Id = 2, CompanyKrs = "0000016432", PropertyName = "FinishIndex", Word = "POJAZD" },
                new Keyword { Id = 3, CompanyKrs = "0000016432", PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 4, CompanyKrs = "0000016432", PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający" },
                new Keyword { Id = 5, CompanyKrs = "0000016432", PropertyName = nameof(Customer.AddressId), Word = "Adres" },
                new Keyword { Id = 6, CompanyKrs = "0000016432", PropertyName = nameof(Customer.AddressId), Word = "Adres korespondencyjny" },
                new Keyword { Id = 7, CompanyKrs = "0000016432", PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania" },
                new Keyword { Id = 8, CompanyKrs = "0000016432", PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania/korespondencyjny" },
                new Keyword { Id = 9, CompanyKrs = "0000016432", PropertyName = nameof(Customer.CellPhone), Word = "Telefon komórkowy" },
                new Keyword { Id = 10, CompanyKrs = "0000016432", PropertyName = nameof(Customer.HomePhone), Word = "Telefon stacjonarny" },
                new Keyword { Id = 11, CompanyKrs = "0000016432", PropertyName = nameof(Customer.Email), Word = "E-MAIL" },
                
                //RESO
                new Keyword { Id = 12, CompanyKrs = "0000493693", PropertyName = "StartIndex", Word = "Dane posiadaczy" },
                new Keyword { Id = 13, CompanyKrs = "0000493693", PropertyName = "FinishIndex", Word = "Dane pojazdu" },
                new Keyword { Id = 14, CompanyKrs = "0000493693", PropertyName = nameof(Customer.PESEL), Word = "Pesel/Regon/Data urodzenia " },
                new Keyword { Id = 15, CompanyKrs = "0000493693", PropertyName = nameof(Customer.LastName), Word = "Imię i nazwisko / Nazwa " },
                new Keyword { Id = 16, CompanyKrs = "0000493693", PropertyName = nameof(Customer.AddressId), Word = "Ulica, nr domu, lokalu " },
                new Keyword { Id = 17, CompanyKrs = "0000493693", PropertyName = nameof(Customer.CellPhone), Word = "Nr telefonu " },
                new Keyword { Id = 18, CompanyKrs = "0000493693", PropertyName = nameof(Customer.Email), Word = "E-mail " },

                //Proama
                new Keyword { Id = 19, CompanyKrs = "10623", PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 20, CompanyKrs = "10623", PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony " },
                new Keyword { Id = 21, CompanyKrs = "10623", PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony" },
                new Keyword { Id = 22, CompanyKrs = "10623", PropertyName = nameof(Customer.BirthDate), Word = "data urodzenia" },
                new Keyword { Id = 23, CompanyKrs = "10623", PropertyName = nameof(Customer.AddressId), Word = "adres" },
                new Keyword { Id = 24, CompanyKrs = "10623", PropertyName = nameof(Customer.CellPhone), Word = "telefon" },
                
                //Compensa
                new Keyword { Id = 25, CompanyKrs = "6691", PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 26, CompanyKrs = "6691", PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony: właściciel" },
                new Keyword { Id = 26, CompanyKrs = "6691", PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony" },
                new Keyword { Id = 27, CompanyKrs = "6691", PropertyName = nameof(Customer.AddressId), Word = "Dane adresowe:" },
                new Keyword { Id = 28, CompanyKrs = "6691", PropertyName = nameof(Customer.CellPhone), Word = "kom:" },
                
                //Allianz
                new Keyword { Id = 29, CompanyKrs = "0000028261", PropertyName = "StartIndex", Word = "OŚWIADCZENIA UBEZPIECZAJĄCEGO" },
                new Keyword { Id = 29, CompanyKrs = "0000028261", PropertyName = "FinishIndex", Word = "TWÓJ AGENT" },
                new Keyword { Id = 31, CompanyKrs = "0000028261", PropertyName = nameof(Customer.PESEL), Word = "PESEL:" },
                new Keyword { Id = 32, CompanyKrs = "0000028261", PropertyName = nameof(Customer.LastName), Word = "UBEZPIECZAJĄCY/WŁAŚCICIEL POJAZDU" },
                new Keyword { Id = 35, CompanyKrs = "0000028261", PropertyName = nameof(Customer.CellPhone), Word = "tel.:" },
            };
        }

        public List<Keyword> GetKeywords()
        {
            LoadData();
            return Keywords;
        }
    }
}
