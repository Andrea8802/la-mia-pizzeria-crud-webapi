using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {

        // Search for Name
        [HttpPut("{word}")]
        public IActionResult StringSearch(string word)
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizze = db.Pizza.Where(p => p.Nome.Contains(word)).ToList();

                if (pizze == null)
                    return NotFound();

                return Ok(pizze);
            }
        }


        // Search for ID
        [HttpPut("{id}")]
        public IActionResult IdSearch(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(p => p.Id == id).FirstOrDefault();

                if (pizza == null)
                    return NotFound();

                return Ok(pizza);
            }
        }

        // Create
        [HttpPost]
        public IActionResult Create([FromBody] Pizza data)
        {

            try
            {
                using (PizzaContext db = new PizzaContext())
                {
                    Pizza pizza = new Pizza();
                    pizza.Nome = data.Nome;
                    pizza.Descrizione = data.Descrizione;
                    pizza.Prezzo = data.Prezzo;
                    pizza.Img = data.Img;

                    pizza.CategoryId = data.CategoryId;

                    pizza.Category = db.Categories.Where(category => category.Id == data.CategoryId).FirstOrDefault();

                    db.Pizza.Add(pizza);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return UnprocessableEntity();
            }


        }

        // Edit
        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] Pizza data)
        {
            try
            {
                using (PizzaContext db = new PizzaContext())
                {
                    Pizza pizza = db.Pizza.Where(p => p.Id == id).FirstOrDefault();

                    if (pizza == null)
                        return NotFound();

                    pizza.Nome = data.Nome;
                    pizza.Descrizione = data.Descrizione;
                    pizza.Prezzo = data.Prezzo;
                    pizza.Img = data.Img;

                    pizza.CategoryId = data.CategoryId;

                    pizza.Category = db.Categories.Where(category => category.Id == data.CategoryId).FirstOrDefault();

                    db.Pizza.Update(pizza);
                    db.SaveChanges();

                    return Ok();

                }
            }
            catch
            {
                return UnprocessableEntity();
            }

        }

        // Delete
        [HttpPut("{id}")]
        public IActionResult Delete(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(p => p.Id == id).FirstOrDefault();

                if (pizza == null)
                    return NotFound();

                db.Pizza.Remove(pizza);
                db.SaveChanges();

                return Ok();

            }
        }
    }
}
