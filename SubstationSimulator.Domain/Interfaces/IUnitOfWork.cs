using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClassRepository Classes { get; }
        IUserClassRepository UserClasses { get; }
        IUserPostRepository UserPosts { get; }
        IUserClassPostRepository UserClassPosts { get; }
        IUserRepository Users { get; }
        IPostRepository Posts { get; }
        IInterlockRepository Interlocks { get; }
        IRoleRepository Roles { get; }
        ISettingRepository Settings { get; }
        Task CommitAsync();
    }
}
