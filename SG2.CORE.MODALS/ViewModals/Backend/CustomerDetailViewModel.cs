using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
   public class CustomerDetailViewModel
    {
        public string Id { get; set; }
        public string SocialProfileId { get; set; }
        public string SocialProfileName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string JVBoxNo { get; set; }
        public string IPNo { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string AddressLine { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string InstaUsrName { get; set; }
        public string InstaPassword { get; set; }
        public string UpdatedOn { get; set; }
        public string JVStatus { get; set; }
        public bool OptedEdEmailSeries { get; set; }
        public bool OptedMarkEmail { get; set; }
        public string Source { get; set; }
        public string SourceText { get; set; }
        public string Register { get; set; }
        public int ResTeamMember { get; set; }
        public bool AvaToEveryOne { get; set; }
        public string Comment { get; set; }
        public IList<UserDTO> usersList { get; set; }
        public IList<CountryDTO> Countries { get; set; }
        public IList<CityDTO> cities { get; set; }
        public IList<TitleDTO> CustomerTitles { get; set; }

        public IList<StatusDTO> statusDTOs { get; set; }
        public string RoleName { get; set; }
        public string CustomerStatus { get; set; }

        public bool IsArchived { get; set; }
        public string ProxyPort { get; set; }
    }
}
