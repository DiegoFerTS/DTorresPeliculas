using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Cines.FromSqlRaw("CineGetAll").ToList();

                    if (query.Count > 0)
                    {
                        resultado.Objects = new List<object>();

                        foreach (var cineResult in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona();

                            cine.Id = cineResult.IdCine;
                            cine.Nombre = cineResult.Nombre;
                            cine.Direccion = cineResult.Direccion;
                            cine.Zona.Id = cineResult.IdZona.Value;
                            cine.Zona.Nombre = cineResult.NombreZona;
                            cine.Ventas = cineResult.Ventas.Value;

                            resultado.Objects.Add(cine);
                        }

                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontraron datos de algun cine";
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

        public static ML.Result GetById(int idCine)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Cines.FromSqlRaw($"CineGetById {idCine}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.Zona = new ML.Zona();

                        cine.Id = query.IdCine;
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Zona.Id = query.IdZona.Value;
                        cine.Zona.Nombre = query.NombreZona;
                        cine.Ventas = query.Ventas.Value;

                        resultado.Object = cine;

                        resultado.Correct = true;

                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontro el cine.";
                    }
                }
            } catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }



        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}'," +
                                                                         $"'{cine.Direccion}'," +
                                                                         $"{cine.Zona.Id}," +
                                                                         $"{cine.Ventas}");

                    if (query > 0)
                    {
                        resultado.Correct = true;
                    } else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se pudo registrar el cine: " + cine.Nombre;
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


        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Database.ExecuteSqlRaw($"CineUpdate {cine.Id}," +
                                                                            $"'{cine.Nombre}'," +
                                                                            $"'{cine.Direccion}'," +
                                                                            $"{cine.Zona.Id}," +
                                                                            $"{cine.Ventas}");

                    if (query > 0)
                    {
                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se pudo actualizar el cine: " + cine.Nombre;
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


        public static ML.Result Delete(int idCine)
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Database.ExecuteSqlRaw($"CineDelete {idCine}");

                    if (query > 0)
                    {
                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se pudo eliminar el cine con el id: " + idCine;
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