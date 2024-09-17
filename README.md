
# Proje Tanımı

Bu proje, .NET Core 8 kullanarak temel bir Web API geliştirme, JWT ile kimlik doğrulama, containerizasyon işlemleri, Serilog, AutoMapper, Fluent Validation gibi paketler kullanarak hazırlanmıştır. Aşağıda projenin genel yapısı ve önemli bileşenleri açıklanmıştır.


## API Geliştirme

* Proje: .NET Core 8 sürümü kullanarak basit bir RESTful Web API geliştirilmiştir.
* CRUD İşlemleri: Ürünler gibi basit bir kaynağı yönetmek için CRUD (Create, Read, Update, Delete) işlemleri uygulanmıştır.
* Bağımlılık Enjeksiyonu ve Middleware:
    * Hata Yönetimi: Hata yönetimi için özel bir ExceptionMiddleware kullanılmıştır.
    * Serilog: Yapılandırılmış loglama yapmak ve farklı hedeflere (konsol, dosya, vb.) entegre etmek için kullanılmıştır.
    * AutoMapper: DTO'lar ve domain modelleri arasında otomatik eşleme yapmak için kullanılmıştır.
    * FluentValidation: Girdi doğrulaması sağlamak için kullanılmıştır.
    * Her önemli sınıf, metod ve özellik için açıklamalar eklenmiştir.

## Veritabanı Entegrasyonu
* Entity Framework Core: SQL Server veritabanına bağlanmak için kullanılmıştır.
* Code-First Yaklaşımı: Veritabanı modellemeyi Code-First yaklaşımı ile gerçekleştirilmiştir.

## API Geliştirme

* Proje: .NET Core 8 sürümü kullanarak basit bir RESTful Web API geliştirilmiştir.
* CRUD İşlemleri: Ürünler gibi basit bir kaynağı yönetmek için CRUD (Create, Read, Update, Delete) işlemleri uygulanmıştır.
* Bağımlılık Enjeksiyonu ve Middleware:
    * Hata Yönetimi: Hata yönetimi için özel bir ExceptionMiddleware kullanılmıştır.
    * Serilog: Yapılandırılmış loglama yapmak ve farklı hedeflere (konsol, dosya, vb.) entegre etmek için kullanılmıştır.
    * AutoMapper: DTO'lar ve domain modelleri arasında otomatik eşleme yapmak için kullanılmıştır.
    * FluentValidation: Girdi doğrulaması sağlamak için kullanılmıştır.
    * Her önemli sınıf, metod ve özellik için açıklamalar eklenmiştir.

## JWT ile Kimlik Doğrulama ve Rol Yönetimi
* JWT Web Token: Kimlik doğrulama mekanizması olarak kullanılmıştır.
* Rol Yönetimi:
  * Admin Rolü: Kaynakları listeleme (Get), ekleme (Create), güncelleme (Update) ve silme (Delete) yetkisi verilmiştir.
  * User Rolü: Kaynakları listeleme (Get) ve ekleme (Create) yetkisi verilmiştir.
  * Yetkilendirme Kontrolleri: Güncelleme ve silme işlemleri için geçerli bir JWT token ve "Admin" rolü gereklidir.
 
## Containerizasyon
* Docker: Uygulama Docker kullanarak containerize edilmiştir.
* Dockerfile ve Docker Compose: Web API ve SQL Server veritabanını container'larda çalıştırmak için Dockerfile ve Docker Compose dosyaları hazırlanmıştır.
* Karşılaşılan Zorluklar: docker-compose.yml dosyasına sql Server veritabanını nasıl ekleneceği Google search ve chatgpt sonuncu bulunup eklenmiştir.

### API Uç Noktaları
- **`POST /Account/Login`**: Kullanıcının e-posta adresi ve parolası ile giriş yapmasını sağlar. Başarılı bir giriş sonrası kullanıcıya JWT token döner. E-posta doğrulaması yapılmamış kullanıcılar veya geçersiz giriş bilgileri durumunda uygun hata mesajları döner.

  * Örnek İstekler

    * Admin ile giriş yapmak için :
    curl --location 'https://localhost:1920/Account/Login' \
    --header 'Content-Type: application/json' \
    --data-raw '{
    "email": "admin@gmail.com",
    "password": "admin123*"
    }'


    * User ile giriş yapmak için :
    curl --location 'https://localhost:1920/Account/Login' \
    --header 'Content-Type: application/json' \
    --data-raw '{
    "email": "user@gmail.com",
    "password": "user123*"
    }'

- **`GET /Product/GetById`**: Belirtilen Id parametresine sahip ürünü getirir. İlgili ürün bulunamazsa hata mesajı döner. Ürün bulunursa, ürün bilgileri ProductViewModel formatında döner.

  * Örnek İstekler

    curl --location --request GET 'https://localhost:1920/Product/GetById' \
    --header 'Content-Type: application/json' \
    --header 'Authorization: Bearer {{TOKEN}}' \
    --data '{
    "id": "6aa7a406-79c4-4159-b1ea-b27329b3d8ca"
    }'

- **`GET /Product/GetList`**: Veritabanındaki tüm ürünleri getirir. Ürün listesi boş ise uygun bir hata mesajı döner. Ürünler başarıyla getirildiğinde, ürünler ProductViewModel formatında bir liste olarak döner.

    * Örnek İstekler

    curl --location 'https://localhost:1920/Product/GetList' \
    --header 'Authorization: Bearer {{TOKEN}}'

- **`POST /Product/Create`**: Yeni bir ürün ekler. Ürün bilgileri ProductViewModel formatında sağlanır. Girdi doğrulama hataları varsa, uygun hata mesajları döner. Başarıyla eklenen ürün sonrası NoContent döner.

    * Örnek İstekler

    curl --location 'https://localhost:1920/Product/Create' \
    --header 'Content-Type: application/json' \
    --header 'Authorization: Bearer {{TOKEN}}' \
    --data '{
    "Name": "Elma",
    "Price": 10,
    "Description": "Elma"
    }'

- **`POST /Product/Update`**: Var olan bir ürünü günceller. Güncelleme işlemi sadece "Admin" rolüne sahip kullanıcılar tarafından yapılabilir. Ürün bilgileri ProductViewModel formatında sağlanır. Ürün doğrulama hataları varsa, uygun hata mesajları döner. Başarıyla güncellenen ürün sonrası NoContent döner.

    * Örnek İstekler

    curl --location 'https://localhost:1920/Product/Update' \
    --header 'Content-Type: application/json' \
    --header 'Authorization: Bearer {{TOKEN}}' \
    --data '{
    "Id": "e2a8ba61-a59b-4987-ac00-e49fc8aad546",
    "Name": "Elma",
    "Price": 12,
    "Description": "Elma"
    }'

- **`DELETE /Product/Delete`**: Var olan bir ürünü siler. Silme işlemi sadece "Admin" rolüne sahip kullanıcılar tarafından yapılabilir. Silinecek ürün Id parametresi ile belirtilir. Ürün bulunamazsa uygun hata mesajı döner. Başarıyla silinen ürün sonrası NoContent döner.

    * Örnek İstekler

    curl --location --request DELETE 'https://localhost:1920/Product/Delete' \
    --header 'Content-Type: application/json' \
    --header 'Authorization: Bearer {{TOKEN}}' \
    --data '{
    "Id": "e2a8ba61-a59b-4987-ac00-e49fc8aad546"
    }'

  ## Link

[Postman](https://api.postman.com/collections/25138324-605b99d5-9c2c-4450-a94d-b2e77035663e?access_key=PMAT-01J7XPZ9RXTBCYKRHC83NXT1XD)
