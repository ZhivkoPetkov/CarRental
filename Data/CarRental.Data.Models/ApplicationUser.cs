// ReSharper disable VirtualMemberCallInConstructor
namespace CarRental.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CarRental.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo
    {
        public ApplicationUser()
        {        
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Orders = new HashSet<Order>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
