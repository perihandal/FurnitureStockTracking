# Mobilya Stok Takip Sistemi

## Proje Açıklaması

Bu proje, .NET 8 ve Entity Framework Core teknolojileri kullanılarak geliştirilmiş kapsamlı bir mobilya stok takip sistemidir. Sistem, bir şirketin tüm mobilya stok yönetimi ihtiyaçlarını karşılayabilecek güvenli, ölçeklenebilir ve yönetilebilir bir altyapı sunmaktadır.

Proje, modern yazılım mimarisi prensipleri doğrultusunda Clean Architecture yaklaşımı benimsenmiş ve üç ana katmandan oluşmaktadır: **API Katmanı**, **Servis Katmanı** ve **Repository Katmanı**. Bu yapı, şirketin operasyonel ihtiyaçlarını karşılayabilecek esnek ve sürdürülebilir bir sistem altyapısı oluşturulmasını amaçlamaktadır.

## Sistem Mimarisi

### Katmanlı Yapı

**API Katmanı (App.API):**
- RESTful API endpoint'leri
- JWT tabanlı kimlik doğrulama ve yetkilendirme sistemi
- Swagger/OpenAPI dokümantasyonu
- FluentValidation ile veri doğrulama
- CORS desteği

**Servis Katmanı (App.Services):**
- İş mantığı (business logic) katmanı
- Veri transfer objeleri (DTOs)
- FluentValidation doğrulama servisleri
- Barkod üretim servisi
- Otomatik fiyat geçmişi yönetimi

**Repository Katmanı (App.Repositories):**
- Entity Framework Core tabanlı veri erişim katmanı
- Generic Repository pattern implementasyonu
- Unit of Work pattern
- Database migration yönetimi
- İlişkisel veri modeli

## Modüller ve Özellikler

Sistem, şirketin farklı departmanlarının ihtiyaçlarına yönelik olarak aşağıdaki ana modüllerden oluşmaktadır:

### 1. Stok Yönetimi Modülü
- **Stok Kartları (StockCards):** Nihai ürün, ara ürün ve hammadde kategorilerinde stok tanımlamaları
- **Barkod Yönetimi (BarcodeCards):** Otomatik barkod üretimi ve çoklu barkod desteği
- **Stok Hareketleri (StockTransactions):** Giriş, çıkış ve transfer işlemleri
- **Depo Yönetimi (Warehouses):** Çoklu depo desteği ve depo bazlı stok takibi

### 2. Fiyat Yönetimi Modülü
- **Fiyat Tanımlamaları (PriceDefinitions):** Ürün bazlı fiyat belirleme
- **Fiyat Geçmişi (PriceHistories):** Otomatik fiyat değişikliği kayıtları
- **Vergi Oranları:** Ürün bazlı vergi tanımlamaları

### 3. Kategorizasyon Modülü
- **Ana Gruplar (MainGroups):** Birincil ürün kategorileri
- **Alt Gruplar (SubGroups):** İkincil kategorizasyon
- **Kategoriler (Categories):** Detaylı ürün sınıflandırması

### 4. Organizasyon Modülü
- **Şirketler (Companies):** Çoklu şirket desteği
- **Şubeler (Branches):** Şube bazlı işlem takibi
- **Kullanıcı Yönetimi:** Rol tabanlı erişim kontrolü

### 5. Güvenlik ve Yetkilendirme
- **JWT Token Sistemi:** Güvenli API erişimi
- **Rol Tabanlı Yetkilendirme:** Admin, Editor ve kullanıcı rolleri
- **TACACS Benzeri Güvenlik:** Merkezi kimlik doğrulama desteği

## Teknik Altyapı

### Kullanılan Teknolojiler
- **.NET 8:** Modern framework desteği
- **Entity Framework Core:** ORM ve database yönetimi
- **SQL Server:** Veritabanı yönetim sistemi
- **JWT (JSON Web Tokens):** Güvenlik ve kimlik doğrulama
- **FluentValidation:** Veri doğrulama
- **Swagger/OpenAPI:** API dokümantasyonu
- **AutoMapper:** Object mapping (gerektiğinde)

### Veritabanı Yapısı
Sistem, ilişkisel veritabanı modeli kullanarak aşağıdaki ana tablolardan oluşmaktadır:

- **StockCards:** Ana stok kartı bilgileri
- **BarcodeCards:** Barkod tanımlamaları
- **StockTransactions:** Stok hareket kayıtları
- **Warehouses & WarehouseStocks:** Depo ve stok durumu
- **Companies & Branches:** Organizasyon yapısı
- **Users & Roles:** Kullanıcı yönetimi
- **Categories, MainGroups, SubGroups:** Kategorizasyon
- **PriceDefinitions & PriceHistories:** Fiyat yönetimi

## API Endpoints

Sistem, RESTful API prensipleri doğrultusunda aşağıdaki ana endpoint gruplarını sunmaktadır:

