using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;


namespace PL.Controllers
{
    public class DulceriaController : Controller
    {

        // Vista de dulceria en duro
        public IActionResult Dulceria()
        {
            return View();
        }


        // Vista de dulceria con base de datos
        public IActionResult GetAll()
        {
            ML.Result resultado = BL.Dulceria.GetAll();
            ML.Dulceria dulceria = new ML.Dulceria();
            dulceria.Productos = new List<object>();
            dulceria.Productos = resultado.Objects;
            return View(dulceria);
        }


        public IActionResult Carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }

        }

        public IActionResult LimpiarCarrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));

            return RedirectToAction("Carrito");

        }

        public IActionResult QuitarProducto(int idProducto)
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            GetCarrito(carrito);

            bool existe = false;

            foreach (ML.Dulceria productoDulceria in carrito.Carrito)
            {
                if (productoDulceria.Id == idProducto)
                {
                    carrito.Carrito.Remove(productoDulceria);
                    existe = true;
                    break;
                }
            }

            if (existe)
            {
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }


            return RedirectToAction("Carrito");

        }

        public IActionResult AgregarProducto(int idProducto)
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result resultado = BL.Dulceria.GetById(idProducto);
            ML.Dulceria producto = (ML.Dulceria)resultado.Object;

            GetCarrito(carrito);

            bool existe = false;


            foreach (ML.Dulceria productoDulceria in carrito.Carrito)
            {
                if (producto.Id == productoDulceria.Id)
                {
                    productoDulceria.Cantidad += 1;
                    existe = true;
                    break;
                }
                else
                {
                    existe = false;
                }
            }

            if (existe)
            {
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }


            return RedirectToAction("Carrito");
        }



        public IActionResult AddCarrito(int idProducto)
        {
            bool existe = false;

            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result resultado = BL.Dulceria.GetById(idProducto);

            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (resultado.Correct)
                {
                    ML.Dulceria producto = (ML.Dulceria)resultado.Object;

                    producto.Cantidad = 1;

                    carrito.Carrito.Add(producto);

                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            else
            {
                ML.Dulceria producto = (ML.Dulceria)resultado.Object;

                GetCarrito(carrito);


                foreach (ML.Dulceria productoDulceria in carrito.Carrito)
                {
                    if (producto.Id == productoDulceria.Id)
                    {
                        productoDulceria.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }

                if (existe)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
                else
                {
                    producto.Cantidad = 1;
                    carrito.Carrito.Add(producto);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }

            return RedirectToAction("GetAll");
        }



        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var objecto in ventaSession)
            {
                ML.Dulceria objetoDulceria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(objecto.ToString());
                carrito.Total += (objetoDulceria.Cantidad * objetoDulceria.Precio);
                carrito.Carrito.Add(objetoDulceria);
            }

            return carrito;
        }


        [HttpPost]
        public IActionResult CrearPDF(string? NumeroTarjeta, string? Direccion, string? Referencia, string? TotalPago)
        {
            // Recuperamos los productos del carrito
            ML.Venta venta = new ML.Venta();
            venta.Carrito = new List<object>();
            GetCarrito(venta);

            // Ubicacion en carpeta temporal del usuario en sesion de windows
            string rutaTemporalPdf = Path.GetTempFileName() + ".pdf";

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(rutaTemporalPdf)))
            {
                using (Document document = new Document(pdfDocument))
                {
                    document.Add(new Paragraph("Resumen de Compra"));

                    // Crear la tabla con 6 columnas
                    Table table = new Table(6);
                    // Anchura de la tabla
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    // Añadir las celdas de encabezado a la tabla
                    table.AddHeaderCell("ID Producto");
                    table.AddHeaderCell("Producto");
                    table.AddHeaderCell("Precio Unitario");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Subtotal");
                    table.AddHeaderCell("Imagen");


                    foreach (ML.Dulceria productoDulceria in venta.Carrito)
                    {
                        table.AddCell(productoDulceria.Id.ToString());
                        table.AddCell(productoDulceria.Nombre);
                        table.AddCell(productoDulceria.Precio.ToString());
                        table.AddCell(productoDulceria.Cantidad.ToString());
                        table.AddCell((productoDulceria.Cantidad * productoDulceria.Precio).ToString());

                        //byte[] imageBytes = Convert.FromBase64String(productoDulceria.Imagen);
                        // En mi casi agrego directamente el link de la imagen porque lo estoy usando imagenes por url
                        ImageData data = ImageDataFactory.Create(productoDulceria.Imagen);
                        Image image = new Image(data);
                        table.AddCell(image.SetWidth(50).SetHeight(50));
                    }

                    // Almacenamos la direccion del archivo en una sesion
                    HttpContext.Session.SetString("DireccionPdf", rutaTemporalPdf);
                    // Dividimos la direccion del archivo en un array para poder obtener el nombre del archivo con su extencion
                    string[] rutaTemporalPdfSplit = rutaTemporalPdf.Split("\\");
                    // Obtenemos el nombre del archivo
                    string nombreArchivo = rutaTemporalPdfSplit[rutaTemporalPdfSplit.Count() - 1];
                    // Se almacena el nombre del archivo en una sesion
                    HttpContext.Session.SetString("NombreArchivo", nombreArchivo);

                    // Añadir la tabla al documento
                    document.Add(table);
                    document.Add(new Paragraph("Numero de tarjeta: " + NumeroTarjeta));
                    document.Add(new Paragraph("Direccion: " + Direccion));
                    document.Add(new Paragraph("Referencia: " + Referencia));
                    document.Add(new Paragraph("Total a pagar: " + TotalPago));

                    // Limpiar carrito
                    ML.Venta carrito = new ML.Venta();
                    carrito.Carrito = new List<object>();
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }

            // Leer el archivo PDF como un arreglo de bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaTemporalPdf);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaTemporalPdf);
            HttpContext.Session.Remove("Carrito");

            // Descargar el archivo PDF
            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReporteProductos.pdf"
            };
            //return RedirectToAction("Carrito");
        }


    }
}
