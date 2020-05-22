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

        public int CompanyId { get; set; }

        public string PropertyName { get; set; }

        public string Word { get; set; }


        //-------------------------------------------------

        private List<Keyword> Keywords { get; set; }

        private void LoadData()
        {
            Keywords = new List<Keyword>()
            {
                //HDI
                new Keyword { Id = 1, CompanyId = 0, PropertyName = "StartIndex", Word = "DANE OSOBOWE" },
                new Keyword { Id = 2, CompanyId = 0, PropertyName = "FinishIndex", Word = "POJAZD" },
                new Keyword { Id = 3, CompanyId = 0, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 4, CompanyId = 0, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający" },
                new Keyword { Id = 5, CompanyId = 0, PropertyName = nameof(Customer.AddressId), Word = "Adres" },
                new Keyword { Id = 6, CompanyId = 0, PropertyName = nameof(Customer.AddressId), Word = "Adres korespondencyjny" },
                new Keyword { Id = 7, CompanyId = 0, PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania" },
                new Keyword { Id = 8, CompanyId = 0, PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania/korespondencyjny" },
                new Keyword { Id = 9, CompanyId = 0, PropertyName = nameof(Customer.CellPhone), Word = "Telefon komórkowy" },
                new Keyword { Id = 10, CompanyId = 0, PropertyName = nameof(Customer.HomePhone), Word = "Telefon stacjonarny" },
                new Keyword { Id = 11, CompanyId = 0, PropertyName = nameof(Customer.Email), Word = "E-MAIL" },

                //HDI
                new Keyword { Id = 1, CompanyId = 1, PropertyName = "StartIndex", Word = "DANE OSOBOWE" },
                new Keyword { Id = 2, CompanyId = 1, PropertyName = "FinishIndex", Word = "POJAZD" },
                new Keyword { Id = 3, CompanyId = 1, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 4, CompanyId = 1, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający" },
                new Keyword { Id = 5, CompanyId = 1, PropertyName = nameof(Customer.AddressId), Word = "Adres" },
                new Keyword { Id = 6, CompanyId = 1, PropertyName = nameof(Customer.AddressId), Word = "Adres korespondencyjny" },
                new Keyword { Id = 7, CompanyId = 1, PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania" },
                new Keyword { Id = 8, CompanyId = 1, PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania/korespondencyjny" },
                new Keyword { Id = 9, CompanyId = 1, PropertyName = nameof(Customer.CellPhone), Word = "Telefon komórkowy" },
                new Keyword { Id = 10, CompanyId = 1, PropertyName = nameof(Customer.HomePhone), Word = "Telefon stacjonarny" },
                new Keyword { Id = 11, CompanyId = 1, PropertyName = nameof(Customer.Email), Word = "E-MAIL" },
                
                //RESO
                new Keyword { Id = 12, CompanyId = 2, PropertyName = "StartIndex", Word = "Dane posiadaczy" },
                new Keyword { Id = 13, CompanyId = 2, PropertyName = "FinishIndex", Word = "Dane pojazdu" },
                new Keyword { Id = 14, CompanyId = 2, PropertyName = nameof(Customer.PESEL), Word = "Pesel/Regon/Data urodzenia " },
                new Keyword { Id = 15, CompanyId = 2, PropertyName = nameof(Customer.LastName), Word = "Imię i nazwisko / Nazwa " },
                new Keyword { Id = 16, CompanyId = 2, PropertyName = nameof(Customer.AddressId), Word = "Ulica, nr domu, lokalu " },
                new Keyword { Id = 17, CompanyId = 2, PropertyName = nameof(Customer.CellPhone), Word = "Nr telefonu " },
                new Keyword { Id = 18, CompanyId = 2, PropertyName = nameof(Customer.Email), Word = "E-mail " },

                //Proama
                new Keyword { Id = 19, CompanyId = 3, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 20, CompanyId = 3, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony " },
                new Keyword { Id = 21, CompanyId = 3, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony" },
                new Keyword { Id = 22, CompanyId = 3, PropertyName = nameof(Customer.BirthDate), Word = "data urodzenia" },
                new Keyword { Id = 23, CompanyId = 3, PropertyName = nameof(Customer.AddressId), Word = "adres" },
                new Keyword { Id = 24, CompanyId = 3, PropertyName = nameof(Customer.CellPhone), Word = "telefon" },
                
                //Compensa
                new Keyword { Id = 25, CompanyId = 4, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 26, CompanyId = 4, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony: właściciel" },
                new Keyword { Id = 26, CompanyId = 4, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczony" },
                new Keyword { Id = 27, CompanyId = 4, PropertyName = nameof(Customer.AddressId), Word = "Dane adresowe:" },
                new Keyword { Id = 28, CompanyId = 4, PropertyName = nameof(Customer.CellPhone), Word = "kom:" },
                
                //Allianz
                new Keyword { Id = 32, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "UBEZPIECZAJĄCY/WŁAŚCICIEL POJAZDU" },
                new Keyword { Id = 33, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "UBEZPIECZAJĄCY/WSPÓŁWŁAŚCICIEL" },
                new Keyword { Id = 34, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający / Ubezpieczony / Właściciel pojazdu:  " },
                new Keyword { Id = 35, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "Dane kontaktowe:" },
                new Keyword { Id = 36, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "WSPÓŁWŁAŚCICIEL" },
                new Keyword { Id = 37, CompanyId = 5, PropertyName = nameof(Customer.LastName), Word = "tel.:" },
                
                //AXA
                new Keyword { Id = 38, CompanyId = 6, PropertyName = "StartIndex", Word = "Dane właściciela pojazdu" },
                new Keyword { Id = 39, CompanyId = 6, PropertyName = "FinishIndex", Word = "1 z 2" },
                new Keyword { Id = 40, CompanyId = 6, PropertyName = nameof(Customer.LastName), Word = "Imiona i nazwisko / Nazwa:" },
                new Keyword { Id = 42, CompanyId = 6, PropertyName = nameof(Customer.AddressId), Word = "Adres zamieszkania / Siedziba:" },
                new Keyword { Id = 43, CompanyId = 6, PropertyName = nameof(Customer.CellPhone), Word = "Nr telefonu kontaktowego:" },
                new Keyword { Id = 44, CompanyId = 6, PropertyName = nameof(Customer.Email), Word = "Email:" },
                new Keyword { Id = 45, CompanyId = 6, PropertyName = nameof(Customer.BirthDate), Word = "Data urodzenia:" },

                //Euroins
                new Keyword { Id = 46, CompanyId = 7, PropertyName = "StartIndex", Word = "Okres ubezpieczenia:" },
                new Keyword { Id = 47, CompanyId = 7, PropertyName = "FinishIndex", Word = "Rok wydania prawa jazdy:" },
                new Keyword { Id = 48, CompanyId = 7, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający/Właściciel:" },
                new Keyword { Id = 49, CompanyId = 7, PropertyName = nameof(Customer.PESEL), Word = "PESEL:" },
                new Keyword { Id = 50, CompanyId = 7, PropertyName = nameof(Customer.AddressId), Word = "Adres:" },
                new Keyword { Id = 51, CompanyId = 7, PropertyName = nameof(Customer.CellPhone), Word = "telefon:" },
                new Keyword { Id = 52, CompanyId = 7, PropertyName = nameof(Customer.Email), Word = "email:" },
                new Keyword { Id = 53, CompanyId = 7, PropertyName = nameof(Customer.BirthDate), Word = "Data urodzenia:" },

                //Generali
                new Keyword { Id = 54, CompanyId = 8, PropertyName = nameof(Customer.LastName), Word = "(Details of Insurance Holder)" },
                new Keyword { Id = 55, CompanyId = 8, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający:" },
                new Keyword { Id = 56, CompanyId = 8, PropertyName = nameof(Customer.LastName), Word = "Ubezpieczający" },
                new Keyword { Id = 57, CompanyId = 8, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
                new Keyword { Id = 58, CompanyId = 8, PropertyName = nameof(Customer.AddressId), Word = "adres" },
                new Keyword { Id = 59, CompanyId = 8, PropertyName = nameof(Customer.CellPhone), Word = "telefon" },
                new Keyword { Id = 60, CompanyId = 8, PropertyName = nameof(Customer.Email), Word = "email" },
                new Keyword { Id = 61, CompanyId = 8, PropertyName = nameof(Customer.BirthDate), Word = "data urodzenia" },

                //GOTHAER
                new Keyword { Id = 62, CompanyId = 9, PropertyName = "StartIndex", Word = "osobowe" },
                new Keyword { Id = 63, CompanyId = 9, PropertyName = "FinishIndex", Word = "Zawód" },
                new Keyword { Id = 64, CompanyId = 9, PropertyName = nameof(Customer.LastName), Word = "Imię i Nazwisko" },
                new Keyword { Id = 65, CompanyId = 9, PropertyName = nameof(Customer.PESEL), Word = "Nr PESEL" },
                new Keyword { Id = 66, CompanyId = 9, PropertyName = nameof(Customer.AddressId), Word = "Adres" },
                new Keyword { Id = 67, CompanyId = 9, PropertyName = nameof(Customer.CellPhone), Word = "Telefon kontaktowy" },

                //WIENER
                new Keyword { Id = 68, CompanyId = 10, PropertyName = "StartIndex", Word = "osobowe" },
                new Keyword { Id = 69, CompanyId = 10, PropertyName = "FinishIndex", Word = "Zawód" },
                new Keyword { Id = 71, CompanyId = 10, PropertyName = nameof(Customer.LastName), Word = "Imię i Nazwisko" },
                new Keyword { Id = 72, CompanyId = 10, PropertyName = nameof(Customer.PESEL), Word = "Nr PESEL" },
                new Keyword { Id = 73, CompanyId = 10, PropertyName = nameof(Customer.AddressId), Word = "Adres" },
                new Keyword { Id = 74, CompanyId = 10, PropertyName = nameof(Customer.CellPhone), Word = "Telefon kontaktowy" },

                //ERGO Hestia
                new Keyword { Id = 75, CompanyId = 11, PropertyName = "StartIndex", Word = "Ubezpieczający/Ubezpieczony" },
                new Keyword { Id = 78, CompanyId = 11, PropertyName = "FinishIndex", Word = "Ubezpieczony pojazd" },
                new Keyword { Id = 79, CompanyId = 11, PropertyName = nameof(Customer.PESEL), Word = "PESEL" },
            };
        }

        public List<Keyword> GetKeywords()
        {
            LoadData();
            return Keywords;
        }
    }
}
