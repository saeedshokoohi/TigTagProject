using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class InvoiceRepository :
        GenericRepository<DataModelContext, Invoice>, IInvoiceRepository
    {
        PageRepository pageRepo = new PageRepository();

        public Invoice GetSingle(Guid Id)
        {

            var query = Context.Invoices.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public Invoice FindByAuthority(string authority)
        {
            var invoices= Context.Invoices.Where(x => x.InvoiceTransactions.Any(t=>t.Authority==authority));
            if (invoices.Count() > 0) return invoices.First();
            else return null;
        }

        public void UpdateInvoiceStatus(Guid invoiceId, int status, string authority, long refID)
        {

           if(status==100)
            {
                Context.Invoices.Where(i => i.Id == invoiceId).ToList().ForEach(i => i.IsPaid = true);
              var transaction=  Context.InvoiceTransactions.Where(it => it.Authority == authority && it.InvoiceId == invoiceId).ToList();
                if(transaction.Count()>0)
                {
                    transaction[0].StatusCode = 100;
                    transaction[0].RefId = refID.ToString();
                    
                }
                Context.SaveChanges();
            }
           else
            {

            }
        }
    }
       
}