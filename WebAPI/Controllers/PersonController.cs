using Microsoft.AspNetCore.Mvc;
using aspnet_crud.Models;
using aspnet_crud.Repository;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        // Request: (GET) api/person
        [Route(""), HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Person> people = new PersonRepository().SelectPeople();
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

        // Request: (PUT) api/person
        [Route("{id}"), HttpPut]
        public IActionResult Put(int id, [FromBody] JsonElement body)
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
        public IActionResult Delete(int id)
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
