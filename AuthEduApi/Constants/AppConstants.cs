namespace AuthEduApi.Constants;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ROLES
// نقش‌های سیستم - string constant تا از typo جلوگیری کنیم
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
public static class AppRoles
{
    public const string Admin = "Admin";
    public const string User  = "User";
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// CLAIMS
// کلیم‌های سفارشی که ما تعریف می‌کنیم و داخل JWT جاسازی میشن
//
// 💡 Claim چیه؟
//   یه جفت Key/Value که داخل توکن رمزنگاری میشه
//   سرور این اطلاعات رو بدون نیاز به دیتابیس می‌خونه
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
public static class AppClaims
{
    public const string IsApproved = "is_approved";   // آیا ادمین تاییدش کرده؟
    public const string IsPro      = "is_pro";        // آیا اکانت پرو داره؟
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// POLICIES
// Policy = مجموعه‌ای از شرط‌ها که برای دسترسی باید برآورده بشن
//
// 💡 Policy vs Role:
//   Role: "کاربر Admin هست؟"
//   Policy: "کاربر لاگینه AND is_approved:true AND is_pro:true ؟"
//   Policy قدرتمندتره چون می‌تونی Claims مختلف رو ترکیب کنی
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
public static class AppPolicies
{
    public const string AdminOnly    = "AdminOnly";    // فقط نقش Admin
    public const string ApprovedUser = "ApprovedUser"; // تایید شده توسط ادمین
    public const string ProUser      = "ProUser";      // تایید شده + اکانت پرو
}
