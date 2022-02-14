using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostViewModel> GetPost(Guid postGuid);
        Task<List<PostViewModel>> GetPosts(bool? isActive = null);
        Task<int> GetPostsCount(bool? isActive = null);
        Task<List<SelectListItem>> GetPostsNameSelectListItem(bool? isActive = null);
        Task<ResultViewModel> UpdatePost(Guid postGuid, object rawValue);
        Task<ResultViewModel> DeletePost(Guid postGuid);
    }
}
