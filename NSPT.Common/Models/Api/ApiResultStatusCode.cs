using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Common.Models.Api;

public enum ApiResultStatusCode
{
    [Display(Name = "عملیات با موفقیت انجام شد")]
    Success = HttpStatusCode.OK,

    [Display(Name = "خطایی در سرور رخ داده است")]
    ServerError = HttpStatusCode.InternalServerError,

    [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
    BadRequest = HttpStatusCode.BadRequest,

    [Display(Name = "یافت نشد")]
    NotFound = HttpStatusCode.NotFound,

    [Display(Name = "لیست خالی است")]
    ListEmpty = 4,

    [Display(Name = "خطایی در پردازش رخ داد")]
    LogicError = 5,

    [Display(Name = "موجودی کیف پول کافی نمی باشد")]
    WalletBalanceNoEnough = 6,

    [Display(Name = "خطای احراز هویت")]
    UnAuthorized = HttpStatusCode.Unauthorized,

    [Display(Name = "سرور خاموش شده است لطفا منتظر بمانید")]
    ServerDown = HttpStatusCode.ServiceUnavailable,

    [Display(Name = "در ارسال پیامک مورد نظر مشکلی رخ داده است")]
    SendSmsError = 7,


    [Display(Name = "در ارسال درخواست به سرورهای دیگر مشکلی رخ داده است")]
    RefitError = 8
}