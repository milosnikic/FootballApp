using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Groups
{
    public interface IGroupsService
    {
        Task<DetailGroupToReturnDto> GetGroup(int id, int userId);
        Task<IEnumerable<GroupToReturnDto>> GetGroupsForUser(int userId);
        Task<IEnumerable<GroupToReturnDto>> GetFavoriteGroupsForUser(int userId);
        Task<KeyValuePair<bool, string>> CreateGroup(int userId, GroupForCreationDto group);
        Task<IEnumerable<GroupToReturnDto>> GetAllGroups(int userId);
        Task<IEnumerable<GroupToReturnDto>> GetAllCreatedGroups(int userId);
        Task<KeyValuePair<bool, string>> LeaveGroup(int groupId, int userId);
        Task<KeyValuePair<bool, string>> RequestJoinGroup(int groupId, int userId);
        Task<KeyValuePair<bool, string>> AcceptUser(int groupId, int userId);
        Task<KeyValuePair<bool, string>> RejectUser(int groupId, int userId);
        Task<KeyValuePair<bool, string>> MakeFavoriteGroup(int groupId, int userId, bool favorite);
        Task<MembershipInformationDto> GetMembershipInformation(int userId, int groupId);
    }
}