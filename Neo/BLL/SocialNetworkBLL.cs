using DAL;
using SocialNetwork;

namespace BLL
{
    public class SocialNetworkBLL
    {
        private readonly SocialNetworkRepositoryNeo _socialNetworkService;
        private readonly SocialNetworkRepository _userService;

        public SocialNetworkBLL(SocialNetworkRepositoryNeo socialNetworkService, SocialNetworkRepository userService)
        {
            _socialNetworkService = socialNetworkService;
            _userService = userService;
        }


        public bool AreConnected(string currentUserEmail, string otherUserEmail)
        {
            return 
        }

            public int CalculateDistanceToUser(string currentUserEmail, string otherUserEmail)
        {
            return _socialNetworkService.CalculateDistance(currentUserEmail, otherUserEmail);
        }
    }
}