using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Topichat.Core {

	public interface IConversationManager 
    {
		/// <summary>
		/// Gets all conversations on the server that include the currently authenticated user.
		/// </summary>
		/// <remarks>
		/// The returned <c>Conversation</c> objects should be configured so that any
		///  messages added to the <c>Messages</c> collection will be propagated
		///  to the server, and likewise, any new messages on the server will be
		///  propagated to the local object's <c>Messages</c> collection.
		/// </remarks>
		/// <returns>An observable collection of matching conversations. As conversations
		///  are started or ended on the server, the collection should be updated.</returns>
		ObservableCollection<Conversation> GetConversations ();

		/// <summary>
		/// Starts or resumes a conversation with the given contact(s).
		/// </summary>
		/// <remarks>
		/// This is a synchronization point that can be used to ensure
		/// that we do not start multiple conversations between the same
		/// parties. The server will create a new <c>Conversation</c> for
		/// the first request it receives and then return that conversation
		/// for subsequent requests for the same set of <c>Contact</c>s.
		/// </remarks>
		/// <returns>The conversation.</returns>
		/// <param name="contact">Contact.</param>
		Task<Conversation> StartConversation (params Contact [] contacts); 
	}
}

