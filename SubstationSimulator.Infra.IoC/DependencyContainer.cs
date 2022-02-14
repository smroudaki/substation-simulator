using ElectricalEmulator.Application.Interfaces;
using ElectricalEmulator.Application.Services;
using ElectricalEmulator.Domain.Interfaces;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            #region Application Layer

            service.AddScoped<IClassService, ClassService>();
            service.AddScoped<IUserClassService, UserClassService>();
            service.AddScoped<IUserClassPostService, UserClassPostService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IMasterService, MasterService>();
            service.AddScoped<IStudentService, StudentService>();
            service.AddScoped<IPostService, PostService>();
            service.AddScoped<IInterlockService, InterlockService>();
            service.AddScoped<IUserPostService, UserPostService>();
            service.AddScoped<ISettingService, SettingService>();

            #endregion

            #region Infra Data Layer

            service.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion
        }
    }
}
