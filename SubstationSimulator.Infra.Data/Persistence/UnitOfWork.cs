using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectricalEmulator.Infra.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ElectricalEmulatorContext _context;
        private IClassRepository _classRepository;
        private IUserClassRepository _userClassRepository;
        private IUserPostRepository _userPostRepository;
        private IUserClassPostRepository _userClassPostRepository;
        private IUserRepository _masterRepository;
        private IPostRepository _postRepository;
        private IInterlockRepository _interlockRepository;
        private IRoleRepository _roleRepository;
        private ISettingRepository _settingRepository;

        public UnitOfWork(ElectricalEmulatorContext context)
        {
            _context = context;
        }

        public IClassRepository Classes
        {
            get
            {
                _classRepository ??= new ClassRepository(_context);
                return _classRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                _masterRepository ??= new UserRepository(_context);
                return _masterRepository;
            }
        }

        public IPostRepository Posts
        {
            get
            {
                _postRepository ??= new PostRepository(_context);
                return _postRepository;
            }
        }

        public IUserClassRepository UserClasses
        {
            get
            {
                _userClassRepository ??= new UserClassRepository(_context);
                return _userClassRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                _roleRepository ??= new RoleRepository(_context);
                return _roleRepository;
            }
        }

        public IUserClassPostRepository UserClassPosts
        {
            get
            {
                _userClassPostRepository ??= new UserClassPostRepository(_context);
                return _userClassPostRepository;
            }
        }

        public IInterlockRepository Interlocks
        {
            get
            {
                _interlockRepository ??= new InterlockRepository(_context);
                return _interlockRepository;
            }
        }

        public IUserPostRepository UserPosts
        {
            get
            {
                _userPostRepository ??= new UserPostRepository(_context);
                return _userPostRepository;
            }
        }

        public ISettingRepository Settings
        {
            get
            {
                _settingRepository ??= new SettingRepository(_context);
                return _settingRepository;
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
