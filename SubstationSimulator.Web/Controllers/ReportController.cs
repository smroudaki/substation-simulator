using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Application.Common.Extensions;
using ElectricalEmulator.Application.ViewModels.Classes;
using ElectricalEmulator.Application.ViewModels.Posts;
using ElectricalEmulator.Application.ViewModels.Reports;
using ElectricalEmulator.Application.ViewModels.Users;
using ElectricalEmulator.Infra.Data.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectricalEmulator.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ElectricalEmulatorContext _context;

        public ReportController(ElectricalEmulatorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reports = await _context.Report
                .OrderByDescending(r => r.CreationDate)
                .Select(r => new ReportViewModel
                {
                    ReportId = r.ReportId,
                    ReportGuid = r.ReportGuid,
                    User = new UserViewModel
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        PhoneNumber = r.User.PhoneNumber,
                        RemainingTime = r.User.RemainingTime.HasValue ? r.User.RemainingTime.Value.ToString() : Constants.NotSet,
                        IsActive = r.User.IsActive ? Constants.Active : Constants.InActive,
                        CreationDate = PersianDateExtensions.ToPeString(r.User.CreationDate, "yyyy/MM/dd HH:mm"),
                        ModifiedDate = r.User.ModifiedDate == null ?
                            Constants.NotSet : PersianDateExtensions.ToPeString(r.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    },
                    //Post = new PostViewModel
                    //{
                    //    PostId = r.Post.PostId,
                    //    PostGuid = r.Post.PostGuid,
                    //    RawValue = r.Post.RawValue,
                    //    RawValueDeserialized = new PostRawValueViewModel
                    //    {

                    //    },
                    //    IsActive = r.Post.IsActive ? Constants.Active : Constants.InActive,
                    //    CreationDate = PersianDateExtensions.ToPeString(r.Post.CreationDate, "yyyy/MM/dd HH:mm"),
                    //    ModifiedDate = r.Post.ModifiedDate == null ?
                    //        Constants.NotSet : PersianDateExtensions.ToPeString(r.Post.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    //},
                    //Class = new ClassViewModel
                    //{
                    //    ClassId = r.Class.ClassId,
                    //    ClassGuid = r.Class.ClassGuid,
                    //    User = new UserViewModel
                    //    {
                    //        Id = r.Class.User.Id,
                    //        FirstName = r.Class.User.FirstName,
                    //        LastName = r.Class.User.LastName,
                    //        PhoneNumber = r.Class.User.PhoneNumber,
                    //        IsActive = r.Class.User.IsActive ? Constants.Active : Constants.InActive,
                    //        CreationDate = PersianDateExtensions.ToPeString(r.Class.User.CreationDate, "yyyy/MM/dd HH:mm"),
                    //        ModifiedDate = r.Class.User.ModifiedDate == null ?
                    //            Constants.NotSet : PersianDateExtensions.ToPeString(r.Class.User.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    //    },
                    //    Title = r.Class.Title,
                    //    Description = r.Class.Description,
                    //    CreationDate = PersianDateExtensions.ToPeString(r.Class.CreationDate, "yyyy/MM/dd HH:mm"),
                    //    ModifiedDate = r.Class.ModifiedDate == null ?
                    //        Constants.NotSet : PersianDateExtensions.ToPeString(r.Class.ModifiedDate.Value, "yyyy/MM/dd HH:mm")
                    //},
                    Value = r.Value,
                    CreationDate = PersianDateExtensions.ToPeString(r.CreationDate, "yyyy/MM/dd HH:mm")

                }).ToListAsync();

            return PartialView(reports);
        }
    }
}
