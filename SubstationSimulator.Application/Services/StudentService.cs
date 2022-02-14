using ElectricalEmulator.Application.Common.ViewModels;
using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultViewModel> DeleteStudent(string userId)
        {
            var user = await _unitOfWork.Users
                .GetUser(userId)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return ResultViewModel.Failure();
            }

            var userClasses = await _unitOfWork.UserClasses
                .GetUserClasses(userId)
                .ToListAsync();

            foreach (var userClass in userClasses)
            {
                var userClassPosts = await _unitOfWork.UserClassPosts
                    .GetUserClassPosts(userClass.UserClassGuid)
                    .ToListAsync();

                foreach (var userClassPost in userClassPosts)
                {
                    _unitOfWork.UserClassPosts.Delete(userClassPost);
                }

                _unitOfWork.UserClasses.Delete(userClass);
            }

            var userPosts = await _unitOfWork.UserPosts
                .GetUserPosts(userId)
                .ToListAsync();

            foreach (var userPost in userPosts)
            {
                _unitOfWork.UserPosts.Delete(userPost);
            }

            _unitOfWork.Users.Delete(user);

            await _unitOfWork.CommitAsync();

            return ResultViewModel.Success();
        }
    }
}
