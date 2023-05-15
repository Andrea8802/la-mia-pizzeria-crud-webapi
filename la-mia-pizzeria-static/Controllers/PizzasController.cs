using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Data.Entity;

namespace la_mia_pizzeria_static.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {

        // Search for Name
        [HttpGet]
        public IActionResult StringSearch(string? word)
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (word == null)
                {
                    IQueryable<Pizza> pizza = db.Pizza;

                    return Ok(pizza.ToList());
                }

                List<Pizza> pizze = db.Pizza.Where(pizze => pizze.Nome.Contains(word)).ToList();

                if (pizze.Count <= 0)
                {
                    return NotFound();
                }

                return Ok(pizze);
            }
        }

        // Search for ID
        [HttpGet("{id}")]
        public IActionResult IdSearch(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(p => p.Id == id).Include(p => p.Category).Include(p=>p.Ingredients).FirstOrDefault();

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

                    pizza.Ingredients = new List<Ingredient>();

                    if (data.Ingredients != null)
                    {
                        foreach (Ingredient ingredient in data.Ingredients)
                        {
                            Ingredient newIngredient = db.Ingredients.Where(ing => ing.Id == ingredient.Id).FirstOrDefault();
                            pizza.Ingredients.Add(newIngredient);
                        }
                    }
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
                    Pizza pizza = db.Pizza.Where(p => p.Id == id).Include(p => p.Category).Include(p => p.Ingredients).FirstOrDefault();

                    if (pizza == null)
                        return NotFound();

                    pizza.Nome = data.Nome;
                    pizza.Descrizione = data.Descrizione;
                    pizza.Prezzo = data.Prezzo;
                    pizza.Img = data.Img;

                    pizza.CategoryId = data.CategoryId;

                    pizza.Category = db.Categories.Where(category => category.Id == data.CategoryId).FirstOrDefault();

                    pizza.Ingredients = new List<Ingredient>();
                    
                    if (data.Ingredients != null)
                    {
                        foreach (Ingredient ingredient in data.Ingredients)
                        {
                            Ingredient newIngredient = db.Ingredients.Where(ing => ing.Id == ingredient.Id).FirstOrDefault();
                            pizza.Ingredients.Add(newIngredient);
                        }
                    }

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
        [HttpDelete("{id}")]
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
