using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Domain.Users.Events;

namespace Ssa.CarSharing.Users.Domain.Users
{
    public class User : Entity
    {
        private User(Guid id, string firstName, string lastName, string email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string IdentityId { get; set; }

        public static User  Create(string firstName, string lastName, string email)
        {
            
            if(string.IsNullOrWhiteSpace(firstName)) 
                throw new ArgumentNullException(nameof(firstName));
            if(string.IsNullOrWhiteSpace(lastName)) 
                throw new ArgumentNullException(nameof(lastName));
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            User user = new User(Guid.NewGuid(), firstName, lastName, email);
            
            user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));

            return user;
        }

    }
}
