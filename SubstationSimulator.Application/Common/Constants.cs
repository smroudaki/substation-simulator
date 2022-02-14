using ElectricalEmulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectricalEmulator.Application.Common
{
    public static class Constants
    {
        public static readonly int elementsWidth= 100;
        public static readonly int elementsHeight = 100;
        public static readonly int elementsHalfWidth = elementsWidth / 2;
        public static readonly int elementsHalfHeight = elementsHeight / 2;
        public static readonly Dictionary<Voltage, string> voltagesLightColor = new Dictionary<Voltage, string>
        {
            { Voltage.Black, "#4B4B4B" },
            { Voltage.Yellow, "#FFFF86" },
            { Voltage.Orange, "#FFB15E" },
            { Voltage.Blue, "#3D78FF" },
            { Voltage.Green, "#88FF5C" },
            { Voltage.Red, "#FF6565" },
            { Voltage.Purple, "#FF76FF" },
        };
        public static readonly Dictionary<Voltage, string> voltagesDarkColor = new Dictionary<Voltage, string>
        {
            { Voltage.Black, "#000000" },
            { Voltage.Yellow, "#FFF700" },
            { Voltage.Orange, "#C24100" },
            { Voltage.Blue, "#0000FF" },
            { Voltage.Green, "#00FF00" },
            { Voltage.Red, "#FF0000" },
            { Voltage.Purple, "#FF00FF" },
        };

        public static readonly int DecreasingRemainigTimeAmount = 10;

        public static readonly string NotSet = "ثبت نشده";

        public static readonly string Active = "فعال";
        public static readonly string InActive = "غیرفعال";

        public static readonly string Accept = "قبول";
        public static readonly string UnAccept = "در انتظار تایید";

        public static readonly string CreateMasterSuccessful = "استاد جدید با موفقیت ثبت شد";
        public static readonly string CreateMasterFailed = "استاد جدید با موفقیت ثبت نشد";

        public static readonly string PasswordChangeSuccessful = "کلمه عبور جدید با موفقیت ثبت شد";
        public static readonly string PasswordChangeFailed = "کلمه عبور جدید با موفقیت ثبت نشد";

        public static readonly string ProfileEditSuccessful = "ویرایش پروفایل با موفقیت ثبت شد";
        public static readonly string ProfileEditFailed = "ویرایش پروفایل با موفقیت ثبت نشد";

        public static readonly string CreateStudentSuccessful = "دانشجوی جدید با موفقیت ثبت شد";
        public static readonly string CreateStudentFailed = "دانشجوی جدید با موفقیت ثبت نشد";

        public static readonly string CreateClassSuccessful = "کلاس جدید با موفقیت ثبت شد";
        public static readonly string CreateClassFailed = "کلاس جدید با موفقیت ثبت نشد";

        public static readonly string ClassSendRequestQuestion = "آیا از ارسال درخواست مطمئن هستید؟";
        public static readonly string ClassSendRequestSuccessful = "ثبت نام شما با موفقیت انجام شد";
        public static readonly string ClassSendRequestFailed = "ثبت نام شما با موفقیت انجام نشد";

        public static readonly string ClassRegisterSuccessful = "ثبت نام در کلاس با موفقیت انجام شد";
        public static readonly string ClassRegisterFailed = "ثبت نام در کلاس با موفقیت انجام نشد";
        public static readonly string ClassRegisterExists = "ثبت نام در کلاس مورد نظر قبلا انجام شده است";

        public static readonly string UserClassAcceptRequestQuestion = "آیا از پذیرش درخواست مطمئن هستید؟";
        public static readonly string UserClassAcceptRequestSuccessful = "درخواست با موفقیت پذیرفته شد";
        public static readonly string UserClassAcceptRequestFailed = "درخواست با موفقیت پذیرفته نشد";

        public static readonly string UserClassDeleteRequestQuestion = "آیا از حذف درخواست مطمئن هستید؟";
        public static readonly string UserClassDeleteRequestSuccessful = "درخواست با موفقیت حذف شد";
        public static readonly string UserClassDeleteRequestFailed = "درخواست با موفقیت حذف نشد";

        public static readonly string ClassDeleteQuestion = "آیا از حذف کلاس مطمئن هستید؟";
        public static readonly string ClassDeleteSuccessful = "حذف کلاس با موفقیت انجام شد";
        public static readonly string ClassDeleteFailed = "حذف کلاس با موفقیت انجام نشد";

        public static readonly string MasterDeleteQuestion = "آیا از حذف استاد مطمئن هستید؟";
        public static readonly string MasterDeleteSuccessful = "حذف استاد با موفقیت انجام شد";
        public static readonly string MasterDeleteFailed = "حذف استاد با موفقیت انجام نشد";

        public static readonly string StudentDeleteQuestion = "آیا از حذف دانشجو مطمئن هستید؟";
        public static readonly string StudentDeleteSuccessful = "حذف دانشجو با موفقیت انجام شد";
        public static readonly string StudentDeleteFailed = "حذف دانشجو با موفقیت انجام نشد";

        public static readonly string ClassUnregisterQuestion = "آیا از حذف دانشجو از کلاس مطمئن هستید؟";
        public static readonly string ClassUnregisterSuccessful = "حذف دانشجو از کلاس با موفقیت انجام شد";
        public static readonly string ClassUnregisterFailed = "حذف دانشجو از کلاس با موفقیت انجام نشد";

        public static readonly string UserClassLoadError = "خطا در بارگیری کاربر کلاس";
        public static readonly string PostLoadError = "خطا در بارگیری پست";
        public static readonly string PostCreateError = "خطا در افزودن پست";
        public static readonly string PostDeserializeError = "خطا در تبدیل پست";

        public static readonly string DeletePostQuestion = "آیا از حذف پست مطمئن هستید؟";
        public static readonly string DeletePostSuccessful = "حذف پست با موفقیت انجام شد";
        public static readonly string DeletePostFailed = "حذف پست با موفقیت انجام نشد";
    }
}
