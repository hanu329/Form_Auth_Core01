using Microsoft.AspNetCore.Mvc;
using FormAuthCore.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;


namespace FormAuthCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BasicsApiController : ControllerBase
    {
        // GET: api/<BasicsApiController>
        private readonly FormCoreAuthContext _context;
        public BasicsApiController(FormCoreAuthContext context)
        {
            _context = context;
        }

        [Authorize(Roles="admin")]
        [HttpGet("userData")]
        public IActionResult Get()
        {
            var res = _context.AuthUsers.ToList();
         
            return Ok(res);
            
        }
        [HttpGet("demo")]
        public IActionResult Demo()
        {
            var res = _context.AuthUsers.ToList();
           // var json = JsonConvert.SerializeObject(res);

            return Ok(res);

        }
        [HttpGet("listArr")]
        public List<object> listArr()
        {
            List<object> obj = new List<object>()
          {

              new {
                id= new List<object>
                {
                  
                   new {iid=1, iname="ravi"},
                   new {iid=2, iname="raviA"}
                },
                 
              },
             
              new List<object>
              {
                  new{ido=1, nameo="three"},
                  new{ido=2, nameo="four"}
              }
          };

            return obj;

        }
        public class clsA { };
        public class clsC { };
        public class clsB { };
        
        [HttpGet("clsArr")]
        /* 
         * public B b{get; set;}
         * public C c{get; set;}
         pubilc IActionResult clsArr(){
        var res = db.A.Select(x=> new A(){
              name=x.name,
              b = new B(){
             bname= x.b.bname,
              c= new C(){
            cname = x.b.c.cname
        }
        }
          }
        }

         */
        [HttpGet("userData/{id}")]
        public IActionResult Get(int id)
        {
            var res = _context.AuthUsers.Find(id);
           
            return Ok(res);
        }

        // POST api/<BasicsApiController>
        [HttpPost("userData")]
        public IActionResult Post([FromBody] User model)
        {
            _context.AuthUsers.Add(model);
            _context.SaveChanges();
            return Ok(model);
        }

        // PUT api/<BasicsApiController>/5
        [HttpPut("edit/{id}")]
        //[FromQuery]
        //int id
        public IActionResult Put([FromBody] User model)
        {
            _context.AuthUsers.Update(model);
            _context.SaveChanges();
            return Ok(model);
        }

        // DELETE api/<BasicsApiController>/5
        [HttpDelete("remove/{id}")]
        public IActionResult Delete(int id)
        {
            var res = _context.AuthUsers.Find(id);
            _context.AuthUsers.Remove(res);
            _context.SaveChanges();
            return Ok(res);
            
        }
    }
}
