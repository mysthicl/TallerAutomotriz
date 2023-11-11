using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.BLL.Interfaces;
using Taller.DAL.Implementacion;
using Taller.DAL.Interfaz;
using Taller.Entity;

namespace Taller.BLL.Implementacion
{
    public class VehiculosServices : IVehiculosServices
    {
        private readonly IGenericRepository<TblCarro> _repository;
        private readonly IGenericRepository<TblHistorialCarro> _repositoryReparacion;
        private readonly IGenericRepository<TblReparacion> _Reparacion;

        public VehiculosServices(IGenericRepository<TblCarro> repository, IGenericRepository<TblHistorialCarro> repositoryReparacion, IGenericRepository<TblReparacion> Reparacion)
        {
            _repository = repository;
            _repositoryReparacion= repositoryReparacion;
            _Reparacion= Reparacion;
        }
        public async Task<TblCarro> Crear(TblCarro carro)
        {

            TblCarro carroExiste = await _repository.Obtener(p => p.Placa == carro.Placa);

            if (carroExiste != null)
            {
                throw new TaskCanceledException("El vehiculo ya existe");
            }


            try
            {
              

                TblCarro carroCreate = await _repository.Crear(carro);

                if (string.IsNullOrEmpty(carro.Placa))
                {
                    throw new TaskCanceledException("No se pudo registrar el Vehiculo");
                }


                return carroCreate;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<TblCarro> Editar(TblCarro carro)
        {
            TblCarro carroExiste = await _repository.Obtener(u => u.Placa == carro.Placa && u.IdCarro != carro.IdCarro);

            if (carroExiste != null)
            {
                throw new TaskCanceledException("El vehiculo ya existe");
            }

            try
            {
                IQueryable<TblCarro> query = await _repository.Consultar(p => p.IdCarro == carro.IdCarro);
                TblCarro carroEdit = query.First();


                carroEdit.Placa = carro.Placa;
                carroEdit.Ano = carro.Ano;
                carroEdit.Modelo = carro.Modelo;
				carroEdit.Marca = carro.Marca;




				bool resp = await _repository.Editar(carroEdit);

                if (!resp)
                    throw new TaskCanceledException("No se pudo modificar el servicio");

                TblCarro UsuarioEditado = query.First();

                return carroEdit;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                TblCarro carroEncontrado = await _repository.Obtener(p => p.IdCarro == id);
                if (carroEncontrado == null)
                    throw new TaskCanceledException("El vehiculo no existe");

                
                bool res = await _repository.Eliminar(carroEncontrado);


                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public Task<List<TblHistorialCarro>> Historial(string placa)
        {
            throw new NotImplementedException();
        }

        public Task<List<TblHistorialCarro>> HistorialReparacion()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TblCarro>> lista()
        {

            IQueryable<TblCarro> query = await _repository.Consultar();

            return query.ToList();
        }
    }
}
