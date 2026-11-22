using _10_AutoMapper.Dtos;
using _10_AutoMapper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _10_AutoMapper.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper; // AutoMapper servisini enjekte ediyoruz. Yoksa mapping çalışmaz

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var sourceUser = new User
            {
                Id = 101,
                FirstName = "Onur",
                LastName = "Yağız",
                Email = "onur@gmail.com",
                PasswordHash = "AQAAAAAACvsaFDSFDFf.....(Çok Gizli Hash)",
                CreatedDate = DateTime.Now.AddMonths(-6)
            };

            // AutoMapper ile User -> UserDto dönüşümü tek satırda yapılır.
            var userDto = _mapper.Map<UserDto>(sourceUser);

            // Karşılaştırma yapabilmek için kaynak Entity'yi de ViewBag ile View'e taşıyoruz. Normalde bu satır olmaz. 
            ViewBag.SourceUser = sourceUser;

            return View(userDto);
        }

        public IActionResult MappingTest()
        {
            var sourceDto = new UserDto
            {
                Id = 202,
                FullName = "Berkay Kara",
                Email = "berkay@gmail.com"
            };

            var userEntity = _mapper.Map<User>(sourceDto);

            ViewBag.SourceDto = sourceDto;

            return View(userEntity);
        }
    }
}
