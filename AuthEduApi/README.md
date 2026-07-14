# 🔐 Auth Edu API — پروژه آموزشی Identity + JWT

## مفاهیم پوشش داده شده

| مفهوم | توضیح |
|-------|-------|
| **Identity** | سیستم مدیریت کاربر، هش پسورد، Role |
| **JWT** | Token stateless — بدون Session |
| **Claim** | اطلاعات جاسازی‌شده داخل توکن |
| **Role** | نقش کاربر در سیستم (Admin/User) |
| **Policy** | مجموعه شرط‌های دسترسی |
| **Authentication** | "تو کی هستی؟" |
| **Authorization** | "اجازه داری؟" |

---

## راه‌اندازی

```bash
dotnet restore
dotnet run
```

Swagger: `https://localhost:7001/swagger`

> دیتابیس SQLite به صورت خودکار ساخته میشه.
> ادمین پیش‌فرض: `admin@edu.com` / `admin123`

---

## معماری سیستم دسترسی

```
┌─────────────────────────────────────────────────┐
│              JWT Token Payload                   │
│                                                  │
│  sub:         "user-guid"                        │
│  email:       "ali@test.com"                     │
│  role:        "User"                             │
│  is_approved: "false"  ← Custom Claim            │
│  is_pro:      "false"  ← Custom Claim            │
│  exp:         1234567890                         │
└─────────────────────────────────────────────────┘
         ↓ سرور این Claims رو بدون دیتابیس می‌خونه
┌─────────────────────────────────────────────────┐
│               Authorization Policies             │
│                                                  │
│  AdminOnly    → RequireRole("Admin")             │
│  ApprovedUser → is_approved == "true"           │
│  ProUser      → is_approved + is_pro == "true"  │
└─────────────────────────────────────────────────┘
```

---

## سطوح دسترسی

```
GET /api/products/public      🔓  همه
GET /api/products/auth-only   🔑  فقط لاگین
GET /api/products             ✅  تایید ادمین
GET /api/products/pro         💎  تایید + Pro
POST /api/products            👑  فقط Admin
```

---

## گردش کار آموزشی (Live Coding Flow)

```
1. Register → IsApproved = false
   POST /api/auth/register

2. Login → توکن با is_approved:false
   POST /api/auth/login

3. بررسی Claims توکن
   GET /api/auth/me
   → is_approved: "false"

4. تلاش دسترسی → 403 Forbidden ❌
   GET /api/products

5. ادمین تایید می‌کنه
   POST /api/admin/approve/{userId}

      ⚠️  نکته JWT: توکن قدیمی هنوز is_approved:false داره!
          کاربر باید مجدداً لاگین کنه.
          این ذات Stateless بودن JWT هست.

6. Login مجدد → توکن با is_approved:true
   POST /api/auth/login

7. دسترسی موفق → 200 OK ✅
   GET /api/products

8. تلاش Pro Feature → 403 Forbidden ❌
   GET /api/products/pro

9. ادمین Pro می‌کنه
   POST /api/admin/upgrade-pro/{userId}

10. Login مجدد → توکن با is_pro:true
    POST /api/auth/login

11. دسترسی Pro Feature → 200 OK ✅
    GET /api/products/pro
```

---

## نکات کلیدی برای آموزش

### 1. JWT Stateless Problem
وقتی ادمین وضعیت کاربر رو تغییر میده، **توکن فعلی بلافاصله تغییر نمیکنه!**
کاربر باید re-login کنه. راه‌حل‌های پیشرفته:
- کوتاه کردن عمر توکن (15 دقیقه + Refresh Token)
- Token Blacklist در Redis
- Security Stamp در Identity

### 2. Authentication vs Authorization
```
Authentication = UseAuthentication()  → "تو کی هستی؟"
Authorization  = UseAuthorization()   → "اجازه داری؟"

⚠️  ترتیب در Program.cs اجباریه!
    اول UseAuthentication() بعد UseAuthorization()
```

### 3. Role vs Policy
```
Role   → ساده "نقش X داری؟"
Policy → قدرتمند "شرط A AND شرط B AND شرط C ؟"

Policy می‌تونه Role + Claims + Custom Logic ترکیب کنه
```

### 4. Claims داخل توکن
```
Claims در توکن رمزنگاری میشن (نه رمز میشن!)
هر کسی می‌تونه محتوای توکن رو decode کنه
اما نمی‌تونه آن رو دستکاری کنه (امضای دیجیتال)

→ هیچوقت اطلاعات حساس (پسورد، کارت بانکی) داخل Claim نذار!
```

---

## ساختار پروژه

```
AuthEduApi/
├── Constants/
│   └── AppConstants.cs     ← Roles + Claims + Policies
├── Models/
│   └── AppUser.cs          ← IdentityUser + IsApproved + IsPro
├── Data/
│   ├── AppDbContext.cs      ← IdentityDbContext
│   └── SeedData.cs         ← Roles + Admin پیش‌فرض
├── DTOs/
│   └── AuthDtos.cs         ← RegisterDto, LoginDto, AuthResponseDto
├── Services/
│   ├── JwtService.cs       ← تولید JWT Token
│   └── AuthService.cs      ← Register + Login Logic
├── Controllers/
│   ├── AuthController.cs   ← register, login, me
│   ├── AdminController.cs  ← approve, revoke, upgrade-pro
│   └── ProductController.cs ← نمایش ۴ سطح دسترسی
├── Program.cs              ← تمام Configuration
├── appsettings.json        ← JWT Config
└── requests.http           ← تست‌های آماده
```
