using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Topichat.Core
{

    public class Contact : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the identifier for this <c>Contact</c>.
        /// </summary>
        /// <remarks>
        /// This property uniquely identifies a specific contact on the server.
        /// </remarks>
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNumber)));
                }
            }
        }

        string phoneNumber;

        /// <summary>
        /// Gets or sets the first name for this <c>Contact</c>.
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }
        string firstName;

        /// <summary>
        /// Gets or sets the last name for this <c>Contact</c>.
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }
        string lastName;

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        public string Initials => $"{FirstName[0]}{LastName[0]}";

        public string FullName => $"{FirstName} {LastName}";

        public string QuickSearchIndex => FirstName[0].ToString();

        public string ImageUrl => "unknowncontact.png";

        bool selected;
        public bool Selected
		{
            get { return selected; }
			set
			{
                if (selected != value)
				{
                    selected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
				}
			}
		}
    }

    class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

			return x.PhoneNumber == y.PhoneNumber;
        }

        public int GetHashCode(Contact contact)
        {
			return Object.ReferenceEquals(contact, null) ? 0 : contact.PhoneNumber.GetHashCode();
		}
    }
}