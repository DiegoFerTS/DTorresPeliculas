using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dulceria
    {

        public static ML.Result GetAll()
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Dulceria.FromSqlRaw("DulceriaGetAll").ToList();
                    resultado.Objects = new List<object>();

                    if (query.Count > 0)
                    {

                        foreach (var dulceriaResult in query)
                        {
                            ML.Dulceria dulceria = new ML.Dulceria();

                            dulceria.Id = dulceriaResult.Id;
                            dulceria.Nombre = dulceriaResult.Nombre;
                            dulceria.Precio = dulceriaResult.Precio;
                            dulceria.Descripcion = dulceriaResult.Descripcion;
                            dulceria.Imagen = dulceriaResult.Imagen;

                            resultado.Objects.Add(dulceria);
                        }

                        resultado.Correct = true;
                    } else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontraron datos de ningun producto de dulceria.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }


        public static ML.Result GetById(int idProducto)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Dulceria.FromSqlRaw($"DulceriaGetById {idProducto}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        resultado.Objects = new List<object>();

                        ML.Dulceria dulceria = new ML.Dulceria();

                        dulceria.Id = query.Id;
                        dulceria.Nombre = query.Nombre;
                        dulceria.Precio = query.Precio;
                        dulceria.Descripcion = query.Descripcion;
                        dulceria.Imagen = query.Imagen;

                        resultado.Object = dulceria;

                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontraron datos de ningun producto de dulceria.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }

    }
}
