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
    public class VentasBLL
    {
        public static bool Guardar(Ventas ventas)
        {
            if (!Existe(ventas.VentaId))
                return Insertar(ventas);
            else
                return Modificar(ventas);
        }

        private static bool Existe(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                paso = contexto.Ventas.Any(x => x.VentaId == id);
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

        private static bool Insertar(Ventas ventas)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                contexto.Ventas.Add(ventas);
                paso = (contexto.SaveChanges() > 0);
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

        private static bool Modificar(Ventas ventas)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                contexto.Entry(ventas).State = EntityState.Modified;
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
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                var ventas = Buscar(id);

                contexto.Entry(ventas).State = EntityState.Deleted;
                paso = (contexto.SaveChanges() > 0);

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

        public static Ventas Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Ventas ventas;

            try
            {
                ventas = contexto.Ventas.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return ventas;
        }

        public static List<Ventas> GetList(Expression<Func<Ventas, bool>> criterio)
        {
            List<Ventas> Lista = new List<Ventas>();
            Contexto contexto = new Contexto();

            try
            {
                Lista = contexto.Ventas.Where(criterio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Lista;
        }
    }
}
