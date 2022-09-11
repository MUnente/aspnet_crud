using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebMVC.Models;
using X.PagedList;

namespace WebMVC.Controllers
{
    public class PersonController : Controller
    {
        public async Task<IActionResult> Index([FromQuery] string? filterType = null, [FromQuery] string? filterValue = null, [FromQuery] int? pageNumber = 1)
        {
            try
            {
                IEnumerable<Person>? people = new List<Person>();
                HttpRequestMessage request = new();
                HttpResponseMessage response = new();
                string peopleJson;
                int pageSize = 5;
                UriBuilder requestUriBuilder = new($"{Api.URLApi}/person");

                if (!String.IsNullOrEmpty(filterType) && !String.IsNullOrEmpty(filterValue))
                    requestUriBuilder.Query = $"{requestUriBuilder.Query}?filterType={filterType}&filterValue={filterValue}";

                using (HttpClient client = new())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = requestUriBuilder.Uri;

                    response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    peopleJson = await response.Content.ReadAsStringAsync();
                }

                people = JsonSerializer.Deserialize<List<Person>>(peopleJson);

                pageNumber = pageNumber ?? 1;

                return View(people.ToPagedList((int)pageNumber, pageSize));
            }
            catch (HttpRequestException ex)
            {
                ErrorViewModel errorView = new ErrorViewModel { ErrorMessage = ex.Message };
                return View("Error", errorView);
            }
        }

        public async Task<IActionResult> FormPerson([FromRoute] int? id = null)
        {
            try
            {
                Person person = new Person { Role = new() };

                if (id != null && id != 0)
                {
                    HttpRequestMessage request = new();
                    HttpResponseMessage response = new();
                    string personJson;
                    UriBuilder requestUriBuilder = new($"{Api.URLApi}/person");

                    requestUriBuilder.Query = $"{requestUriBuilder.Query}?id={id}";

                    using (HttpClient client = new())
                    {
                        request.Method = HttpMethod.Get;
                        request.RequestUri = requestUriBuilder.Uri;

                        response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        personJson = await response.Content.ReadAsStringAsync();
                    }

                    person = JsonSerializer.Deserialize<List<Person>>(personJson).FirstOrDefault();
                }

                return View(person);
            }
            catch (HttpRequestException ex)
            {
                ErrorViewModel errorView = new ErrorViewModel { ErrorMessage = ex.Message };
                return View("Error", errorView);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FormPerson(Person person)
        {
            try
            {
                HttpRequestMessage request = new();
                HttpResponseMessage response = new();

                using (HttpClient client = new())
                {
                    if (person.Id != null)
                    {
                        request.Method = HttpMethod.Put;
                        request.RequestUri = new Uri($"{Api.URLApi}/person/{person.Id}");
                    }
                    else
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri($"{Api.URLApi}/person");
                    }
                    request.Content = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");

                    response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                }

                return RedirectToAction("Index", "Person");
            }
            catch (Exception ex)
            {
                ErrorViewModel errorView = new ErrorViewModel { ErrorMessage = ex.Message };
                return View("Error", errorView);
            }
        }

        public async Task<IActionResult> DeletePerson([FromRoute] int? id = null)
        {
            try
            {
                HttpRequestMessage request = new();
                HttpResponseMessage response = new();

                using (HttpClient client = new())
                {
                    request.Method = HttpMethod.Delete;
                    request.RequestUri = new Uri($"{Api.URLApi}/person/{id}");

                    response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                }

                return RedirectToAction("Index", "Person");
            }
            catch (HttpRequestException ex)
            {
                ErrorViewModel errorView = new ErrorViewModel { ErrorMessage = ex.Message };
                return View("Error", errorView);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}