using System;
using System.Collections.ObjectModel;

namespace Chatopia
{
    public interface IContacts
    {
        /// <summary>
        /// Gets the <c>Contact</c> associated with the currently authenticated user.
        /// </summary>
        Contact Me { get; }

        /// <summary>
        /// Searches contacts on the server in the address book of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// The returned <c>Contact</c> objects should be configured so that any
        ///  changes on the server will be propagated to the local <c>Contact</c> object.
        /// </remarks>
        /// <returns>An observable collection of matching contacts. As more contacts are found
        ///   on the server, or as the searchTerm changes, the collection should be updated.</returns>
        ObservableCollection<Contact> GetContacts();
    }
}
