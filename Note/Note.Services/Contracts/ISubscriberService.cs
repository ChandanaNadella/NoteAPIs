namespace Note.Services.Contracts
{
    using Note.API.Common.Helpers;
    using Note.Repository.Data.Entities;
    using DC = API.DataContracts;
    using System;

    public interface ISubscriberService
    {
        /// <summary>
        /// Registration.
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        Tuple<bool, string> Create(DC.Requests.SubscriberCreationRequest subscriberRequest);

        /// <summary>
        /// Get list of all subscribers.
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <returns></returns>
        Tuple<PagedList<Subscriber>, bool, string> GetAll(SubscriberResourceParameters pageparams);

        /// <summary>
        /// Get subscriber details by subscriber id.
        /// Shiva Ch, Bpktech. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tuple<Subscriber, bool, string> GetById(int id);
    }
}
