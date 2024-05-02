using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private IPeopleService _peopleService;

        public PeopleController([FromKeyedServices("people2Service")] IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        [HttpGet("all")]
        public List<People> GetPeople() => Repository.PeopleList;

        [HttpGet("{id}")]
        public ActionResult<People> Get(int id)
        {
            var people = Repository.PeopleList.FirstOrDefault(p => p.Id == id);
            if (people == null) return NotFound();
            return people;
        }

        [HttpGet("search/{search}")]
        public List<People> Get(string search) => Repository.PeopleList.Where(p => p.Name.ToUpper().Contains(search.ToUpper())).ToList();

        [HttpPost]
        public IActionResult Add(People people)
        {
            if (!_peopleService.Validate(people)) return BadRequest("El nombre es requerido");
            Repository.PeopleList.Add(people);
            return NoContent();
        }
    }

    public class Repository
    {
        public static List<People> PeopleList =
        [
            new() {Id=1, Name="Pedro", Birthdate = new DateTime(1990,12,3)},
            new() {Id=2, Name="Luis", Birthdate = new DateTime(1990,12,3)},
            new() {Id=3, Name="Ana", Birthdate = new DateTime(1990,12,3)},
            new() {Id=4, Name="Hugo", Birthdate = new DateTime(1990,12,3)},
        ];
    }


    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }
}