### Kimlik Doğrulama
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Yeni kullanıcı kaydı
- `POST /api/auth/refresh` - Token yenileme

### Stok Yönetimi
- `GET /api/stockcard` - Tüm stok kartları
- `POST /api/stockcard` - Yeni stok kartı oluşturma
- `PUT /api/stockcard/{id}` - Stok kartı güncelleme
- `GET /api/stockcard/{pageNumber}/{pageSize}` - Sayfalama desteği

### Barkod Yönetimi
- `GET /api/barcodecard` - Barkod listesi
- `POST /api/barcodecard` - Yeni barkod oluşturma
- `POST /api/barcodecard/generate` - Otomatik barkod üretimi

### Depo İşlemleri
- `GET /api/warehouse` - Depo listesi
- `GET /api/warehousestock` - Depo stok durumu
- `POST /api/stocktransaction` - Stok hareketi kayıt

## Güvenlik Özellikleri

### JWT Tabanlı Kimlik Doğrulama
- Güvenli token tabanlı oturum yönetimi
- Configurable token süresi
- Refresh token desteği
- Role-based authorization

### Rol Tabanlı Erişim Kontrolü
- **Admin:** Tüm işlemler için tam yetki
- **Editor:** Veri ekleme ve düzenleme yetkisi
- **User:** Sadece okuma yetkisi

### Veri Doğrulama
- FluentValidation ile comprehensive veri doğrulama
- Business rule validations
- Duplicate control mechanisms

## Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8 SDK
- SQL Server (LocalDB desteklenir)
- Visual Studio 2022 veya VS Code

### Kurulum Adımları

1. **Repository'yi klonlayın:**
```bash
git clone [repository-url]
cd FurnitureStockTracking
```

2. **Bağımlılıkları yükleyin:**
```bash
dotnet restore
```

3. **Veritabanı bağlantı ayarları:**
`appsettings.json` dosyasında connection string'i güncelleyin.

4. **Veritabanı migration:**
```bash
dotnet ef database update --project App.Repositories --startup-project App.API
```

5. **Uygulamayı çalıştırın:**
```bash
dotnet run --project App.API
```

### API Dokümantasyonu
Uygulama çalıştırıldıktan sonra Swagger UI'a aşağıdaki adresten erişilebilir:
```
https://localhost:7000/swagger
```

## Proje Yapısı

```
FurnitureStockTracking/
├── App.API/                    # Web API katmanı
│   ├── Controllers/           # API Controllers
│   ├── Auth/                  # JWT kimlik doğrulama
│   └── Startup/              # Başlangıç servisleri
├── App.Services/              # İş mantığı katmanı
│   ├── StockCardServices/    # Stok kartı servisleri
│   ├── BarcodeCardServices/  # Barkod servisleri
│   └── AuthServices/         # Kimlik doğrulama servisleri
└── App.Repositories/          # Veri erişim katmanı
    ├── StockCards/           # Stok kartı repository
    ├── BarcodeCards/         # Barkod repository
    └── Migrations/           # EF Core migrations
```

## Özellikler ve Avantajlar

### İş Süreçleri
- **Otomatik Barkod Üretimi:** Sistem, stok kartı oluştururken otomatik olarak benzersiz barkod üretebilir
- **Çoklu Depo Desteği:** Farklı lokasyonlarda stok takibi yapılabilir
- **Fiyat Geçmişi:** Tüm fiyat değişiklikleri otomatik olarak kayıt altına alınır
- **Stok Hareket Takibi:** Detaylı giriş, çıkış ve transfer kayıtları

### Teknik Avantajlar
- **Scalable Architecture:** Büyüyen iş ihtiyaçlarına uyum sağlayabilir
- **API-First Approach:** Frontend teknoloji bağımsızlığı
- **Comprehensive Logging:** Detaylı sistem log kayıtları
- **Error Handling:** Merkezi hata yönetimi sistemi

### Güvenlik
- **JWT Authentication:** Modern ve güvenli kimlik doğrulama
- **Role-based Authorization:** Granular yetki kontrolü
- **Input Validation:** Comprehensive veri doğrulama
- **SQL Injection Protection:** Entity Framework güvenlik katmanı

## Gelecek Geliştirmeler

- **Raporlama Modülü:** Detaylı stok ve satış raporları
- **Dashboard:** Yönetici paneli ve grafiksel gösterimler
- **Notification System:** E-posta ve SMS bildirimleri
- **Mobile App:** iOS ve Android uygulamaları
- **Integration APIs:** ERP ve muhasebe sistemleri entegrasyonu
- **Advanced Analytics:** Stok analizi ve tahminleme

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

---

Bu sistem, modern yazılım geliştirme prensipleri ve güvenlik standartları gözetilerek tasarlanmış olup, mobilya sektöründeki şirketlerin stok yönetimi ihtiyaçlarını karşılayacak kapsamlı bir çözüm sunmaktadır. Sistem hem iç operasyonları optimize etmekte hem de dış entegrasyonlar için hazır bir altyapı sağlamaktadır.