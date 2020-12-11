using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckBoxPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 這個範例只適合做在 ajax 的情況
            // 用於 form post 的範例，目前還沒想到

            var vm = new TestViewModel
                     {
                         TestItemDtos = GetTest2ItemDtos()
                     };

            // 把其中二個項目指定為無 Id ，代表原本的資料無勾選
            vm.TestItemDtos[1].Id = null;
            vm.TestItemDtos[3].Id = null;

            return View(vm);
        }

        [HttpPost]
        public IActionResult PostIndex(TestViewModel vm)
        {
            // 如果 View 的 Test2ItemDto.Test2ItemDtos 在 Post 的 FormData
            // - 只有一個項目，必須要用 T[] 型態來 binding
            // - 有二個以上項目，就可以使用原本的型態來 binding，但未勾選的項目，就不會有 html value 欄位 (Test2ItemDto.ForeignKeyId)


            // 可以取出 ForeignKeyId 不為 null 的項目，就是勾選的資料
            // 勾選項目中
            // - Id 有值，代表是上次已勾選，此次仍勾選
            // - Id 沒有值，代表是此次才勾選的
            var checkItems = vm.TestItemDtos
                               .Where(dto => dto.ForeignKeyId != null)
                               .ToArray();

            return RedirectToAction("Index");
        }

        private static TestItemDto[] GetTest2ItemDtos()
        {
            return new[]
                   {
                       new TestItemDto { Id = 1, Name = "A", ForeignKeyId = 11 },
                       new TestItemDto { Id = 2, Name = "B", ForeignKeyId = 12 },
                       new TestItemDto { Id = 3, Name = "C", ForeignKeyId = 13 },
                       new TestItemDto { Id = 4, Name = "D", ForeignKeyId = 14 },
                       new TestItemDto { Id = 5, Name = "E", ForeignKeyId = 15 },
                   };
        }
    }

    public class TestViewModel
    {
        public TestItemDto[] TestItemDtos { get; set; }
    }

    public class TestItemDto
    {
        /// <summary>
        /// 資料表 Priamry Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 清單項目的 Id
        /// </summary>
        public int? ForeignKeyId { get; set; }

        public string Name { get; set; }
    }
}
