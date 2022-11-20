namespace Server.WebFramework.Configurations;


public class PersianIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"ارور ناشناخته ای رخ داده است" }; }
    public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "در درخواست شما تداخلی ایجاد شده است" }; }
    public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "رمز عبور اشتباه است" }; }
    public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "توکن ارسالی اشتباه است" }; }
    public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "یوزری با این مشخصات در حال حاضر لاگین کرده است" }; }
    public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"یوزر نیم '{userName}' صحیح نمی باشد فقط می توانید از حروف و اعداد استفاده کنید" }; }
    public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"ایمیل '{email}' صحیح نمی باشد" }; }
    public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"یوزرنیم '{userName}' قبلا توسط اکانت دیگری استفاده شده است" }; }
    public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"ایمیل '{email}' قبل استفاده شده است" }; }
    public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"نقش '{role}' موجود نمی باشد" }; }
    public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"نقش '{role}' قبلا برای این کاربر استفاده شده است" }; }
    public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "کاربر قبلا رمز عبوری را استفاده کرده است" }; }
    public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "کاربر مورد نظر قفل شده است" }; }
    public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"نشق مورد نظر برای این کاربر استفاده شده است" }; }
    public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"کاربر مورد نظر در نقش '{role}' نیست" }; }
    public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"پسورد حداقل باید {length} کاراکتر باشد" }; }
    public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "رمز عبور باید حداقل یک کاراکتر غیر عددی داشته باشد" }; }
    public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "پسور مورد نظر باید حداقل یک عدد داشته باشد ('0'-'9')" }; }
    public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "پسورد مورد نظر باید حداقل یکی از حروف ('a'-'z') به صورت کوچک داشته باشد" }; }
    public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "پسورد مورد نظر باید حداقل یکی از حروف ('A'-'Z') به صورت بزرگ داشته باشد" }; }
}