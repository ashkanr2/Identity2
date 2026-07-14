using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthEduApi.Models;

/// <summary>
/// مدل کاربر ما
///
/// 💡 چرا از IdentityUser ارث‌بری می‌کنیم؟
///   IdentityUser فیلدهای پایه رو داره: Id, UserName, Email,
///   PasswordHash, PhoneNumber, LockoutEnabled و...
///   ما فقط فیلدهای اختصاصی کسب‌وکارمون رو اضافه می‌کنیم
/// </summary>
public class AppUser : IdentityUser
{
    public string? FullName { get; set; }

    /// <summary>
    /// آیا ادمین این کاربر رو تایید کرده؟
    ///
    /// 🎯 این فیلد قلب سیستم تایید ماست:
    ///   - پیش‌فرض: false  (کاربر نمی‌تونه کاری کنه)
    ///   - ادمین true می‌کنه  → کاربر می‌تونه وارد سیستم بشه
    ///   - این مقدار داخل JWT به عنوان Claim جاسازی میشه
    /// </summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>
    /// آیا کاربر اکانت پرمیوم داره؟
    ///
    /// 🎯 این فیلد سطح اشتراک کاربر رو نشون میده:
    ///   - false = کاربر عادی
    ///   - true  = کاربر پرو (دسترسی به فیچرهای پیشرفته)
    /// </summary>
    public bool IsPro { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
