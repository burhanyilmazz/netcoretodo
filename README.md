# netcoretodo

**Kullanılan Teknolojiler**
  - .Net Core Api
  - .Net Core Mvc
  - Dapper
  - MSSQL
  - Jquery
  - Flurl
  - JSON Web Token
  - Swagger
  - Api Versioning

Uygulama katmanlı mimariye uygun tasarlanmış olup Rest tabanlı çalışmaktadır. ORM olarak **mini-ORM Dapper** kullanılmıştır. Uygulama http isteklerini api projesine göndermektedir.Authentication ve authorization işlemleri için **JSON Web Token** kullanılmıştır.

**Kullanılan Yazılım Desenleri**
  - Singleton
  - Repository Pattern
  - Dependeny Injection
 
**Katmanlar**
  - **Todo.Core** : Sistem çekirdeğinde bulunan; class, enum,viewmodel, extensionlar ve helperlar bu katmandadır.
  - **Todo.Domain** : Uygulama nesnelerin tutulduğu katmandır. Bu katmanda interface’ler abstract classlar ve classlar tutulmaktadır. Class’lar içerisinde validasyon ve Tablo-Nesne modellemesi için gerekli dataannotionlar bulunmaktadır. 
  - **Todo.DataAccess** : Projenin veritabanı MSSQL’dir. Veriye erişim için **mini-ORM Dapper** toolu kullanılmıştır. Singleton connection factory pattern’i ile uygulama yaşam süresi boyunca bir seferlik oluşan connection bilgisini, Generic Repository pattern'e implemente ederek ile veri erişimi sağlamaktadır. 
  -  **Todo.Api** : Bu katmanda projenin REST servisidir. Core,Domain ve DataAccess katmanı, bu Api projesini beslemektedir.
        - Environment bazlı bir konfigürayon kurgulanmıştır. Dev > Test > Prod.  İlgili environment config dosyasındaki connectionstring bilgisini alarak data servisleri implemente etmektedir.
        - Dependency injection tool’u olarak Microsoft’un DependencyInjection extension’u kullanılmıştır.
        - Api versiyonlama route’a göre yapılmıştır. Startup dosyasında swagger entegrasyonuyla birlikte tüm controller versiyonlanacak şekilde configure edilmiştir. Örn: api/v1/user/Login.. api/v2/user/Login
        - Authentication ve Authorization işlemleri için JWT kullanılmıştır. Jwt konfigürasyonu, ilgili environment’in config bilgilerine göre oluşmaktadır. JWT tokenı, kullanıcının bilgileriyle birlikte, kullanıcının yetkilerini de içerebilir. Claims-policy based bir yapı kullanılmıştır. Metotların yetkilendirilmesi de override edilerek custom bir şekilde yapılmıştır. Sistemde bulunabilecek right'ları da tanımlayabiliriz. Method actionfilterlarına ilgili yetkiyi enum’la tanımlarsam, yapılan her istekte token içerisindeki yetkiyi kontrol ederek işleme devam edebilir hale gelir. Tüm bu yetkilendirme için dinamik policy yapısı hazırdır.
        - Tüm model validasyonları için OnActionExecuting action filter’ı override edilerek custom bir validasyon yönetilmiştir.
        - Response tutarlılığını sağlamak için yazdığım bir middleware ile request’leri handle edip tek bir serviceResult dönüyorum. Böylelikle her türlü olumlu-olumsuz mesajlar ile birlikte istediğim objeyi döndürebiliyorum.
        -  UI katmanında .NET Core MVC kullanılmıştır. Web katmanının tek işi, gerek front-end tarafında gerekse back-end tarafında da .NET Core Api servisinden beslenmektir. 


İsmail KANAT
Software Developer
www.ismailkanat.com
0538 344 35 47

