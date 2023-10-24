using Microsoft.AspNetCore.Mvc;

namespace SLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result resultado = BL.Cine.GetAll();

            if (resultado.Correct)
            {
                return Ok(resultado);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("/{idCine}")]
        [HttpDelete]
        public ActionResult Delete(int idCine)
        {
            ML.Result resultado = BL.Cine.Delete(idCine);

            if (resultado.Correct)
            {
                return StatusCode(200, resultado);
            } else
            {
                return StatusCode(400);
            }
        }

        [Route("/{idCine}")]
        [HttpGet]
        public ActionResult GetById(int idCine)
        {
            ML.Result resultado = BL.Cine.GetById(idCine);

            if (resultado.Correct)
            {
                return StatusCode(200, resultado);
            } else
            {
                return StatusCode(400, resultado);
            }
        }

        [Route("")]
        [HttpPost]
        public ActionResult Add([FromBody]ML.Cine cine)
        {
            ML.Result resultado = BL.Cine.Add(cine);

            if (resultado.Correct)
            {
                return StatusCode(200, resultado);
            }
            else
            {
                return StatusCode(400, resultado);
            }
        }

        [Route("/{idCine}")]
        [HttpPut]
        public ActionResult Update(int idCine, [FromBody]ML.Cine cine)
        {
            cine.Id = idCine;
            ML.Result resultado = BL.Cine.Update(cine);

            if (resultado.Correct)
            {
                return StatusCode(200, resultado);
            }
            else
            {
                return StatusCode(400, resultado);
            }
        }
    }
}
