using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExample.CacheService;
using RedisExample.Models;

namespace RedisExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books;
        private ICacheService _cacheService;

        public BooksController(ICacheService cacheService)
        {
            _cacheService = cacheService;

            InitBookList();
        }

        public IActionResult Index()
        {
            if (_cacheService.Any("books"))
            {
                var books = _cacheService.Get<List<Book>>("books");
                return Ok(books);
            }

            _cacheService.Add("books", _books);

            return Ok(_books);
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            _books.Add(book);
            _cacheService.Remove("books");

            return Ok("Books Added");
        }

        private void InitBookList()
        {

            if (_books == null)
            {
                _books = new List<Book>();
                _books.Add(new Book(1, "Suç ve Ceza", "Dostoyevski"));
                _books.Add(new Book(2, "Totem ve Tabu", "Freud"));
                _books.Add(new Book(3, "Devrim Psikolojisi", "Le Bon"));
                _books.Add(new Book(4, "Yaşam Bilgisi", "Adler"));
            }
        }
    }
}
