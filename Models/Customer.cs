using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceManagerApp.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string PESEL { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string CellPhone { get; set; }

        public string HomePhone { get; set; }

        public string Email { get; set; }

        public string AddressId { get; set; }
    }
}


//HDI
/*
 *Ubezpieczajacy: - Nazwisko Imię(usuń przecinek)
 * PESEL: pesel(usuń przecinek)
 * Adres:(lub zamieszkania/korespondencyjny:) kod pocztowy Miejscowość(zmniejsz litery, usuń przecinek) adres
 * komórkowy: numer(9 ostatnich cyfr(usuwanie kierunkowego), usuń przecinek)
 * stacjonarny: numer(usuń przecinek)
 * E-MAIL: adres e-mail
 */

//Ubezpieczony: WOJCIECH SKRZEKUT, data urodzenia: 24.10.1974, adres: MORDARKA 141, 34-600 MORDARKA

//"Dane adresowe: ROZTOKA 68, 34-606 ŁUKOWICA, kom: 500872293"
//"Ubezpieczony: właściciel,  MATEUSZ DANIEL, PESEL 83072318192  "