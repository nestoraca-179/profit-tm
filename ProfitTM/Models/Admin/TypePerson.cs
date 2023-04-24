using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class TypePerson : ProfitAdmManager
    {
        public saTipoCliente GetTypeClientByID(string id)
        {
            saTipoCliente typeClient;

            try
            {
                typeClient = db.saTipoCliente.AsNoTracking().SingleOrDefault(t => t.tip_cli == id);
            }
            catch (Exception ex)
            {
                typeClient = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPO DE CLIENTE " + id, ex);
            }

            return typeClient;
        }

        public saTipoProveedor GetTypeSupplierByID(string id)
        {
            saTipoProveedor typeSupplier;

            try
            {
                typeSupplier = db.saTipoProveedor.AsNoTracking().SingleOrDefault(t => t.tip_pro == id);
            }
            catch (Exception ex)
            {
                typeSupplier = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPO DE PROVEEDOR " + id, ex);
            }

            return typeSupplier;
        }

        public List<saTipoCliente> GetAllTypeClients()
        {
            List<saTipoCliente> typeClients;

            try
            {
                typeClients = db.saTipoCliente.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                typeClients = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPOS DE CLIENTE", ex);
            }

            return typeClients;
        }

        public List<saTipoProveedor> GetAllTypeSuppliers()
        {
            List<saTipoProveedor> typeSuppliers;

            try
            {
                typeSuppliers = db.saTipoProveedor.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                typeSuppliers = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPOS DE PROVEEDOR", ex);
            }

            return typeSuppliers;
        }
    }
}