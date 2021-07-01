using Microsoft.EntityFrameworkCore;
using Parcial2_apd2_20180906.DAL;
using Parcial2_apd2_20180906.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Parcial2_apd2_20180906.BLL
{
    public class CobrosBLL
    {
        public static bool Guardar(Cobros cobros)
        {
            if (!Existe(cobros.CobroId))
                return Insertar(cobros);
            else
                return Modificar(cobros);
        }

        private static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool paso = false;

            try
            {
                paso = contexto.Cobros.Any(e => e.CobroId == id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return paso;
        }

        private static bool Insertar(Cobros cobros)
        {
            Contexto contexto = new Contexto();
            bool paso = false;

            try
            {
                foreach (var item in cobros.Detalle)
                {
                    item.Venta = contexto.Ventas.Find(item.VentaId);
                    item.Venta.Balance -= item.Cobrado;
                    VentasBLL.Guardar(item.Venta);
                }

                contexto.Cobros.Add(cobros);
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return paso;
        }

        private static bool Modificar(Cobros cobros)
        {
            Contexto contexto = new Contexto();
            bool paso = false;

            try
            {
                contexto.Database.ExecuteSqlRaw($"Delete from CobrosDetalle where CobroId = {cobros.CobroId}");

                foreach (var item in cobros.Detalle)
                {
                    contexto.Entry(item).State = EntityState.Added;
                }

                contexto.Entry(cobros).State = EntityState.Modified;
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return paso;
        }

        public static bool Eliminar(int id)
        {
            Contexto contexto = new Contexto();
            bool paso = false;

            try
            {
                var cobros = Buscar(id);
                if (cobros != null)
                {
                    contexto.Cobros.Remove(cobros);
                    paso = contexto.SaveChanges() > 0;

                    if (paso)
                    {
                        foreach (var cobroDetalle in cobros.Detalle)
                        {
                            var venta = VentasBLL.Buscar(cobroDetalle.VentaId);
                            if(venta != null)
                            {
                                venta.Balance += cobroDetalle.Cobrado;
                                VentasBLL.Guardar(venta);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return paso;
        }

        public static Cobros Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Cobros cobros;

            try
            {
                cobros = contexto.Cobros
                    .Include(x => x.Detalle)
                    .ThenInclude(Detalle => Detalle.Venta)
                    .Include(x => x.Cliente)
                    .Where(x => x.CobroId == id)
                    .SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return cobros;
        }

        public static List<Cobros> GetList(Expression<Func<Cobros, bool>> criterio)
        {
            Contexto contexto = new Contexto();
            List<Cobros> lista = new List<Cobros>();

            try
            {
                lista = contexto.Cobros.Where(criterio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return lista;
        }
    }
}
