// ReSharper disable VirtualMemberCallInConstructor
namespace CarRental.Models
{
    using CarRental.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;
    using System;

    public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public virtual DateTime? DeletedOn { get; set; }
    }
}
