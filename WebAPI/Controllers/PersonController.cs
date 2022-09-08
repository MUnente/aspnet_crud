using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repository;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        // Request: (GET) api/person
        [Route(""), HttpGet]
        public IActionResult Get([FromQuery] int? id = null, [FromQuery] int? filterType = null, [FromQuery] string? filterValue = null, [FromQuery] int? page = null)
        {
            try
            {
                string? filterDescription = null;

                if (filterType != null)
                {
                    if (String.IsNullOrEmpty(filterValue))
                        throw new Exception("Valor para filtragem não informado.");

                    switch (filterType)
                    {
                        case (int)EFilterType.Id:
                            filterDescription = EFilterType.Id.ToString();
                            break;
                        case (int)EFilterType.Username:
                            filterDescription = EFilterType.Username.ToString();
                            break;
                        case (int)EFilterType.Fullname:
                            filterDescription = EFilterType.Fullname.ToString();
                            break;
                        case (int)EFilterType.Fulldate:
                            filterDescription = EFilterType.Fulldate.ToString();
                            break;
                        default:
                            throw new Exception("Tipo de filtragem inexistente.");
                    }
                }

                List<Person> people = new PersonRepository().SelectPeople(id, filterDescription, filterValue);
                return Ok(people);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Result = "Error",
                    Message = ex.Message
                });
            }
        }

        // Request: (POST) api/person
        [Route(""), HttpPost]
        public IActionResult Insert([FromBody] JsonElement body)
        {
            try
            {
                Person? person = JsonSerializer.Deserialize<Person>(body);

                if (person == null)
                    throw new Exception("Body não informado.");

                person = new PersonRepository().InsertPerson(person);

                if (person.Id == 0)
                    throw new Exception("Não foi possível cadastrar a entidade na base de dados.");

                return Ok(new
                {
                    Result = "Success",
                    Message = "Dados cadastrados com sucesso.",
                    Content = person
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Result = "Error",
                    Message = ex.Message
                });
            }
        }

        // Request: (PUT) api/person/{id}
        [Route("{id}"), HttpPut]
        public IActionResult Put([FromRoute] int id, [FromBody] JsonElement body)
        {
            try
            {
                Person? person = JsonSerializer.Deserialize<Person>(body);

                if (id == 0)
                    throw new Exception("Rota mal informada.");

                if (person == null)
                    throw new Exception("Body não informado.");

                person.Id = id;

                person = new PersonRepository().UpdatePerson(person);

                return Ok(new
                {
                    Result = "Success",
                    Message = "Dados atualizados com sucesso.",
                    Content = person
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Result = "Error",
                    Message = ex.Message
                });
            }

        }

        // Request: (DELETE) api/person/{id}
        [Route("{id}"), HttpDelete]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("Rota mal informada");

                bool isPersonDeleted = new PersonRepository().DeletePerson(id);

                if (!isPersonDeleted)
                    throw new Exception("Não foi possível excluir a entidade da base de dados.");

                return Ok(new
                {
                    Result = "Success",
                    Message = "A entidade foi excluída da base de dados."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Result = "Error",
                    Message = ex.Message
                });
            }
        }
    }
}
