using InsuranceManagerApp.Models;
using InsuranceManagerApp.ViewModels;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InsuranceManagerApp
{
    public class ParsePdf
    {
        private readonly string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PlikiPdf");
        PropertyInfo[] properties = typeof(Customer).GetProperties();
        private List<Customer> customers = new List<Customer>();
        private List<Address> addresses = new List<Address>();

        public class ProgressEventArgs
        {
            public int Progress { get; set; }
        }

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        private static string ExtractTextFromPdf(string filePath)
        {
            PDDocument document = null;
            try
            {
                document = PDDocument.load(filePath);
                PDFTextStripper stripper = new PDFTextStripper();
                var text = stripper.getText(document);
                var whitespace = text.First(c => String.IsNullOrWhiteSpace(c.ToString()));
                return text.Replace(whitespace, ' ');
            }
            finally
            {
                if (document != null) document.close();
            }
        }
        private ObservableCollection<CustomerDataViewModel> customerDatas = new ObservableCollection<CustomerDataViewModel>();
        private IEnumerable<string> GetListOfFiles()
        {
            List<string> files = new List<string>();
            if (Directory.Exists(folderPath))
            {
                files = Directory.GetFiles(folderPath, "*.pdf").ToList();
            }

            return files;

        }
        public ObservableCollection<CustomerDataViewModel> ParseFiles()
        {
            var filesList = GetListOfFiles();
            var countParsedFiles = 0;
            var progress = 0;
            ProgressChanged?.Invoke(this, new ProgressEventArgs() { Progress = progress });
            foreach (var filePath in filesList)
            {
                countParsedFiles++;
                if(countParsedFiles * 100 / filesList.Count() > progress)
                {
                    progress = countParsedFiles * 100 / filesList.Count();
                    ProgressChanged?.Invoke(this, new ProgressEventArgs() { Progress = progress });
                }
                var documentText = ExtractTextFromPdf(filePath);
                var documentLines = documentText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                documentText = documentText.ToLower();
                var _company = new Company();
                var companies = _company.GetCompanies();
                Company company = new Company();
                foreach(var item in companies)
                {
                    if(documentText.Contains(item.KRS) && documentText.Replace("-", " ").Contains(item.NIP) && documentText.Contains(item.Name.ToLower()))
                    {
                        company = item;
                        break;
                    }
                }
                if(company != null && documentText != null) ParseData(documentLines, company);
            };//parsing evry file
            var addressProps = typeof(Address).GetProperties();
            var viewModelProperties = typeof(CustomerDataViewModel).GetProperties();
            Customer customer = new Customer();
            foreach (var item in customers)
            { 
                var customerData = new CustomerDataViewModel();
                
                foreach(var vmProp in viewModelProperties)
                {
                    foreach(var property in properties)
                    {
                        dynamic value;
                        if (property.Name == nameof(customer.AddressId))
                        {
                            var address = addresses.First(a => a.Id == item.AddressId);
                            foreach(var prop in addressProps)
                            {
                                if (prop.Name == vmProp.Name)
                                {
                                    value = prop.GetValue(address);
                                    vmProp.SetValue(customerData, value);
                                }
                            }
                        }//adding address data to view model
                        else if(property.Name == vmProp.Name)
                        {
                            value = property.GetValue(item);
                            vmProp.SetValue(customerData, value);
                        }//adding customer data
                    }
                }
                customerDatas.Add(customerData);
            } //adding parsed customers to observableCollection
            return customerDatas;
        }
        
        

        private void ParseData(List<string> documentLines, Company company)
        {
            var _keyword = new Keyword();
            List<Keyword> keywords = _keyword.GetKeywords().Where(k => k.CompanyKrs == company.KRS).ToList();

            Customer customer = new Customer();

            try
            {
                if (company.Name == "HDI")
                {
                    customer = ParseHdiPdf(documentLines, keywords);
                }//parse HDI
                else if (company.Name == "Warta")
                {
                    customer = ParseHdiPdf(documentLines, keywords);
                }//parse Warta
                else if (company.Name == "RESO")
                {
                    customer = ParseResoPdf(documentLines, keywords);
                }//parse RESO
                else if (company.Name == "Proama")
                {
                    customer = ParseProamaPdf(documentLines, keywords);
                }//parse RESO
                else if (company.Name == "Compensa")
                {
                    customer = ParseCompensaPdf(documentLines, keywords);
                }
                else if (company.Name == "Allianz")
                {
                    customer = ParseAllianzPdf(documentLines, keywords);
                }
            }
            catch
            {
                return;
            }

            //cut phone number if too long or in bad type of phone
            customer = CorrectPhoneNumber(customer);

            //generate birth date from pesel 
            if (customer.PESEL != null && customer.BirthDate == null) customer.BirthDate = BirthDateFromPesel(customer.PESEL);

            if (customer.FirstName == null || customer.LastName == null || customer.BirthDate == null || customer.AddressId == null) customer = null;


            if (customer != null)
            {
                //generate Id from customer data
                var dataToGenerateId = customer.FirstName + customer.LastName + customer.BirthDate + customer.AddressId;
                customer.Id = GenerateHash(dataToGenerateId);

                //add or update customer
                if (customer != null) UpdateOrAddCustomer(customer);
            }
        }

        private void UpdateOrAddCustomer(Customer customer)
        {
            var customerExist = CustomerExist(customer.Id);
            if (!customerExist) customers.Add(customer);
            else
            {
                var existingCustomer = customers.First(c => c.Id == customer.Id);
                var properties = typeof(Customer).GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetValue(existingCustomer) == null && property.GetValue(customer) != null)
                    {
                        dynamic value = property.GetValue(customer);
                        property.SetValue(existingCustomer, value);
                    }
                }
            }
        }//update data if null or add customer if not exist

        private bool CustomerExist(string id)
        {
            bool status = false;
            foreach(var customer in customers)
            {
                if (id == customer.Id) status = true;
            }
            return status;
        }//check if customer exist

        private DateTime BirthDateFromPesel(string pesel)
        {
            var year = Convert.ToInt32(pesel.Substring(0, 2));
            var month = Convert.ToInt32(pesel.Substring(2, 2));
            var day = Convert.ToInt32(pesel.Substring(4, 2));
            if(month > 12)
            {
                year += 2000;
                month -= 20;
            }
            else
            {
                year += 1900;
            }
            var birthDate = new DateTime(year, month, day);
            return birthDate;
        }//Generate date of birth from pesel

        private Customer CorrectPhoneNumber(Customer customer)
        {
            var cellPhonePrefixes = new List<string>()
            {
                "45",
                "50",
                "51",
                "53",
                "57",
                "60",
                "66",
                "69",
                "72",
                "73",
                "78",
                "79",
                "88"
            };
            string tmpNumber = null;
            var cellPhone = customer.CellPhone;
            if (!String.IsNullOrEmpty(cellPhone))
            {
                if(cellPhone.Length > 9) cellPhone = cellPhone.Substring(cellPhone.Length - 9);
                if (!cellPhonePrefixes.Contains(cellPhone.Substring(0, 2)))
                {
                    tmpNumber = cellPhone;
                    cellPhone = null;
                }
            }
            var homePhone = customer.HomePhone;
            if (!String.IsNullOrEmpty(homePhone))
            {
                if(homePhone.Length > 9) homePhone = homePhone.Substring(homePhone.Length - 9);
                if (cellPhonePrefixes.Contains(homePhone.Substring(0, 2)))
                {
                    if ( cellPhone == null ) cellPhone = homePhone;
                    homePhone = null;
                }
            }
            if(homePhone == null) homePhone = tmpNumber;

            customer.CellPhone = cellPhone;
            customer.HomePhone = homePhone;


            return customer;
        }//edit phone number if too long

        private string ParseAddress(string addressToParse)
        {
            var splittedAddress = Regex.Split(RemoveBoundaryWhitespace(addressToParse), ", ");
            var index = splittedAddress[0].IndexOf(' ');
            var zipCode = RemoveBoundaryWhitespace(splittedAddress[0].Substring(0, index).ToUpper());
            var city = RemoveBoundaryWhitespace(splittedAddress[0].Substring(index+1).ToUpper());
            var house = RemoveBoundaryWhitespace(splittedAddress[1].ToUpper());
            if (house.Contains("UL. " + city)) house = house.Replace("UL. " + city + " ", "");
            else if (house.Contains(city)) house = house.Replace(city + " ", "");
            var addressToGenerateHash = zipCode + city + house;
            var hash = GenerateHash(addressToGenerateHash);
            Address address = new Address()
            {
                City = city,
                House = house,
                ZipCode = zipCode,
                Id = hash
            };
            if(!addresses.Any(a => a.Id == address.Id)) addresses.Add(address);
            return address.Id;
        }//create address variable from string data

        static string GenerateHash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }//generate hash from string

        private string RemoveBoundaryWhitespace(string text)
        {
            if (String.IsNullOrWhiteSpace(text)) return text;
            while (String.IsNullOrWhiteSpace(text.First().ToString()))
            {
                text = text.Substring(1);
            }
            while (String.IsNullOrWhiteSpace(text.Last().ToString()))
            {
                var length = text.Length;
                text = text.Substring(0, length - 1);
            }
            return text;
        }

        //------------------------------------------------------------------------
        //Companies parse methods

        private Customer ParseHdiPdf(List<string> documentLines, List<Keyword> keywords)
        {
            Customer customer = new Customer();

            var startIndex = documentLines.LastIndexOf(keywords.First(w => w.PropertyName == "StartIndex").Word);
            var finishIndex = documentLines.LastIndexOf(keywords.First(w => w.PropertyName == "FinishIndex").Word);
            string customerDataString = "";
            for (int index = startIndex + 1; index < finishIndex; index++)
            {
                customerDataString += documentLines[index];
                if (documentLines[index].Last() != '-') customerDataString += " ";
            }


            var customerData = Regex.Split(customerDataString, "[,:] | Nr rejestracyjny.+").Where(w => !String.IsNullOrWhiteSpace(w)).ToList();

            for (int index = 0; index < customerData.Count - 1; index++)
            {
                foreach (var keyword in keywords)
                {
                    if (customerData[index] == keyword.Word)
                    {
                        dynamic propertyValue;
                        foreach (var property in properties)
                        {
                            if (property.Name == keyword.PropertyName)
                            {
                                if (property.GetValue(customer) != null) break;
                                
                                propertyValue = customerData[++index];

                                if (propertyValue == "") break;

                                if (property.Name == nameof(customer.AddressId))
                                {
                                    var address = propertyValue + ", " + customerData[++index]; //poczta, adres
                                    propertyValue = ParseAddress(address);
                                }

                                if (property.Name == nameof(customer.BirthDate)) propertyValue = DateTime.Parse(propertyValue);

                                if (property.Name == nameof(customer.Email)) propertyValue = propertyValue.ToLower();

                                if (property.Name == nameof(customer.LastName))
                                {
                                    var name = propertyValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    customer.FirstName = name[1].ToUpper();
                                    customer.LastName = name[0].ToUpper();
                                }
                                else
                                {
                                    if (propertyValue.GetType() == typeof(string))
                                    {
                                        propertyValue = RemoveBoundaryWhitespace(propertyValue);
                                        if (propertyValue.ToLower() == "brak") propertyValue = null;
                                    }
                                    property.SetValue(customer, propertyValue);
                                }
                                break;
                            }
                        }
                    }

                }
            }
            return customer;
        }//parsing HDI files

        private Customer ParseResoPdf(List<string> documentLines, List<Keyword> keywords)
        {
            Customer customer = new Customer();
            var startIndex = documentLines.IndexOf(keywords[0].Word);
            var finishIndex = documentLines.IndexOf(keywords[1].Word);

            var customerData = documentLines.GetRange(startIndex + 1, finishIndex - startIndex - 1);

            for (int index = 0; index < customerData.Count; index++)
            {
                foreach (var keyword in keywords)
                {
                    if (customerData[index].Contains(keyword.Word))
                    {
                        dynamic propertyValue;
                        foreach (var property in properties)
                        {
                            if (property.Name == keyword.PropertyName)
                            {
                                if (property.GetValue(customer) != null) break;

                                propertyValue = customerData[index].Replace(keyword.Word, "");
                                var text = propertyValue as string;
                                var count = Convert.ToDouble(text.Where(t => t == ' ').Count()) / 2;
                                string pattern = string.Concat(Enumerable.Repeat(".+? ", (int)Math.Ceiling(count)));
                                if (!String.IsNullOrWhiteSpace(pattern) && text.Where(t => t == ' ').Count() != 1)
                                {
                                    propertyValue = Regex.Match(propertyValue, pattern).Value;
                                }
                                else if(property.Name != nameof(customer.LastName) && property.Name != nameof(customer.AddressId))
                                {
                                    propertyValue = propertyValue.Split(' ')[0];
                                }

                                if (propertyValue == "") break;

                                if (property.Name == nameof(customer.AddressId))
                                {
                                    var address = customerData[++index].Replace("Kod pocztowy, Miejscowość ", "").Replace(", ", " ");
                                    var countAddress = Convert.ToDouble(address.Where(c => c == ' ').Count()) / 2;
                                    string patternAddress = string.Concat(Enumerable.Repeat(".+? ", (int)Math.Ceiling(countAddress)));
                                    if (!String.IsNullOrWhiteSpace(patternAddress) && address.Where(d => d == ' ').Count() != 1) address = Regex.Match(address, patternAddress).Value;
                                    if (address.EndsWith(" ")) address = RemoveBoundaryWhitespace(address);
                                    if (propertyValue.EndsWith(" ")) propertyValue = RemoveBoundaryWhitespace(propertyValue);
                                    address = address + ", " + propertyValue;
                                    propertyValue = ParseAddress(address);
                                }

                                if (property.Name == nameof(customer.BirthDate)) propertyValue = DateTime.Parse(propertyValue);

                                if (property.Name == nameof(customer.Email)) propertyValue = propertyValue.ToLower();

                                if (property.Name == nameof(customer.LastName))
                                {
                                    var name = propertyValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    customer.FirstName = name[0].ToUpper();
                                    customer.LastName = name[1].ToUpper();
                                }
                                else
                                {
                                    if (propertyValue.GetType() == typeof(string))
                                    {
                                        propertyValue = RemoveBoundaryWhitespace(propertyValue);
                                        if (propertyValue.ToLower() == "brak") propertyValue = null;
                                    }
                                    property.SetValue(customer, propertyValue);
                                }
                                
                                break;
                            }
                        }
                    }
                }
            }

            return customer;
        }//parsing RESO files

        private Customer ParseProamaPdf(List<string> documentLines, List<Keyword> keywords)
        {
            Customer customer = new Customer();

            var customerDataString = documentLines.First(l => l.Contains(keywords.Last(k => k.PropertyName == nameof(customer.LastName)).Word));
            var customerData = Regex.Split(customerDataString.Normalize(), "[,:] ").Where(w => !String.IsNullOrWhiteSpace(w)).ToList();

            var properties = typeof(Customer).GetProperties();
            for(int index = 0; index < customerData.Count; index++)
            {
                foreach(var keyword in keywords)
                {
                    if(customerData[index].Contains(keyword.Word))
                    {
                        dynamic propertyValue;
                        foreach(var property in properties)
                        {
                            if(property.Name == keyword.PropertyName)
                            {
                                if (property.GetValue(customer) != null) break;

                                if (property.Name == nameof(customer.CellPhone) && customerData[index].Length > keyword.Word.Length) index--;
                                propertyValue = customerData[++index];

                                if (propertyValue == "") break;

                                if (property.Name == nameof(customer.AddressId))
                                {
                                    var address = customerData[++index] + ", " + propertyValue;
                                    propertyValue = ParseAddress(address);
                                }

                                if (property.Name == nameof(customer.BirthDate)) propertyValue = DateTime.Parse(propertyValue);

                                if (property.Name == nameof(customer.Email)) propertyValue = propertyValue.ToLower();
                                
                                if(property.Name == nameof(customer.LastName))
                                {
                                    if (keyword.Word.Contains(' ')) propertyValue = customerData[--index].Replace(keyword.Word, "");
                                    var name = propertyValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    customer.FirstName = name[0].ToUpper();
                                    customer.LastName = name[1].ToUpper();
                                }
                                else
                                {
                                    if (propertyValue.GetType() == typeof(string))
                                    {
                                        propertyValue = RemoveBoundaryWhitespace(propertyValue);
                                        if (propertyValue.ToLower() == "brak") propertyValue = null;
                                    }
                                    property.SetValue(customer, propertyValue);
                                }
                                break;
                            }
                        }

                    }
                }
            }

            return customer;
        }//parsing Proama files

        private Customer ParseCompensaPdf(List<string> documentLines, List<Keyword> keywords)//parsing Compensa files
        {
            Customer customer = new Customer();
            string customerDataString;
            var lastNameLine = documentLines.First(l => l.Contains(keywords.Last(k => k.PropertyName == nameof(customer.LastName)).Word));
            
            var index = documentLines.IndexOf(lastNameLine) - 1;
            customerDataString = lastNameLine + ", " + documentLines[index];
            var customerData = customerDataString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (index = 0; index < customerData.Count; index++)
            {
                foreach (var keyword in keywords)
                {
                    if (customerData[index].Contains(keyword.Word))
                    {
                        dynamic propertyValue;
                        foreach (var property in properties)
                        {
                            if (property.Name == keyword.PropertyName)
                            {
                                if (property.GetValue(customer) != null) break;

                                if(customerData[index].Length > keyword.Word.Length) index--;
                                propertyValue = customerData[++index].Replace(keyword.Word, "");

                                if (propertyValue == "") break;

                                if (property.Name == nameof(customer.AddressId))
                                {
                                    var address = customerData[++index] + ", " + propertyValue; //poczta, adres
                                    if (!address.Contains("-"))
                                    {
                                        var postOffice = RemoveBoundaryWhitespace(customerData[++index]);
                                        var city = RemoveBoundaryWhitespace(customerData[--index]);
                                        var street = RemoveBoundaryWhitespace(propertyValue);
                                        if (street.Contains(city)) city = "";
                                        address = postOffice + ", " + city + " " + street;
                                    }
                                    propertyValue = ParseAddress(address);
                                }

                                if (property.Name == nameof(customer.BirthDate)) propertyValue = DateTime.Parse(propertyValue);

                                if (property.Name == nameof(customer.Email)) propertyValue = propertyValue.ToLower();

                                if (property.Name == nameof(customer.LastName))
                                {
                                    var name = propertyValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    customer.FirstName = name[0].ToUpper();
                                    customer.LastName = name[1].ToUpper();
                                }
                                else
                                {
                                    if (propertyValue.GetType() == typeof(string))
                                    {
                                        propertyValue = RemoveBoundaryWhitespace(propertyValue);
                                        if (propertyValue.ToLower() == "brak") propertyValue = null;
                                    }
                                    property.SetValue(customer, propertyValue);
                                }
                                break;
                            }
                        }
                    }

                }
            }

            return customer;
        }

        private Customer ParseAllianzPdf(List<string> documentLines, List<Keyword> keywords)
        {
            string customerString = "";
            for (int i = 0; i < documentLines.Count; i++)
            {
                foreach(var keyword in keywords)
                {
                    string text = "null";
                    if (documentLines[i].Contains(keyword.Word) && !documentLines[i].Contains("509353195"))
                    {
                        var replaced = documentLines[i].Replace(keyword.Word, "");
                        if (replaced.Replace(" ", "") == "")
                        {
                            text = documentLines[++i] + " " + documentLines[++i];
                        }
                        else
                        {
                            text = replaced;
                        }
                        if(keyword.Id == 36)
                        {
                            Console.WriteLine("Współwłaściciel" + text);
                        }
                        else if (!customerString.Contains(text)) customerString += ", " + text;

                        break;
                    }
                }
            }
            var customerData = customerString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var name = RemoveBoundaryWhitespace(customerData[0]);
            var firstName = name.Split(' ')[0];
            var lastName = name.Split(' ')[1];
            var pesel = RemoveBoundaryWhitespace(customerData.First(d => d.ToLower().Contains("pesel")));
            pesel = pesel.Remove(0, pesel.Length - 11);
            string address = "";
            string postOffice = "";
            if (!customerData[2].Contains("data urodzenia"))
            {
                address = RemoveBoundaryWhitespace(customerData[2]);
                postOffice = RemoveBoundaryWhitespace(customerData[3]);
            }
            else
            {
                address = RemoveBoundaryWhitespace(customerData[3]);
                postOffice = RemoveBoundaryWhitespace(customerData[4]);
            }
            var fullAddress = postOffice + ", " + address;
            var addressId = ParseAddress(fullAddress);
            var phone = RemoveBoundaryWhitespace(customerData.FirstOrDefault(c => c.Contains("+")));

            Customer customer = new Customer()
            {
                FirstName = firstName.ToUpper(),
                LastName = lastName.ToUpper(),
                PESEL = pesel,
                AddressId = addressId,
                CellPhone = phone
            };

            return customer;
        }
    }
}
