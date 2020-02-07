using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
namespace Dal
{

    class DalXml : IDal
    {

        static readonly DalXml instance = new DalXml();
        PersonXmlHandler personHandler = new PersonXmlHandler();
        GuestRequestXmlHandler guestRequestHandler = new GuestRequestXmlHandler();

        public static List<Person> persons;
        public static List<Host> hosts;
        public static List<HostingUnit> hostingUnits;
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
        public static List<BankBranch> bankBranches;
        public string
           HostingUnitPath = @"../../../../XMLFiles/HostingUnitXML.xml",
           AdminPath = @"../../../../XMLFiles/AdminXML.xml",
           HostPath = @"../../../../XMLFiles/HostXML.xml",
           OrderdPath = @"../../../../XMLFiles/OrderXML.xml",
           BankBranchPath = @"../../../../XMLFiles/BranchesXML.xml",
           ConfigurationPath = @"../../../../XMLFiles/ConfigurationXML.xml";

        #region singelton
        static DalXml() { }
        DalXml()
        {
            try
            {
                if (!File.Exists(personHandler.PersonPath))
                    personHandler.CreatePersonFile();
            }
            catch { }
        }
        public static DalXml Instance { get { return instance; } }

        #endregion

        #region Person Function functions

        public bool ChaeckPersonMail(string mail)
        {
            personHandler.load();
            return persons.Any(x => mail == x.Email);
        }
        public Person GetPersonById(string Id)
        {
            personHandler.load();
            Person person = persons.FirstOrDefault(x => Id == x.Id);
            return person == null ? throw new MissingMemberException("Person ID", Id) : person.Clone();
        }

        public Person GetPersonByMail(string mail)
        {
            personHandler.load();
            Person person = persons.FirstOrDefault(x => mail == x.Email);
            return person == null ? throw new MissingMemberException("Person ID", mail) : person.Clone();
        }

        public void AddPerson(Person person)
        {
            personHandler.load();
            if (persons.Any(x => person.Id == x.Id))
                throw new DuplicateKeyException(person.Id, " " + person.Id + " Person ID");
            persons.Add(person.Clone());
            personHandler.Save();
        }

        public IEnumerable<Person> GetPersonsByName(string fullName)
        {
            personHandler.load();
            var personsList = from item in persons
                              let FullName = item.FirstName + " " + item.LastName
                              where FullName == fullName
                              select item.Clone();
            return persons.Count() > 0 ? persons : throw new MissingMemberException("person", fullName);
        }

        public void UpdatePerson(Person person)
        {
            personHandler.load();
            int count = persons.RemoveAll(x => x.Id == person.Id);
            if (count == 0)
                throw new MissingMemberException("Person ID", person.Id);
            persons.Add(person.Clone());
            personHandler.Save();
        }

        public void UpdateStatusPerson(string id, Status status)
        {
            personHandler.load();
            bool found = false;
            foreach (Person item in persons)
            {
                if (item.Id == id)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Person ID", id);
            else
                personHandler.Save();
        }
        #endregion

        #region Guest Request functions
        public GuestRequest GetRequest(uint Key)
        {
            guestRequestHandler.load();
            GuestRequest request = guestRequests.FirstOrDefault(x => Key == x.Key);
            return request == null ?
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key)) : request.Clone();
        }

        public IEnumerable<GuestRequest> GetAllRequestsOfClient(string ClientId)
        {
            guestRequestHandler.load();
            var requsts = from item in guestRequests
                          where item.ClientId == ClientId
                          select item.Clone();
            if (requsts.Any())
                throw new MissingMemberException("Booking requsts of Client ID", ClientId);
            return requsts;
        }

        public uint AddGuestRequest(GuestRequest request)
        {
            guestRequestHandler.load();
            if (request.Key == 0)
                request.Key = Configuration.GuestRequestserialKey++;
            if (guestRequests.Any(x => request.Key == x.Key)) // if there is a problem white the serialNumber
                throw new DuplicateKeyException(Convert.ToString(request.Key), "" + Convert.ToString(request.Key) + " Guest Request Key");
            guestRequests.Add(request.Clone());
            guestRequestHandler.Save();
            return request.Key;
        }

        public void UpdateGuestRequest(GuestRequest request)
        {
            guestRequestHandler.load();
            int count = guestRequests.RemoveAll(x => x.Key == request.Key);
            if (count == 0)
                throw new MissingMemberException("Request Key", Convert.ToString(request.Key));
            guestRequests.Add(request.Clone());
            guestRequestHandler.Save();
        }

        public void UpdateStatusRequest(uint Key, RequestStatus status)
        {
            guestRequestHandler.load();
            bool found = false;
            foreach (GuestRequest item in guestRequests)
            {
                if (item.Key == Key)
                {
                    found = true;
                    item.Status = status;
                }
            }
            if (!found)
                throw new MissingMemberException("Guest Request Key", Convert.ToString(Key));
            else
                guestRequestHandler.Save();
        }
        #endregion



    }
}
