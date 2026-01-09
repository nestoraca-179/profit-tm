using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Account : ProfitAdmManager
    {
        public saCuentaIngEgr GetAccountByID(string id)
        {
            saCuentaIngEgr account;

            try
            {
                account = db.saCuentaIngEgr.AsNoTracking().SingleOrDefault(c => c.co_cta_ingr_egr == id);
            }
            catch (Exception ex)
            {
                account = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTA " + id, ex);
            }

            return account;
        }

        public saCuentaBancaria GetBankAccountByID(string id)
        {
            saCuentaBancaria bankAccount;

            try
            {
                bankAccount = db.saCuentaBancaria.AsNoTracking().SingleOrDefault(c => c.cod_cta == id);
            }
            catch (Exception ex)
            {
                bankAccount = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTA BANCARIA " + id, ex);
            }

            return bankAccount;
        }

        public scGastos GetExpenseAccountByID(string id)
		{
            scGastos expenseAccount;

            try
            {
                expenseAccount = db.scGastos.AsNoTracking().SingleOrDefault(c => c.co_gas == id);
            }
            catch (Exception ex)
            {
                expenseAccount = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTA DE GASTO " + id, ex);
            }

            return expenseAccount;
        }
        
        public List<saCuentaIngEgr> GetAllAccounts()
        {
            List<saCuentaIngEgr> accounts;

            try
            {
                accounts = db.saCuentaIngEgr.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                accounts = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTAS", ex);
            }

            return accounts;
        }

        public List<saCuentaBancaria> GetAllBankAccounts(bool onlyActive)
        {
            List<saCuentaBancaria> accounts;

            try
            {
                string acc_ish = "0134-0205-12-20-53024949";
                string acc_its = "0134-0205-10-2053025089";
                accounts = db.saCuentaBancaria.AsNoTracking().Where(c => c.cod_cta != "0190-1" && c.num_cta != acc_ish && c.num_cta != acc_its).ToList();
                if (onlyActive)
                    accounts = accounts.Where(a => !a.inactivo).ToList();
            }
            catch (Exception ex)
            {
                accounts = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTAS BANCARIAS", ex);
            }

            return accounts;
        }

        public List<scGastos> GetAllExpenseAccounts()
        {
            List<scGastos> accounts;

            try
            {
                accounts = db.scGastos.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                accounts = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTAS DE GASTO", ex);
            }

            return accounts;
        }
    }
}