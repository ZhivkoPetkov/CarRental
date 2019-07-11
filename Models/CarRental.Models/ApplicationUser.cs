// ReSharper disable VirtualMemberCallInConstructor
namespace CarRental.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {        
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Orders = new HashSet<Order>();
            this.Vouchers = new HashSet<Voucher>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }

    }
}
