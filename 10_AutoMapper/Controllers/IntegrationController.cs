using _10_AutoMapper.Dtos;
using _10_AutoMapper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace _10_AutoMapper.Controllers
{
    /// <summary>
    /// Dış sistem / 3.Parti API Entegrasyonu Controller'ı:
    /// ======================================================
    /// Bir e-ticaret, B2B tedarikci, kargo veya harici CRM servisinden HTTP/REST ile 
    /// JSON formatında veri çekeriz(veya Webhook ile bize push edilir)
    /// 
    /// Dış API'den gelen veriler:
    ///  - snake_case isimlendirmelere sahiptir("external_id" vs gibi)
    ///  - İç içe (nested) nesneler barındırır(örn: "address")
    ///   - Tipleri ve formatları iç sistemimizle tam uyuşmaz(kuruş cinsinden tutar, ISO tarih, string durum)
    ///   
    /// İşte Burada AutoMapper Kullanılarak;
    /// Dış sistemin ham yanıtı(ExternalCustomerDto), tek bir satırda kendi tertemiz, 
    /// güvenli ve standartlara uygun iç etki alanı modelimize(Customer) dönüştürülür.
    /// </summary>
    public class IntegrationController : Controller
    {
        private readonly IMapper _mapper;

        public IntegrationController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            // 1. ADIM: Dış servisten(3.Parti REST API veya Webhook) gelen ham JSON yanıtını simüle ediyoruz.

            string rawJsonFromExternalApi = """
                 {
                    "external_id":"GLOBAL-CRM-94832",
                    "first_name":"Berkay",
                    "last_name": "Kara",
                    "contact_email":"berkay.kara@gmail.com",
                    "phone_number":"+095015016161",
                    "status_code":"ACTIVE",
                    "balance_cents":856543,
                    "created_timestamp": "2026-09-21T09:45:00Z",
                    "address":{
                    "street":":Büyükderece Cad. No: 61 K:8",
                    "district":"Şişli",
                    "city":"İstanbul",
                    "zip_code":"34454",
                    "country_code":"TR"
                    }
                 }
                """;

            var externalCustomerDto = JsonSerializer.Deserialize<ExternalCustomerDto>(rawJsonFromExternalApi, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Dış sistemin kirli DTO'sunu karşılıyor ve iç sistemimizin temiz kurumsal Customer domain nesnesine dönüştürüyoruz.
            var internalCustomer = _mapper.Map<Customer>(externalCustomerDto);

            internalCustomer.Id = 1043;

            ViewBag.RawJson = rawJsonFromExternalApi;
            ViewBag.ExternalDto = externalCustomerDto;

            return View(internalCustomer);
          
        }
    }
}
